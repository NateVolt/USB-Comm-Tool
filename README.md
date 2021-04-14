# USB Comm Tool
This repo was created to help me develop a more portable solution for communicating with USB devices specifically USB Test and Measurement class devices. This approach will remove the need for NI VISA to be installed on every computer that needs to talk to USBTMC devices. 

# Setup

The overall architecture of this approach could look like this.

 ```
UsbDeviceImplementationClass (Device spcific interface written using the manufacturer's programming manual)
│
└───UsbCommunicationClass (Generic USB enumeration and communication class)
    │
    └───Windows.Devices.Usb (Built in USB library)
        │
        └───Winusb.sys (kernel level USB functionality)
 ```

UsbDeviceImplementationClass - This is a device specific class that implements all remote control commands specific to the device. Will use UsbCommunicationClass as the means to discover, connect, and talk to the physical device.

UsbCommunicationClass - This will be a generic communication class like [SerialPort](https://docs.microsoft.com/en-us/dotnet/api/system.io.ports.serialport?view=dotnet-plat-ext-3.1) that will likely live in a library.

Windows.Devices.Usb - This is the built in low level USB implementation that UsbCommunicationClass  will rely on.

Winusb.sys - Provides a generic USB driver that can be used for most USB devices that do not already have a driver that will facilitate communication.

## Source

This work is being based on the Microsoft tutorial ["Talking to USB devices, start to finish (UWP app)"](https://docs.microsoft.com/en-us/windows-hardware/drivers/usbcon/talking-to-usb-devices-start-to-finish).  We will be using [Winusb.sys](https://docs.microsoft.com/en-us/windows-hardware/drivers/usbcon/winusb-installation) in place of writing a custom driver. 

## Drivers

A [custom INF](https://docs.microsoft.com/en-us/windows-hardware/drivers/usbcon/winusb-installation#writing-a-custom-inf-for-winusb-installation) has been created to facilitate discovery and connection to the USB device. However, this INF will need to be paired with a [signed CAT file](https://docs.microsoft.com/en-us/windows-hardware/drivers/install/catalog-files). This signed CAT file is required to assign the driver to the USB device. The only alternative is to boot with the ["Disable Driver Signature Enforcement"](https://docs.microsoft.com/en-us/windows-hardware/drivers/install/test-signing#use-the-f8-advanced-boot-option) option. This will work for testing, but is not a valid solution for general use. 

## C# USB Setup

The USB functionality will be provided by [Windows.Devices.Usb](https://docs.microsoft.com/en-us/uwp/api/windows.devices.usb?view=winrt-19041). This namespace is part of UWP, so we must follow the [Nuget setup](https://docs.microsoft.com/en-us/windows/apps/desktop/modernize/desktop-to-uwp-enhance) to import the APIs into a Windows Forms App. After importing the nuget package (requires .net framework 4.6+), we must migrate from [packages.config to PackageReference](https://stackoverflow.com/a/59802372/8657981). To do this we just right click "References" in the solution explorer and select "Migrate packages.config to PackageReference". After doing this, build the application. Then we can use any of the UWP APIs in WFA, with the exception of those lacking the [DualApiPartition attribute](https://docs.microsoft.com/en-us/windows/win32/apiindex/uwp-apis-callable-from-a-classic-desktop-app#the-dualapipartition-attribute). 
