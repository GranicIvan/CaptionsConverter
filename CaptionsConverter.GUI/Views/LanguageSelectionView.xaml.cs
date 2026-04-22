using CaptionsConverter.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CaptionsConverter.GUI.Views
{
    public partial class LanguageSelectionView : UserControl
    {
        public LanguageSelectionView()
        {
            InitializeComponent();
            LoadLanguages();
        }

        private void LoadLanguages()
        {
            LanguageListPanel.Children.Clear();

            var languages = ConfigurationManager.Settings.LanguageMappings;

            foreach (var language in languages)
            {
                var tooltipText = BuildCharacterMappingTooltip(language.CharacterReplacements);
                
                var checkBox = new CheckBox
                {
                    Content = language.LanguageName,
                    IsChecked = language.IsEnabled,
                    FontSize = 14,
                    Margin = new Thickness(0, 8, 0, 8),
                    Tag = language.LanguageName,
                    ToolTip = tooltipText
                };

                checkBox.Checked += LanguageCheckBox_Changed;
                checkBox.Unchecked += LanguageCheckBox_Changed;

                var detailsText = new TextBlock
                {
                    Text = $"   {language.CharacterReplacements.Count} character replacements defined",
                    FontSize = 12,
                    Foreground = System.Windows.Media.Brushes.Gray,
                    Margin = new Thickness(25, 0, 0, 8)
                };

                LanguageListPanel.Children.Add(checkBox);
                LanguageListPanel.Children.Add(detailsText);
            }

            UpdateActiveLanguageDisplay();
        }

        private string BuildCharacterMappingTooltip(Dictionary<string, string> characterReplacements)
        {
            if (characterReplacements == null || characterReplacements.Count == 0)
            {
                return "No character replacements defined";
            }

            var tooltip = new StringBuilder();
            tooltip.AppendLine("Character Replacements:");
            tooltip.AppendLine();

            foreach (var replacement in characterReplacements.OrderBy(r => r.Key))
            {
                tooltip.AppendLine($"  {replacement.Key}  ->  {replacement.Value}");
            }

            return tooltip.ToString().TrimEnd();
        }

        private void LanguageCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.Tag is string languageName)
            {
                ConfigurationManager.SetLanguageEnabled(languageName, checkBox.IsChecked == true);
                UpdateActiveLanguageDisplay();
            }
        }

        private void UpdateActiveLanguageDisplay()
        {
            var enabledLanguages = ConfigurationManager.GetEnabledLanguages();
            
            if (enabledLanguages.Any())
            {
                ActiveLanguagesText.Text = $"Active: {string.Join(", ", enabledLanguages)}";
                ActiveLanguagesText.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                ActiveLanguagesText.Text = "No languages enabled - conversions will not make any changes!";
                ActiveLanguagesText.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void EnableAll_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationManager.EnableAllLanguages();
            LoadLanguages();
        }

        private void DisableAll_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationManager.DisableAllLanguages();
            LoadLanguages();
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfigurationManager.SaveConfiguration();
                MessageBox.Show("Language settings saved successfully!", "Success", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).NavigateTo(new MainMenuView());
        }
    }
}
