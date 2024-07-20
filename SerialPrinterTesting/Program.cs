using System.Text;
using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;

class Program
{
    static void Main()
    {
        // Configura la conexión TCP
        //usb serial connection

        var printer = new SerialPrinter("COM3", 9600);
       
        // Crea un emisor para comandos ESC/POS
        var e = new EPSON();

        // Ejemplo de datos del cheque
        string payee = "John Doe";
        string amount = "$1000.00";
        string date = "2024-07-15";
        string stringtobytes = "HOLA yo era un striung y ahora soy bytes\n";
        byte[] bytes = Encoding.ASCII.GetBytes(stringtobytes);

        // Crea los comandos para imprimir el cheque
        byte[] commands = ByteSplicer.Combine(
            e.Initialize(),
            e.PrintLine(payee),
            bytes,
            e.RightAlign(),
            e.PrintLine(amount),
            e.PrintLine(date),
            e.PrintLine("HOLA              "),
            e.LeftAlign(),
            e.PrintLine("HOLA               line spacing to 4"),
            e.PrintLine("HOLA               line spacing to 4"),
            e.PrintLine("HOLA               line spacing to 4"),
            e.FeedLines(2)
        );

        printer.Write(commands);

        Console.WriteLine("Cheque printed!");
    }
}