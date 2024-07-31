using System.Text;
using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System;
using System.IO.Ports;

class Program
{
    //[DllImport("kernel32.dll", SetLastError = true)]
    //static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

    static void Main()
    {
        SerialPort serialPort = new SerialPort("COM11", 9600, Parity.None, 8, StopBits.One)
        {
            DtrEnable = true, // Configurar DTR/DSR
            RtsEnable = true, // Configurar RTS/CTS
            ReadTimeout = 500,
            WriteTimeout = 500
        };

        try
        {
            serialPort.Open();

            if (serialPort.IsOpen)
            {
                Console.WriteLine("Puerto COM11 abierto correctamente.");

                // Inicializar la impresora
                byte[] initializePrinter = { 0x1B, 0x40 }; // ESC @
                serialPort.Write(initializePrinter, 0, initializePrinter.Length);

                // Establecer espaciado entre líneas
                byte[] setLineSpacing = { 0x1B, 0x33, 0x20 }; // ESC 3
                serialPort.Write(setLineSpacing, 0, setLineSpacing.Length);

                // Datos para imprimir
                string payee = "John Doe";
                string amount = "$1000.00";
                string date = "2024-07-15";
                string message = "Hola, ya pude imprimir con ESC POS\n";
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);

                // Comandos para imprimir el cheque
                byte[] printCommands = ByteSplicer.Combine(
                    Encoding.ASCII.GetBytes(payee + "\n"),
                    Encoding.ASCII.GetBytes(amount + "\n"),
                    Encoding.ASCII.GetBytes(date + "\n"),
                    messageBytes,   
                    new byte[] { 0x1B, 0x64, 0x05 } // ESC d (Imprimir y avanzar 5 líneas)
                );

                serialPort.Write(printCommands, 0, printCommands.Length);

                Console.WriteLine("Cheque impreso!");

                serialPort.Close();
            }
            else
            {
                Console.WriteLine("No se pudo abrir el puerto COM10.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }



        //// Configura la conexión TCP
        ////usb serial connection

        ////var printer = new SerialPrinter(portName: "USB003", 9600);

        //// Reemplaza con el Vendor ID y Product ID de tu impresora
        //UsbDeviceFinder myUsbFinder = new UsbDeviceFinder(0x1FC9, 0x2016);
        //UsbDevice myUsbDevice = UsbDevice.OpenUsbDevice(myUsbFinder);

        //if (myUsbDevice == null)
        //{
        //    Console.WriteLine("Device Not Found.");
        //    return;
        //}

        //// If this is a "whole" usb device (libusb-win32, linux libusb-1.0)
        //IUsbDevice wholeUsbDevice = myUsbDevice as IUsbDevice;
        //if (!ReferenceEquals(wholeUsbDevice, null))
        //{
        //    // Select config #1
        //    wholeUsbDevice.SetConfiguration(1);

        //    // Claim interface #0.
        //    wholeUsbDevice.ClaimInterface(0);
        //}

        //// Open write endpoint (typically endpoint 1)
        //UsbEndpointWriter printer = myUsbDevice.OpenEndpointWriter(WriteEndpointID.Ep01);


        //// Crea un emisor para comandos ESC/POS
        //var e = new EPSON();

        //// Ejemplo de datos del cheque
        //string payee = "John Doe";
        //string amount = "$1000.00";
        //string date = "2024-07-15";
        //string stringtobytes = "HOLA yo era un striung y ahora soy bytes\n";
        //byte[] bytes = Encoding.ASCII.GetBytes(stringtobytes);

        //// Crea los comandos para imprimir el cheque
        //byte[] commands = ByteSplicer.Combine(
        //    e.Initialize(),
        //    e.PrintLine(payee),
        //    bytes,
        //    e.RightAlign(),
        //    e.PrintLine(amount),
        //    e.PrintLine(date),
        //    e.PrintLine("HOLA              "),
        //    e.LeftAlign(),
        //    e.PrintLine("HOLA               line spacing to 4"),
        //    e.PrintLine("HOLA               line spacing to 4"),
        //    e.PrintLine("HOLA               line spacing to 4"),
        //    e.FeedLines(2)
        //);

        //printer.Write(commands);

        //Console.WriteLine("Cheque printed!");
    }
}