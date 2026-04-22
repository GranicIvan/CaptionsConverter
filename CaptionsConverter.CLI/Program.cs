using System.Text;
using CaptionsConverter.Logic;
using CaptionsConverter.CLILogic;

class Program
{
    public static void Main(String[] args)
    {
        Console.WriteLine("Program start!\n");        

        string fileExtensions = "";

        if (args.Contains("--list-languages"))
        {
            ListLanguages();
            return;
        }

        if (args.Contains("--enable-all"))
        {
            ConfigurationManager.EnableAllLanguages();
            ConfigurationManager.SaveConfiguration();
            Console.WriteLine("All languages enabled.");
            args = args.Where(a => a != "--enable-all").ToArray();
        }

        if (args.Contains("--disable-all"))
        {
            ConfigurationManager.DisableAllLanguages();
            ConfigurationManager.SaveConfiguration();
            Console.WriteLine("All languages disabled.");
            args = args.Where(a => a != "--disable-all").ToArray();
        }

        int enableLangIndex = Array.IndexOf(args, "--enable-language");
        if (enableLangIndex >= 0 && enableLangIndex + 1 < args.Length)
        {
            string languageName = args[enableLangIndex + 1];
            ConfigurationManager.EnableOnlyLanguage(languageName);
            ConfigurationManager.SaveConfiguration();
            Console.WriteLine($"Enabled language: {languageName}");
            args = args.Where((a, i) => i != enableLangIndex && i != enableLangIndex + 1).ToArray();
        }

        if (!CliHandler.ArgumentHandling(args, ref fileExtensions))
        {
            Console.WriteLine("Exiting program due to invalid arguments.");
            return;
        }

        Console.WriteLine($"Active languages: {string.Join(", ", ConfigurationManager.GetEnabledLanguages())}");
        Console.WriteLine();

        try
        {
            ConversionResult result = CCLogic.FileReading(args[0], fileExtensions);
            Console.WriteLine($"\n{result.Status}: {result.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            CliHandler.printHelp();           
        }
        
        Console.WriteLine("\nKraj");
    }

    private static void ListLanguages()
    {
        Console.WriteLine("Available Language Mappings:");
        Console.WriteLine("-----------------------------");
        
        var languages = ConfigurationManager.Settings.LanguageMappings;
        foreach (var lang in languages)
        {
            string status = lang.IsEnabled ? "[ENABLED]" : "[DISABLED]";
            Console.WriteLine($"{status} {lang.LanguageName}");
            Console.WriteLine($"  Character replacements: {lang.CharacterReplacements.Count}");
            foreach (var replacement in lang.CharacterReplacements.Take(3))
            {
                Console.WriteLine($"    '{replacement.Key}' → '{replacement.Value}'");
            }
            if (lang.CharacterReplacements.Count > 3)
            {
                Console.WriteLine($"    ... and {lang.CharacterReplacements.Count - 3} more");
            }
            Console.WriteLine();
        }
    }
}

