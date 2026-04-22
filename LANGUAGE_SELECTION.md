# Language Selection Feature

The CaptionsConverter now supports configurable language character mappings across all three interfaces: Logic, GUI, and CLI.

## Configuration File

Language mappings are stored in `appsettings.json` located in the application directory:

```json
{
  "DefaultFileExtension": ".str",
  "DefaultFallbackEncoding": "windows-1252",
  "LanguageMappings": [
    {
      "LanguageName": "Serbian (Latin)/Croatian",
      "IsEnabled": true,
      "CharacterReplacements": {
        "è": "?",
        "ð": "?",
        "æ": "?",
        "È": "?",
        "Ð": "?",
        "Æ": "?"
      }
    }
  ]
}
```

## GUI Usage

1. Launch the GUI application
2. Click **"?? Language Settings"** from the main menu
3. Check/uncheck languages to enable/disable them
4. Click **"?? Save Settings"** to persist your changes
5. Return to the main menu and perform conversions as usual

**Features:**
- Enable/disable individual language mappings with checkboxes
- View active languages in real-time
- **Enable All** / **Disable All** quick actions
- Save settings to persist across sessions

## CLI Usage

### List Available Languages
```bash
CaptionsConverter.exe --list-languages
```

### Enable a Specific Language
```bash
CaptionsConverter.exe "C:/captions" .srt --enable-language "Serbian (Latin)/Croatian"
```

### Enable All Languages
```bash
CaptionsConverter.exe "C:/captions" .srt --enable-all
```

### Disable All Languages
```bash
CaptionsConverter.exe "C:/captions" .srt --disable-all
```

### Standard Conversion (uses currently enabled languages)
```bash
CaptionsConverter.exe "C:/captions" .srt
```

## Logic Layer (Programmatic Access)

```csharp
using CaptionsConverter.Logic;

// Get all available languages
var languages = ConfigurationManager.GetAvailableLanguages();

// Get currently enabled languages
var enabledLanguages = ConfigurationManager.GetEnabledLanguages();

// Enable a specific language
ConfigurationManager.SetLanguageEnabled("Serbian (Latin)/Croatian", true);

// Enable only one language (disables all others)
ConfigurationManager.EnableOnlyLanguage("Serbian (Latin)/Croatian");

// Enable/disable all languages
ConfigurationManager.EnableAllLanguages();
ConfigurationManager.DisableAllLanguages();

// Save configuration changes
ConfigurationManager.SaveConfiguration();

// Reload configuration from file
ConfigurationManager.ReloadConfiguration();

// Get active character replacements for conversion
var replacements = ConfigurationManager.GetActiveCharacterReplacements();
```

## Adding New Languages

Edit `appsettings.json` and add a new language mapping:

```json
{
  "LanguageName": "Your Language Name",
  "IsEnabled": false,
  "CharacterReplacements": {
    "sourceChar1": "targetChar1",
    "sourceChar2": "targetChar2"
  }
}
```

**Important Notes:**
- Multiple enabled languages will **combine** their character replacements
- If two languages map the same source character to different targets, the **last one wins**
- Only single-character replacements are supported (1 character ? 1 character)
- Changes are applied to the in-memory settings immediately
- Use **Save Settings** to persist changes to disk

## Default Behavior

- Default file extension: `.str`
- Default encoding fallback: `windows-1252`
- Default language: `Serbian (Latin)/Croatian` (enabled by default)

## Configuration File Location

The application looks for `appsettings.json` in:
1. Current working directory
2. Application base directory (where the .exe is located)

If not found, default settings are used and a new file is created.
