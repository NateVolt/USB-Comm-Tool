using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Usb;
using Windows.Storage.Streams;

namespace USB_Comm_Tool
{
    public class UsbComm : IDisposable
    {
        private UsbDevice usbDevice;
        private DataWriter writer;
        private DataReader reader;
        /// <summary>
        /// Determines how many bytes we will attempt to read from the buffer at once. We may, and often do, actually read fewer than this.
        /// 
        /// <para>This is currently an arbitrary number.</para>
        /// </summary>
        private const uint readerBufferSize = 64;
        /// <summary>
        /// Used to track the number of bulk transfers initiated which in turn is used to generate the bTag for subsequent bulk transfers. 
        /// This must be incremented for each transfer to comply with USB protocols.
        /// </summary>
        private byte messageCounter = 0;

        /// <summary>
        /// Creates a USB device from a <paramref name="deviceInformation"/> object.
        /// </summary>
        /// <param name="deviceInformation">The <see cref="DeviceInformation"/> of the device we wish to connect to.</param>
        /// <returns>A fully configured <see cref="UsbComm"/> device.</returns>
        public static async Task<UsbComm> CreateAsync(DeviceInformation deviceInformation)
        {
            return await CreateAsync(deviceInformation.Id);
        }

        /// <summary>
        /// Creates a USB device from a decvice ID string.
        /// </summary>
        /// <param name="deviceId">A string representing the device. Usually pulled from <see cref="DeviceInformation.Id"/>.</param>
        /// <returns>A fully configured <see cref="UsbComm"/> device.</returns>
        public static async Task<UsbComm> CreateAsync(string deviceId)
        {
            var newClass = new UsbComm();
            return await newClass.InitializeAsync(deviceId);
        }

        /// <summary>
        /// Handles all USB initialization tasks as well as class configuration.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        private async Task<UsbComm> InitializeAsync(string deviceId)
        {
            this.usbDevice = await UsbDevice.FromIdAsync(deviceId);
            this.writer = InitializeWriter();
            this.reader = InitializeReader();
            return this;
        }

        /// <summary>
        /// Gets the <see cref="DataWriter"/> for the Bulk-Out pipe.
        /// </summary>
        /// <returns>The bulk-out <see cref="DataWriter"/>.</returns>
        private DataWriter InitializeWriter()
        {
            UsbBulkOutPipe writePipe = usbDevice.DefaultInterface.BulkOutPipes[0];
            writePipe.WriteOptions |= UsbWriteOptions.AutoClearStall;
            return new DataWriter(writePipe.OutputStream);
        }

        /// <summary>
        /// Gets the <see cref="DataReader"/> for the Bulk-In pipe.
        /// </summary>
        /// <returns>The bulk-in <see cref="DataReader"/>.</returns>
        private DataReader InitializeReader()
        {
            UsbBulkInPipe readPipe = usbDevice.DefaultInterface.BulkInPipes[0];
            readPipe.ReadOptions |= UsbReadOptions.AutoClearStall;
            return new DataReader(readPipe.InputStream);
        }

        /// <summary>
        /// Empty constructor because we must use async to initialize the USB. So that setup is done in the static CreateAsync method.
        /// </summary>
        private UsbComm()
        {
            
        }

        public static async Task<DeviceInformationCollection> EnumerateDevices(UInt32 vid = 0x00, UInt32 pid = 0x00)
        {
            // VID and PID can be found by opening Device Manage > Right click device of interest > Properties > Details > Select Hardware Ids from the Property dropdown > Copy the values
            // VID and PID should not be used unless we wish to enumerate only specific devices from specific manufacturers. Otherwise, we should enumerate all in the test and measurement class devices.
            // A selector can be made from the VID and PID alone
            //UsbDevice.GetDeviceSelector(vid, pid);
            string selector = "";
            if (vid != 0x00 || pid != 0x00)
            {
                // If the caller specified and VID and/or PID we should enumerate using this.
                selector = UsbDevice.GetDeviceSelector(vid, pid);
            }
            else
            {
                // Otherwise, we should enumerate all USBTMC devices and let the caller filter the result as they see fit.
                selector = UsbDevice.GetDeviceClassSelector(UsbDeviceClasses.Measurement);
            }
            // We !MUST! use the selector to search for USB devices. If the selector created from VID / PID or Device Class does not return the device of interest, then it is not configured properly.
            // If we search for our device with a more broad selector, we may find the device, but when we try to connect to it "UsbDevice.FromIdAsync" will return null.
            return await DeviceInformation.FindAllAsync();
        }

