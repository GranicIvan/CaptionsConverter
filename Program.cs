using System.IO;
class Program
{
    public static void Main(String[] args)
    {
        Console.WriteLine("Hello, World!\n");

        try {
            foreach (string file in Directory.EnumerateFiles(args[0], "*.xml"))
            {
                string contents = File.ReadAllText(file);
                changeCharacters(contents);
            }
        } catch (Exception ex)
        {
            Console.WriteLine(ex.ToString()); 
        }
        
        Console.WriteLine("\nKraj");
    }

    static void changeCharacters(string contents) {
        contents.Replace("\r\n", "\n");
    }
}

