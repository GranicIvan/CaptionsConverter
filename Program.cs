using System.IO;
class Program
{
    public static void Main(String[] args)
    {
        Console.WriteLine("Program start!\n");

        Console.WriteLine(args[0]);

        try {
            foreach (string file in Directory.EnumerateFiles(args[0], "*.srt"))
            {
                Console.WriteLine(file);
                string contents = File.ReadAllText(file);
                string result = changeCharacters(contents);
                File.WriteAllText(file, result);
            }
        } catch (Exception ex)
        {
            Console.WriteLine(ex.ToString()); 
        }
        
        Console.WriteLine("\nKraj");
    }

    static string  changeCharacters(string contents) {

        var replacements = new Dictionary<char, char>
        {
            ['è'] = 'č',
            ['ð'] = 'đ',
            ['æ'] = 'ć'
        };

        string result = replacements.Aggregate(contents, (current, pair) => current.Replace(pair.Key, pair.Value));

        return result;

       
    }
}

