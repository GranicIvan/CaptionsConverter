using System.Text;
using CaptionsConverter.Logic;
using CaptionsConverter.CLILogic;

class Program
{
    public static void Main(String[] args)
    {
        Console.WriteLine("Program start!\n");

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        string fileExtensions = "*.str";

        if (args.Length < 1)
        {
            Console.WriteLine("Program needs argument, path to folder that contains files to change");
            CliHandler.printHelp();
            return;
        }
        else if (args[0].ToLower() == "help" || args[0].ToLower() == "-h" || args[0].ToLower() == "--help" || args[0].ToLower() == "-help")
        {
            CliHandler.printHelp();
            return;
        }
        else if (args.Length > 1)
        {
            fileExtensions = args[1];
        }

        CCLogic.FileReading(args[0], fileExtensions);

        Console.WriteLine("\nKraj");
    }


}

