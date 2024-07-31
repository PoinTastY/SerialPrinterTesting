using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using System.IO.Ports;
using System.Text;

class Program
{
    //[DllImport("kernel32.dll", SetLastError = true)]
    //static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

    static void Main()
    {
        // Configura el puerto serial
        SerialPort serialPort = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
        serialPort.Handshake = Handshake.RequestToSend; // RTS/CTS


        serialPort.Open();

        // Configurar el área de impresión (7 cm x 16.6 cm)
        //byte[] setPrintArea = new byte[]
        //{
        //0x1B, 0x57, // ESC W: Establecer el área de impresión
        //0x00, 0x00, // xL (low byte for x, starting position)
        //0x00, 0x00, // xH (high byte for x, starting position)
        //0x00, 0x00, // yL (low byte for y, starting position)
        //0x00, 0x00, // yH (high byte for y, starting position)
        //(byte)(198 % 256), (byte)(198 / 256), // dxL, dxH (width in points, 7 cm)
        //(byte)(471 % 256), (byte)(471 / 256)  // dyL, dyH (height in points, 16.6 cm)
        //}; 
        // Definir área de impresión


        byte[] selectPageMode = new byte[] { 0x1B, 0x4C }; // ESC L: Seleccionar modo páginaf
        byte[] restorePrintDirection = new byte[] { 0x1B, 0x54, 0x00 }; // ESC T 0: Restaurar la dirección de impresión a 0 grados
        byte[] setPrintDirection = new byte[] { 0x1B, 0x54, 0x03 }; 
        byte[] leftAlign = new byte[] { 0x1B, 0x61, 0x00 }; // ESC a 0: Alinear a la izquierda
        byte[] formFeed = new byte[] { 0x0C }; // FF: Imprimir y volver al modo estándar
        byte[] setReverseEject = new byte[] { 0x1B, 0x46, 0x01 }; // ESC F n
        byte[] reverseFeed = new byte[] { 0x1B, 0x45, 0x02 }; // ESC e n
        byte[] escC4 = new byte[] { 0x1B, 0x63, 0x04 }; // ESC c 
        byte[] activateReverseFeed = new byte[] { 0x1B, 0x46, 0x01 }; // Activa el retorno del papel después de impresión
        byte[] selectStopPrintingSensor = new byte[] { 0x1B, 0x63, 0x04 }; // Selecciona los sensores para detener la impresión
        byte[] releasePaper = new byte[] { 0x1B, 0x71 };


        // Define ESC c 3 command to select paper sensor(s) to output paper end signals
        byte[] selectPaperEndSignal = new byte[]
        {
    0x1B, 0x63, // ESC c
    0x03        // Select paper sensor(s) to output paper end signals
        };

        byte[] selectStopPrinting = new byte[]
{
    0x1B, 0x63, // ESC c
    0x04        // Select paper sensor(s) to stop printing
};

        // Ejemplo de datos del cheque
        string payee = "Cooperativa Pedro Ezqueda";
        string amount = "                                       $100000.00";
        string amountInWords = "Cien mil pesos 00/100 M.N.";
        string date = "                       2024-07-15";

        var e = new EPSON();

        // Crea los comandos para imprimir el cheque
        byte[] commands = ByteSplicer.Combine(
            e.Initialize(),
            selectPageMode,
            setPrintDirection,
            e.FeedLines(3),
            e.PrintLine(date),
            e.FeedLines(1),
            e.PrintLine(payee),
            e.FeedLines(1),
            e.PrintLine(amount),
            e.FeedLines(1),
            e.PrintLine(amountInWords),
            releasePaper,
            formFeed

        );

        serialPort.Write(commands, 0, commands.Length);




        Console.WriteLine("Cheque printed!");
    }
}