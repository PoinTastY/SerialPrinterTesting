using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using System.IO.Ports;

using ESCPOS_NEusing ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using System.IO.Ports;
using System.Text;

class Program
{
    //[DllImport("kernel32.dll", SetLastError = true)]
    //static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

    static void Main()
    {
        // Setup serial port
        SerialPort serialPort = new SerialPort("COM13", 9600, Parity.None, 8, StopBits.One);
        serialPort.Handshake = Handshake.RequestToSend; // RTS/CTS


        serialPort.Open();


        byte[] selectPageMode = [0x1B, 0x4C];
        byte[] setPrintDirection = [0x1B, 0x54, 0x03];
        byte[] formFeed = [0x0C];
        byte[] releasePaper = [0x1B, 0x71];

        // Example data
        string payee = "Cooperativa Pedro Ezqueda";
        string amount = "                                       $100000.00";
        string amountInWords = "Cien mil pesos 00/100 M.N.";
        string date = "                       2024-07-15";

        var e = new EPSON();


        //byte[] commands = ByteSplicer.Combine(
        //    e.Initialize(),
        //    selectPageMode,
        //    setPrintDirection,
        //    e.FeedLines(3),
        //    e.PrintLine(date),
        //    lineFeed,                                                                         SUS METHOD CFAIOSDJFLAKSDFKLJAS
        //    e.FeedLines(1),
        //    e.PrintLine(payee),
        //    e.FeedLines(1),
        //    e.PrintLine(amount),
        //    e.FeedLines(1),
        //    e.PrintLine(amountInWords),
        //    releasePaper,
        //    formFeed

        //);

        serialPort.Write(e.Initialize(), 0, e.Initialize().Length);
        Thread.Sleep(100);

        serialPort.Write(selectPageMode, 0, selectPageMode.Length);
        Thread.Sleep(100);

        serialPort.Write(setPrintDirection, 0, setPrintDirection.Length);
        Thread.Sleep(100);

        serialPort.Write(e.FeedLines(3), 0, e.FeedLines(3).Length);
        Thread.Sleep(100);

        serialPort.Write(e.PrintLine(date), 0, e.PrintLine(date).Length);
        Thread.Sleep(100);

        serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
        Thread.Sleep(100);

        serialPort.Write(e.PrintLine(payee), 0, e.PrintLine(payee).Length);
        Thread.Sleep(100);

        serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
        Thread.Sleep(100);

        serialPort.Write(e.PrintLine(amount), 0, e.PrintLine(amount).Length);
        Thread.Sleep(100);

        serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
        Thread.Sleep(100);

        serialPort.Write(e.PrintLine(amountInWords), 0, e.PrintLine(amountInWords).Length);
        Thread.Sleep(100);

        serialPort.Write(e.FeedLines(1), 0, e.FeedLines(1).Length);
        Thread.Sleep(100);

        serialPort.Write(releasePaper, 0, releasePaper.Length);
        Thread.Sleep(100);

        serialPort.Write(formFeed, 0, formFeed.Length);
        Thread.Sleep(100);

        serialPort.Close();
        Console.WriteLine("Puerto serial cerrado.");

        serialPort.Close();


        Console.WriteLine("Cheque printed!");
    }
}