        /// <summary>
        /// Sends the <paramref name="command"/> text without telling the device that a response to the command is required.
        /// </summary>
        /// <param name="command">The command string to send to the device.</param>
        /// <returns></returns>
        public async Task SendCommand(string command)
        {
            await sendMessage(new BulkTransfer(command, this.messageCounter, BulkTransfer.MsgId.DEV_DEP_MSG_OUT));
        }

        /// <summary>
        /// Sends the device an "empty" command that instructs the device to reply to the last command.
        /// </summary>
        /// <returns></returns>
        public async Task RequestResponse()
        {
            await sendMessage(new BulkTransfer("", this.messageCounter, BulkTransfer.MsgId.REQUEST_DEV_DEP_MSG_IN));
        }

        /// <summary>
        /// Sends the <paramref name="query"/> text and tells the device to reply to this query.
        /// </summary>
        /// <param name="query">The query string to send to the device.</param>
        /// <returns></returns>
        public async Task<string> SendQuery(string query)
        {
            await sendMessage(new BulkTransfer(query, this.messageCounter, BulkTransfer.MsgId.DEV_DEP_MSG_OUT));
            await sendMessage(new BulkTransfer("", this.messageCounter, BulkTransfer.MsgId.REQUEST_DEV_DEP_MSG_IN));
            return await ReadMessage();
        }

        /// <summary>
        /// Internal write method that all writes should go through to ensure that <see cref="messageCounter"/> is incremented properly. Failure to increment this counter can break USB communications.
        /// </summary>
        /// <param name="bulkTransfer">The <see cref="BulkTransfer"/> created from our command/query string.</param>
        /// <returns></returns>
        private async Task<uint> sendMessage(BulkTransfer bulkTransfer)
        {
            this.messageCounter++;
            writer.WriteBytes(bulkTransfer.ToBytes());
            return await writer.StoreAsync();
        }

        /// <summary>
        /// Reads the first message in the queue.
        /// </summary>
        /// <returns>A string representation of the data portion of the first message in the queue.</returns>
        public async Task<string> ReadMessage()
        {
            // load all available bytes in the reader up to a max of readerBufferSize.
            uint availableBytes = await reader.LoadAsync(readerBufferSize);
            // read the number of bytes loaded into a buffer
            IBuffer buffer = reader.ReadBuffer(availableBytes);
            // convert this raw data into the BulkTransfer that it represents
            BulkTransfer bulkTransfer = new BulkTransfer(buffer.ToArray());
            // Return the message contained in the parsed BulkTransfer
            return bulkTransfer.Message;
        }

        public void Dispose()
        {
            if (this.reader != null)
            {
                this.reader.Dispose();
            }
            if (this.writer != null)
            {
                this.writer.Dispose();
            }
            if (this.usbDevice != null)
            {
                this.usbDevice.Dispose();
            }
        }

        #region GetDescriptorStrings
        public string GetDeviceDescriptorAsString()
        {
            string content = null;

            var deviceDescriptor = this.usbDevice.DeviceDescriptor;

            content = "Device Descriptor\n"
                    + "\nUsb Spec Number : 0x" + deviceDescriptor.BcdUsb.ToString("X4", NumberFormatInfo.InvariantInfo)
                    + "\nMax Packet Size (Endpoint 0) : " + deviceDescriptor.MaxPacketSize0.ToString("D", NumberFormatInfo.InvariantInfo)
                    + "\nVendor ID : 0x" + deviceDescriptor.VendorId.ToString("X4", NumberFormatInfo.InvariantInfo)
                    + "\nProduct ID : 0x" + deviceDescriptor.ProductId.ToString("X4", NumberFormatInfo.InvariantInfo)
                    + "\nDevice Revision : 0x" + deviceDescriptor.BcdDeviceRevision.ToString("X4", NumberFormatInfo.InvariantInfo)
                    + "\nNumber of Configurations : " + deviceDescriptor.NumberOfConfigurations.ToString("D", NumberFormatInfo.InvariantInfo);

            return content;
        }

