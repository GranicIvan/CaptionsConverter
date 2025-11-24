using CaptionsConverter.Logic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace CaptionsConverter.GUI.Views
{
    
    public partial class FolderBatchView : UserControl
    {

        private string? _selectedFolderPath;


        public FolderBatchView()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).NavigateTo(new MainMenuView());
        }

        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "Select folder with files to convert";
            dialog.UseDescriptionForTitle = true;
            dialog.ShowNewFolderButton = false;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _selectedFolderPath = dialog.SelectedPath;
                SelectedFolderText.Text = $"📂 {dialog.SelectedPath}";
                SelectedFolderDisplay.Visibility = Visibility.Visible;
            }
        }

        private void ConvertAllFiles_Click(object sender, RoutedEventArgs e)
        {

            string extension = CustomExtensionInput.Text.Trim();

            if (string.IsNullOrEmpty(_selectedFolderPath))
            {
                MessageBox.Show("Please select a folder first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            ConversionResult result = CCLogic.FileReading(_selectedFolderPath, extension);           

            MessageBoxImage icon = result.Status switch
            {
                ConversionStatus.Success => MessageBoxImage.Information,
                ConversionStatus.PartialSuccess => MessageBoxImage.Warning,
                ConversionStatus.SkippedAllFiles => MessageBoxImage.Warning,
                ConversionStatus.NoFilesFound => MessageBoxImage.Warning,
                ConversionStatus.Failed => MessageBoxImage.Error,
                _ => MessageBoxImage.None
            };

            MessageBox.Show(result.Message, result.Status.ToString(), MessageBoxButton.OK, icon);
        }


        private void DragOverFolder(object sender, DragEventArgs e)
        {
            // Check if user is dragging a folder
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (paths.Length == 1 && Directory.Exists(paths[0]))
                {
                    e.Effects = DragDropEffects.Copy;
                    DropZoneBorder.BorderBrush = Brushes.DodgerBlue;
                    DropZoneBorder.Background = new SolidColorBrush(Color.FromArgb(30, 30, 144, 255)); // Light blue tint
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

        private void DragLeaveFolder(object sender, DragEventArgs e)
        {

            resetBorderColor();            
        }

        private void DropFolder(object sender, DragEventArgs e)
        {
            resetBorderColor();

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (paths.Length == 1 && Directory.Exists(paths[0]))
                {
                    _selectedFolderPath = paths[0];
                    SelectedFolderText.Text = $"📂 {_selectedFolderPath}";
                    SelectedFolderDisplay.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Please drop only one valid folder.", "Invalid drop", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void resetBorderColor() 
        {
            DropZoneBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(208, 215, 222)); // #D0D7DE
            DropZoneBorder.Background = new SolidColorBrush(Color.FromRgb(246, 248, 250)); // #F6F8FA
        }

    }
}
