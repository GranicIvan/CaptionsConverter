using System.Text;
using Ude;

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
            printHelp();
            return;
        }
        else if (args[0].ToLower() == "help" || args[0].ToLower() == "-h" || args[0].ToLower() == "--help" || args[0].ToLower() == "-help")
        {
            printHelp();
            return;
        }
        else if (args.Length > 1)
        {
            fileExtensions = args[1];
        }

        FileReading(args[0], fileExtensions);

        Console.WriteLine("\nKraj");
    }

    static string changeCharacters(string contents)
    {

        var replacements = new Dictionary<char, char>
        {
            ['è'] = 'č',
            ['ð'] = 'đ',
            ['æ'] = 'ć',
            ['È'] = 'Č',
            ['Ð'] = 'Đ',
            ['Æ'] = 'Ć'
        };

        string result = replacements.Aggregate(contents, (current, pair) => current.Replace(pair.Key, pair.Value));

        return result;
    }

    static void FileReading(string folderPath, string fileExtension)
    {
        try
        {           

            foreach (string file in Directory.EnumerateFiles(folderPath, "*.srt"))
            {
                //Console.WriteLine(file);                
                Encoding sourceEncoding = DetectEncoding(file);
                string contents = File.ReadAllText(file, sourceEncoding);
                string result = changeCharacters(contents);               
                File.WriteAllText(file, result, Encoding.UTF8);

            }
        }
        catch (Exception ex)
        {
            printHelp();
            Console.WriteLine(ex.ToString());
        }
    }

    static void printHelp()
    {
        Console.WriteLine("Usage: ./CaptionsConverter.exe <FolderPath> [file extension]");
        Console.WriteLine("Files will be overwritten");
        Console.WriteLine("Path must use / or \\\\ or \"\\\" ");
        Console.WriteLine("Examples: C:/user/file.zip  or  C:\\\\user\\\\file.zip or \"C:\\user\\file.zip\" \n");
        Console.WriteLine("Default for file extension is .srt\n");
    }


    static Encoding DetectEncoding(string filePath)
    {
        using var fileStream = File.OpenRead(filePath);
        var detector = new CharsetDetector();
        detector.Feed(fileStream);
        detector.DataEnd();

        if (detector.Charset != null)
        {
            Console.WriteLine($"Detected encoding: {detector.Charset} for file: {Path.GetFileName(filePath)}");

            // Try to return the corresponding Encoding object
            try
            {
                return Encoding.GetEncoding(detector.Charset);
            }
            catch
            {
                Console.WriteLine("Unsupported encoding detected, falling back to Windows-1252.");
            }
        }

        // Fallback
        return Encoding.GetEncoding("windows-1252");
    }

}

