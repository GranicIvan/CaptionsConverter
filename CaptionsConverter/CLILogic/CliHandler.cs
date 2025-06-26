

namespace CaptionsConverter.CLILogic
{
    public class CliHandler
    {

        public static void printHelp()
        {
            Console.WriteLine("Usage: ./CaptionsConverter.exe <FolderPath> [file extension]");
            Console.WriteLine("Files will be overwritten");
            Console.WriteLine("Path must use / or \\\\ or \"\\\" ");
            Console.WriteLine("Examples: C:/user/file.zip  or  C:\\\\user\\\\file.zip or \"C:\\user\\file.zip\" \n");
            Console.WriteLine("Default for file extension is .srt\n");
        }

    }
}
