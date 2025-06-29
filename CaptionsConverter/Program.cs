using System.Text;
using CaptionsConverter.Logic;
using CaptionsConverter.CLILogic;

class Program
{
    public static void Main(String[] args)
    {
        Console.WriteLine("Program start!\n");

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        string fileExtensions = "*.str"; // Default file extension

        if(!CliHandler.ArgumentHandling(args, ref fileExtensions))
        {
            Console.WriteLine("Exiting program due to invalid arguments.");
            return;
        }

        try
        {
            CCLogic.FileReading(args[0], fileExtensions);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            CliHandler.printHelp();           
        }
        

        Console.WriteLine("\nKraj");
    }


}

