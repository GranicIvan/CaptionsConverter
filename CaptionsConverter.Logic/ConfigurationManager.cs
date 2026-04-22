using System.Text.Json;
using CaptionsConverter.Logic.Models;
using System.Diagnostics;

namespace CaptionsConverter.Logic
{
    public static class ConfigurationManager
    {
        private static AppSettings? _settings;
        private static readonly string _defaultConfigPath = "appsettings.json";

        public static AppSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    LoadConfiguration();
                }
                return _settings!;
            }
        }

        public static void LoadConfiguration(string? configPath = null)
        {
            string path = configPath ?? _defaultConfigPath;

            try
            {
                if (!File.Exists(path))
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _defaultConfigPath);
                }

                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    _settings = JsonSerializer.Deserialize<AppSettings>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        ReadCommentHandling = JsonCommentHandling.Skip
                    });
                }
                else
                {
                    _settings = GetDefaultSettings();
                    SaveConfiguration(path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configuration: {ex.Message}. Using default settings.");
                _settings = GetDefaultSettings();
            }

            _settings ??= GetDefaultSettings();
        }

        public static void SaveConfiguration(string? configPath = null)
        {
            string path = configPath ?? _defaultConfigPath;

            try
            {
                if (!File.Exists(path))
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _defaultConfigPath);
                }

                string json = JsonSerializer.Serialize(_settings ?? GetDefaultSettings(), new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving configuration: {ex.Message}");
            }
        }

        public static void ReloadConfiguration(string? configPath = null)
        {
            _settings = null;
            LoadConfiguration(configPath);
        }

        private static AppSettings GetDefaultSettings()
        {
            return new AppSettings
            {
                DefaultFileExtension = ".str",
                DefaultFallbackEncoding = "windows-1252",
                AutoOpenOutputFolder = false,
                ConfirmBeforeConversion = true,
                LanguageMappings = new List<LanguageMapping>
                {
                    new LanguageMapping
                    {
                        LanguageName = "Serbian (Latin)/Croatian",
                        IsEnabled = true,
                        CharacterReplacements = new Dictionary<string, string>
                        {
                            ["?"] = "?",
                            ["?"] = "?",
                            ["?"] = "?",
                            ["?"] = "?",
                            ["?"] = "?",
                            ["?"] = "?"
                        }
                    }
                }
            };
        }

        public static Dictionary<char, char> GetActiveCharacterReplacements()
        {
            var replacements = new Dictionary<char, char>();

            foreach (var mapping in Settings.LanguageMappings.Where(m => m.IsEnabled))
            {
                foreach (var replacement in mapping.CharacterReplacements)
                {
                    if (replacement.Key.Length == 1 && replacement.Value.Length == 1)
                    {
                        replacements[replacement.Key[0]] = replacement.Value[0];
                    }
                }
            }

            return replacements;
        }

        public static List<string> GetAvailableLanguages()
        {
            return Settings.LanguageMappings.Select(m => m.LanguageName).ToList();
        }

        public static List<string> GetEnabledLanguages()
        {
            return Settings.LanguageMappings.Where(m => m.IsEnabled).Select(m => m.LanguageName).ToList();
        }

        public static void SetLanguageEnabled(string languageName, bool isEnabled)
        {
            var language = Settings.LanguageMappings.FirstOrDefault(m => m.LanguageName == languageName);
            if (language != null)
            {
                language.IsEnabled = isEnabled;
            }
        }

        public static void EnableOnlyLanguage(string languageName)
        {
            foreach (var mapping in Settings.LanguageMappings)
            {
                mapping.IsEnabled = mapping.LanguageName == languageName;
            }
        }

        public static void DisableAllLanguages()
        {
            foreach (var mapping in Settings.LanguageMappings)
            {
                mapping.IsEnabled = false;
            }
        }

        public static void EnableAllLanguages()
        {
            foreach (var mapping in Settings.LanguageMappings)
            {
                mapping.IsEnabled = true;
            }
        }

        public static bool ShouldAutoOpenFolder()
        {
            return Settings.AutoOpenOutputFolder;
        }

        public static bool ShouldConfirmBeforeConversion()
        {
            return Settings.ConfirmBeforeConversion;
        }

        public static void SetAutoOpenFolder(bool value)
        {
            Settings.AutoOpenOutputFolder = value;
        }

        public static void SetConfirmBeforeConversion(bool value)
        {
            Settings.ConfirmBeforeConversion = value;
        }

        public static void OpenFolderInExplorer(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = folderPath,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error opening folder: {ex.Message}");
                }
            }
        }
    }
}