        public string GetConfigurationDescriptorAsString()
        {
            string content = null;

            var usbConfiguration = this.usbDevice.Configuration;
            var configurationDescriptor = usbConfiguration.ConfigurationDescriptor;

            content = "Configuration Descriptor\n"
                    + "\nNumber of Interfaces : " + usbConfiguration.UsbInterfaces.Count.ToString("D", NumberFormatInfo.InvariantInfo)
                    + "\nConfiguration Value : 0x" + configurationDescriptor.ConfigurationValue.ToString("X2", NumberFormatInfo.InvariantInfo)
                    + "\nSelf Powered : " + configurationDescriptor.SelfPowered.ToString()
                    + "\nRemote Wakeup : " + configurationDescriptor.RemoteWakeup.ToString()
                    + "\nMax Power (milliAmps) : " + configurationDescriptor.MaxPowerMilliamps.ToString("D", NumberFormatInfo.InvariantInfo);

            return content;
        }

        public string GetInterfaceDescriptorsAsString()
        {
            string content = null;

            var interfaces = this.usbDevice.Configuration.UsbInterfaces;

            content = "Interface Descriptors";

            foreach (UsbInterface usbInterface in interfaces)
            {
                // Class/subclass/protocol values from the first interface setting.

                UsbInterfaceDescriptor usbInterfaceDescriptor = usbInterface.InterfaceSettings[0].InterfaceDescriptor;

                content += "\n\nInterface Number: 0x" + usbInterface.InterfaceNumber.ToString("X2", NumberFormatInfo.InvariantInfo)
                        + "\nClass Code: 0x" + usbInterfaceDescriptor.ClassCode.ToString("X2", NumberFormatInfo.InvariantInfo)
                        + "\nSubclass Code: 0x" + usbInterfaceDescriptor.SubclassCode.ToString("X2", NumberFormatInfo.InvariantInfo)
                        + "\nProtocol Code: 0x" + usbInterfaceDescriptor.ProtocolCode.ToString("X2", NumberFormatInfo.InvariantInfo)
                        + "\nNumber of Interface Settings: " + usbInterface.InterfaceSettings.Count.ToString("D", NumberFormatInfo.InvariantInfo)
                        + "\nNumber of open Bulk In pipes: " + usbInterface.BulkInPipes.Count.ToString("D", NumberFormatInfo.InvariantInfo)
                        + "\nNumber of open Bulk Out pipes: " + usbInterface.BulkOutPipes.Count.ToString("D", NumberFormatInfo.InvariantInfo)
                        + "\nNumber of open Interrupt In pipes: " + usbInterface.InterruptInPipes.Count.ToString("D", NumberFormatInfo.InvariantInfo)
                        + "\nNumber of open Interrupt Out pipes: " + usbInterface.InterruptOutPipes.Count.ToString("D", NumberFormatInfo.InvariantInfo);
            }

            return content;
        }

