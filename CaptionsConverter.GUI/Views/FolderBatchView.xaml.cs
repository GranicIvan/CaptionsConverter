using CaptionsConverter.Logic;
using System.Windows;
using System.Windows.Controls;


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
                SelectedFolderText.Text = $"Selected folder:\n{dialog.SelectedPath}";
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


            CCLogic.FileReading(_selectedFolderPath, extension);

        }
    }
}
