using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Enumeration;

namespace USB_Comm_Tool
{
    public partial class MainForm : Form
    {
        private UsbComm usb;
        private BindingList<KeyValuePair<string, DeviceInformation>> enumeratedDevices = new BindingList<KeyValuePair<string, DeviceInformation>>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.VersionLabel.Text += Application.ProductVersion;
            this.DiscoveredDevicesListBox.ValueMember = "Value";
            this.DiscoveredDevicesListBox.DisplayMember = "Key";
            this.DiscoveredDevicesListBox.DataSource = enumeratedDevices;
            this.DiscoveredDevicesListBox.DisplayMember = "Key";
            
        }

        private async void DiscoverDevicesButton_Click(object sender, EventArgs e)
        {
            UpdateUiState(true);
            try
            {
                enumeratedDevices.Clear();

                foreach (var entry in await UsbComm.EnumerateDevices(0x21D2, 0x32C1))
                {
                    if (enumeratedDevices.Where(x => x.Key == entry.Name).ToList().Count != 0) continue;
                    if (!entry.Id.Contains("USB")) continue;
                    //if (entry.)
                    enumeratedDevices.Add(new KeyValuePair<string, DeviceInformation>(entry.Name, entry));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\r\n\r\n{ex.StackTrace}");
            }
            UpdateUiState(false);

            void UpdateUiState(bool isDiscovering)
            {
                this.DiscoverDevicesButton.Enabled = !isDiscovering;
                this.ConnectToDeviceButton.Enabled = !isDiscovering;
                this.GetDescriptorsButton.Enabled = !isDiscovering;
            }
        }

        private async void ConnectToDeviceButton_Click(object sender, EventArgs e)
        {
            UpdateUiState(true);
            try
            {
                DeviceInformation selectedDeviceInfo = (DeviceInformation)this.DiscoveredDevicesListBox.SelectedValue;
                this.usb = await UsbComm.CreateAsync(selectedDeviceInfo);
                if (this.usb == null)
                {
                    throw new Exception($"Failed to initialize selected USB device '{selectedDeviceInfo.Name} with id '{selectedDeviceInfo.Id}''.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\r\n\r\n{ex.StackTrace}");
            }
            UpdateUiState(false);

            void UpdateUiState(bool isConnecting)
            {
                this.ConnectToDeviceButton.Enabled = !isConnecting;
                this.tabControl1.Enabled = (this.usb != null);
            }
        }

        private void GetDeviceDescriptorsButton_Click(object sender, EventArgs e)
        {
            if (this.usb != null)
            {
                try
                {
                    this.DeviceDescriptorsRichTextBox.Text = this.usb.GetDeviceDescriptorAsString();
                    this.ConfigurationDescriptorsRichTextBox.Text = this.usb.GetConfigurationDescriptorAsString();
                    this.InterfaceDescriptorsRichTextBox.Text = this.usb.GetInterfaceDescriptorsAsString();
                    this.EndpointDescriptorsRichTextBox.Text = this.usb.GetEndpointDescriptorsAsString();
                    this.CustomDescriptorsRichTextBox.Text = this.usb.GetCustomDescriptorsAsString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occured while Getting Descriptors.\r\n" +
                    $"\r\n" +
                    $"Error: {ex.Message}\r\n\r\n" +
                    $"StackTrace: {ex.Message}");
                }
            }
        }

        private async void SendButton_Click(object sender, EventArgs e)
        {
            if (this.usb == null)
            {
                return;
            }
            string command = this.SendRichTextBox.Text + "\n";
            //this.SendRichTextBox.Text = "";
            if (command != "")
            {
                try
                {
                    await this.usb.SendCommand(command);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occured while Sending Message.\r\n" +
                    $"\r\n" +
                    $"Error: {ex.Message}\r\n\r\n" +
                    $"StackTrace: {ex.Message}");
                }
            }
        }

        

        private async void ReceiveButton_Click(object sender, EventArgs e)
        {
            if (this.usb == null)
            {
                return;
            }
            try
            {
                this.ReceiveRichTextBox.Text = await this.usb.ReadMessage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured while Receiving Message.\r\n" +
                    $"\r\n" +
                    $"Error: {ex.Message}\r\n\r\n" +
                    $"StackTrace: {ex.Message}");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.usb != null)
            {
                this.usb.Dispose();
            }
        }

        private async void QueryButton_Click(object sender, EventArgs e)
        {
            if (this.usb == null)
            {
                return;
            }
            string query = this.SendRichTextBox.Text + "\n";
            try
            {
                this.ReceiveRichTextBox.Text = await this.usb.SendQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured while Querying.\r\n" +
                    $"\r\n" +
                    $"Error: {ex.Message}\r\n\r\n" +
                    $"StackTrace: {ex.Message}");
            }
        }

        private async void RequestResponseButton_Click(object sender, EventArgs e)
        {
            if (this.usb == null)
            {
                return;
            }
            try
            {
                await this.usb.RequestResponse();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured while Requesting Response.\r\n" +
                    $"\r\n" +
                    $"Error: {ex.Message}\r\n\r\n" +
                    $"StackTrace: {ex.Message}");
            }
        }
    }
}