        public string GetEndpointDescriptorsAsString()
        {
            string content = null;

            var usbInterface = this.usbDevice.DefaultInterface;
            var bulkInPipes = usbInterface.BulkInPipes;
            var bulkOutPipes = usbInterface.BulkOutPipes;
            var interruptInPipes = usbInterface.InterruptInPipes;
            var interruptOutPipes = usbInterface.InterruptOutPipes;

            content = "Endpoint Descriptors for open pipes";

            // Print Bulk In Endpoint descriptors
            foreach (UsbBulkInPipe bulkInPipe in bulkInPipes)
            {
                var endpointDescriptor = bulkInPipe.EndpointDescriptor;

                content += "\n\nBulk In Endpoint Descriptor"
                        + "\nEndpoint Number : 0x" + endpointDescriptor.EndpointNumber.ToString("X2", NumberFormatInfo.InvariantInfo)
                        + "\nMax Packet Size : " + endpointDescriptor.MaxPacketSize.ToString("D", NumberFormatInfo.InvariantInfo);
            }

            // Print Bulk Out Endpoint descriptors
            foreach (UsbBulkOutPipe bulkOutPipe in bulkOutPipes)
            {
                var endpointDescriptor = bulkOutPipe.EndpointDescriptor;

                content += "\n\nBulk Out Endpoint Descriptor"
                        + "\nEndpoint Number : 0x" + endpointDescriptor.EndpointNumber.ToString("X2", NumberFormatInfo.InvariantInfo)
                        + "\nMax Packet Size : " + endpointDescriptor.MaxPacketSize.ToString("D", NumberFormatInfo.InvariantInfo);
            }

            // Print Interrupt In Endpoint descriptors
            foreach (UsbInterruptInPipe interruptInPipe in interruptInPipes)
            {
                var endpointDescriptor = interruptInPipe.EndpointDescriptor;

                content += "\n\nInterrupt In Endpoint Descriptor"
                        + "\nEndpoint Number : 0x" + endpointDescriptor.EndpointNumber.ToString("X2", NumberFormatInfo.InvariantInfo)
                        + "\nMax Packet Size : " + endpointDescriptor.MaxPacketSize.ToString("D", NumberFormatInfo.InvariantInfo)
                        + "\nInterval : " + endpointDescriptor.Interval.Duration().ToString();
            }

            // Print Interrupt Out Endpoint descriptors
            foreach (UsbInterruptOutPipe interruptOutPipe in interruptOutPipes)
            {
                var endpointDescriptor = interruptOutPipe.EndpointDescriptor;

                content += "\n\nInterrupt Out Endpoint Descriptor"
                        + "\nEndpoint Number : 0x" + endpointDescriptor.EndpointNumber.ToString("X2", NumberFormatInfo.InvariantInfo)
                        + "\nMax Packet Size : " + endpointDescriptor.MaxPacketSize.ToString("D", NumberFormatInfo.InvariantInfo)
                        + "\nInterval : " + endpointDescriptor.Interval.Duration().ToString();
            }

            return content;
        }

        public string GetCustomDescriptorsAsString()
        {
            string content = null;
            // Descriptor information will be appended to this string and then printed to UI
            content = "Raw Descriptors";

            var configuration = this.usbDevice.Configuration;
            var allRawDescriptors = configuration.Descriptors;

            // Print first 2 bytes of all descriptors within the configuration descriptor    
            // because the first 2 bytes are always length and descriptor type
            // the UsbDescriptor's DescriptorType and Length properties, but we will not use these properties
            // in order to demonstrate ReadDescriptorBuffer() and how to parse it.

            foreach (UsbDescriptor descriptor in allRawDescriptors)
            {
                var descriptorBuffer = new Windows.Storage.Streams.Buffer(descriptor.Length);
                descriptor.ReadDescriptorBuffer(descriptorBuffer);

                DataReader reader = DataReader.FromBuffer(descriptorBuffer);

                // USB data is Little Endian according to the USB spec.
                reader.ByteOrder = ByteOrder.LittleEndian;

                // ReadByte has a side effect where it consumes the current byte, so the next ReadByte will read the next character.
                // Putting multiple ReadByte() on the same line (same variable assignment) may cause the bytes to be read out of order.
                var length = reader.ReadByte().ToString("D", NumberFormatInfo.InvariantInfo);
                var type = "0x" + reader.ReadByte().ToString("X2", NumberFormatInfo.InvariantInfo);

                content += "\n\nDescriptor"
                        + "\nLength : " + length
                        + "\nDescriptorType : " + type;
            }

            return content;
        }
        #endregion GetDescriptorStrings
        #region Bulk Transfer

        /// <summary>
        /// Represents all data related to a USBTMC Bulk Transfer. 
        /// <para>
        /// This class can generate a Bulk Transfer object from a string (the data) and some configuration parameters.
        /// This class can also parse received byte arrays into a formatted Bulk Transfer so that the <see cref="Message"/> can easily be read from them.
        /// </para>
        /// </summary>
        public class BulkTransfer
        {
            private const int HeaderSize = 12;
            private const char DefaultTerminationCharacter = '\n';

