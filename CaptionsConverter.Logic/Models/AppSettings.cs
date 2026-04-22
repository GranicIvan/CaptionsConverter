namespace CaptionsConverter.Logic.Models
{
    public class AppSettings
    {
        public string DefaultFileExtension { get; set; } = ".str";
        public string DefaultFallbackEncoding { get; set; } = "windows-1252";
        public List<LanguageMapping> LanguageMappings { get; set; } = new();
    }

    public class LanguageMapping
    {
        public string LanguageName { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public Dictionary<string, string> CharacterReplacements { get; set; } = new();
    }
}
