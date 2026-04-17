using CaptionsConverter.Logic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CaptionsConverter.GUI.Views
{
    public partial class SingleFileView : UserControl
    {
        private string? _selectedFilePath;

        public SingleFileView()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).NavigateTo(new MainMenuView());
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Select a caption file to convert",
                Filter = "Subtitle files (*.srt;*.sub;*.str;*.txt)|*.srt;*.sub;*.str;*.txt|All files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                _selectedFilePath = dialog.FileName;
                SelectedFileText.Text = $"📄 {dialog.FileName}";
                SelectedFileDisplay.Visibility = Visibility.Visible;
            }
        }

        private void ConvertFile_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedFilePath))
            {
                MessageBox.Show("Please select a file first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ConversionResult result = CCLogic.SingleFileConversion(_selectedFilePath);

            MessageBoxImage icon = result.Status switch
            {
                ConversionStatus.Success => MessageBoxImage.Information,
                ConversionStatus.SkippedAllFiles => MessageBoxImage.Warning,
                ConversionStatus.NoFilesFound => MessageBoxImage.Warning,
                ConversionStatus.Failed => MessageBoxImage.Error,
                _ => MessageBoxImage.None
            };

            MessageBox.Show(result.Message, result.Status.ToString(), MessageBoxButton.OK, icon);
        }

        private void DragOverFile(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (paths.Length == 1 && File.Exists(paths[0]))
                {
                    e.Effects = DragDropEffects.Copy;
                    DropZoneBorder.BorderBrush = Brushes.DodgerBlue;
                    DropZoneBorder.Background = new SolidColorBrush(Color.FromArgb(30, 30, 144, 255));
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }

            e.Handled = true;
        }

        private void DragLeaveFile(object sender, DragEventArgs e)
        {
            ResetBorderColor();
        }

        private void DropFile(object sender, DragEventArgs e)
        {
            ResetBorderColor();

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (paths.Length == 1 && File.Exists(paths[0]))
                {
                    _selectedFilePath = paths[0];
                    SelectedFileText.Text = $"📄 {_selectedFilePath}";
                    SelectedFileDisplay.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Please drop only one valid file.", "Invalid drop", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void ResetBorderColor()
        {
            DropZoneBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(208, 215, 222));
            DropZoneBorder.Background = new SolidColorBrush(Color.FromRgb(246, 248, 250));
        }
    }
}