            /// <summary>
            /// Used to identify the type of transfer. I.E. Command or Query.
            /// </summary>
            private MsgId msgId;
            /// <summary>
            /// The transmit counter. Used to identify unique message transfers.
            /// </summary>
            private byte bTag;
            /// <summary>
            /// The one's complement of <see cref="bTag"/>.
            /// </summary>
            private byte bTagInverse;
            /// <summary>
            /// The byte length of the data contained in the transfer.
            /// </summary>
            private uint transferSize;
            /// <summary>
            /// Digit 7 through Digit 2 Reserved. All bits must be 0.
            /// 
            /// D1 =
            ///   1 – All of the following are true:
            ///     • The USBTMC interface supports TermChar
            ///     • The bmTransferAttributes. TermCharEnabled bit was set in the REQUEST_DEV_DEP_MSG_IN.
            ///     • The last USBTMC message data byte in this transfer matches the TermChar in the REQUEST_DEV_DEP_MSG_IN.
            ///   0 – One or more of the above conditions is not met.
            /// D0 EOM (End Of Message)
            ///   1 - The last USBTMC message data byte in the transfer is the last byte of the USBTMC message.
            ///   0 – The last USBTMC message data byte in the transfer is not the last byte of the USBTMC message.
            /// </summary>
            private BmTransferAttributes bmTransferAttributes;
            /// <summary>
            /// The transfer MAY, but usually does not, specify the termination character.
            /// </summary>
            private char terminationCharacter = (char)0x00;
            /// <summary>
            /// The string reperesentation of the data to transmit/recieve.
            /// </summary>
            private string message;

            /// <summary>
            /// String reperesentation of the data in the <see cref="BulkTransfer"/>.
            /// </summary>
            public string Message => message;
            /// <summary>
            /// Length of data in the <see cref="BulkTransfer"/>.
            /// </summary>
            public uint TransferSize => transferSize;

            /// <summary>
            /// Convert a string message (<paramref name="messageData"/>) into a formatted BulkTransfer.
            /// </summary>
            /// <param name="messageData">The message data to be transmitted.</param>
            /// <param name="bTagValue">The current transmit counter.</param>
            /// <param name="id">The type of message transaction.</param>
            /// <param name="terminationCharToUse">Only set this if you plan to assign <paramref name="attributes"/> with <see cref="BmTransferAttributes.TerminationCharacterSet"/>. Only change this if you know what you are doing.</param>
            /// <param name="attributes">Usually just left to the default. Only change this if you know what you are doing.</param>
            public BulkTransfer(string messageData, byte bTagValue, MsgId id, char terminationCharToUse = (char)0x00, BmTransferAttributes attributes = BmTransferAttributes.EndOfMessage)
            {
                this.message = messageData;
                this.msgId = id;
                this.bTag = bTagValue;
                // Take the ones complement using '~'
                this.bTagInverse = (byte)~this.bTag;
                this.transferSize = (uint)messageData.Length;
                this.bmTransferAttributes = attributes;
                // If they have specified a custom termination character use that. Otherwise this should ALWAYS be 0x00. Don't mess with this unless you know what you're doing.
                this.terminationCharacter = attributes.HasFlag(BmTransferAttributes.TerminationCharacterSet) ? terminationCharacter : (char)0x00;
            }

