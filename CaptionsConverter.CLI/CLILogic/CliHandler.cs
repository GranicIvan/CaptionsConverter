namespace CaptionsConverter.CLILogic
{
    public class CliHandler
    {

        public static void printHelp()
        {
            Console.WriteLine("------- HELP ------- ");
            Console.WriteLine("Usage: ./CaptionsConverter.exe <FolderPath> [file extension] [options]");
            Console.WriteLine("Files will be overwritten");
            Console.WriteLine("Path must use / or \\\\ or \"\\\" ");
            Console.WriteLine("Examples: C:/user/file.zip  or  C:\\\\user\\\\file.zip or \"C:\\user\\file.zip\" \n");
            Console.WriteLine("Default file extension is .str\n");
            Console.WriteLine("Options:");
            Console.WriteLine("  --list-languages         List all available language mappings");
            Console.WriteLine("  --enable-language <name> Enable specific language mapping");
            Console.WriteLine("  --disable-all           Disable all language mappings");
            Console.WriteLine("  --enable-all            Enable all language mappings");
            Console.WriteLine("\nExamples:");
            Console.WriteLine("  CaptionsConverter.exe C:/captions .srt");
            Console.WriteLine("  CaptionsConverter.exe C:/captions .srt --enable-language \"Serbian (Latin)/Croatian\"");
        }


        // Returns true if arguments are valid, false otherwise
        public static bool ArgumentHandling(string[] args, ref string fileExtensions)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Program needs argument(s), path to folder that contains files to change");
                printHelp();                
                return false;
            }
            else if (args[0].ToLower() == "help" ||
                args[0].ToLower() == "-h" ||
                args[0].ToLower() == "--help" ||
                args[0].ToLower() == "-help")
            {
                printHelp();
                return false;
            }
            else if (args.Length > 1)
            {
                fileExtensions = args[1];
            }
            return true;
        }

    }
}