            /// <summary>
            /// Convert received data into a formatted <see cref="BulkTransfer"/>.
            /// </summary>
            /// <param name="rawData"></param>
            public BulkTransfer(byte[] rawData)
            {
                this.msgId = (MsgId)rawData[0];
                this.bTag = rawData[1];
                this.bTagInverse = rawData[2];
                // We ignore rawData[3] because it is a reserved byte that is always 0x00.
                this.transferSize = (uint)rawData[4] + (((uint)rawData[5]) << 8) + (((uint)rawData[6]) << 16) + (((uint)rawData[7]) << 24);
                this.bmTransferAttributes = (BmTransferAttributes)rawData[8];
                this.terminationCharacter = (char)rawData[9];
                if (this.terminationCharacter == 0x00 && this.transferSize == 0x00)
                {
                    // If we were not given a data length, determine the size of the data manually
                    // total message length minus header size gives us data length plus any alignment bytes
                    // then we subtract the total message length by the index of the data's last termination character to get the number of aligment bytes
                    // then we subtract the number of alignment bytes from the headerless length to get the total data length
                    uint totalLength = (uint)rawData.Length;
                    // we add one to this because index of is inclusive and we want to keep the termination character as part of the data (data count)
                    uint alignmentBytesStartPosition = (uint)(rawData.ToList().LastIndexOf((byte)DefaultTerminationCharacter) + 1);
                    uint transferSizeMinusHeader = (totalLength - HeaderSize);
                    uint numberOfAlignmentBytes = (totalLength - alignmentBytesStartPosition);

                    this.transferSize = transferSizeMinusHeader - numberOfAlignmentBytes;
                }
                // We ignore rawData[10] because it is a reserved byte that is always 0x00.
                // We ignore rawData[11] because it is a reserved byte that is always 0x00.
                // Skip the header data and take the data length (transferSize) and then concat it to a single string
                // We do not trim the termination character(s) because that is for higher level layers to handle
                this.message = string.Join("", rawData.Skip(HeaderSize).Take((int)transferSize).Select(x => (char)x));
            }

            /// <summary>
            /// Converts the <see cref="BulkTransfer"/> into bytes which can be directly transmitter on the Bulk-Out pipe.
            /// </summary>
            /// <returns>A byte representation of the <see cref="BulkTransfer"/>.</returns>
            public byte[] ToBytes()
            {
                List<byte> data = new List<byte>()
            {
                (byte)msgId,
                bTag,
                bTagInverse,
                0x00, // Reserved
                ((byte)(transferSize >> 0)),
                ((byte)(transferSize >> 8)),
                ((byte)(transferSize >> 16)),
                ((byte)(transferSize >> 24)),
                (byte)bmTransferAttributes,
                (byte)terminationCharacter,
                0x00, // Reserved
                0x00, // Reserved
            };
                // Add all data to transfer
                data.AddRange(message.Select(x => (byte)x));
                // Add 0 to 3 alignment bytes to make the total transfer size a multiple of 4. This is required by the USBTMC spec.
                data.AddRange(Enumerable.Repeat(0x00, 4 - data.Count % 4).Select(x => (byte)x));
                return data.ToArray();
            }

            public enum MsgId
            {
                /// <summary>
                /// Sends a command to the device.
                /// </summary>
                DEV_DEP_MSG_OUT = 1,
                /// <summary>
                /// Requests the device respond to the last command.
                /// </summary>
                REQUEST_DEV_DEP_MSG_IN = 2,
            }

            /// <summary>
            /// <para>This should usually be set to <see cref="BmTransferAttributes.EndOfMessage"/> only!</para>
            /// Used to indicate if this is the last tranfer in a message and if we are using a custom termination character.
            /// Top 6 bits are 0 - bit 1 is TermCharSetBit - bit 0 is EomBit
            /// </summary>
            [Flags]
            public enum BmTransferAttributes
            {
                /// <summary>
                /// 
                ///   <para>End Of Message Enabled IF The last USBTMC message data byte in the transfer is the last byte of the USBTMC message.</para>
                ///   <para>End Of Message Disabled IF The last USBTMC message data byte in the transfer is not the last byte of the USBTMC message.</para>
                /// </summary>
                EndOfMessage = 1,
                /// <summary>
                ///   <para>TerminationCharacterSet Enabled If All of the following are true:
                ///     • The USBTMC interface supports TermChar
                ///     • The bmTransferAttributes. TermCharEnabled bit was set in the REQUEST_DEV_DEP_MSG_IN.
                ///     • The last USBTMC message data byte in this transfer matches the TermChar in the REQUEST_DEV_DEP_MSG_IN.</para>
                ///   <para>TerminationCharacterSet Disabled IF One or more of the above conditions is not met.
                ///   We generally do not use this. AKA Nate has not figured out how this works.</para>
                /// </summary>
                TerminationCharacterSet = 2,
            }
        }
        #endregion Bulk Transfer
    }
}
