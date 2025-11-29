using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CaptionsConverter.GUI.Views
{
    /// <summary>
    /// Interaction logic for MainMenuView.xaml
    /// </summary>
    public partial class MainMenuView : UserControl
    {
        public string AppVersion { get; }

        public MainMenuView()
        {
            InitializeComponent();
            
            // Get version from assembly
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            AppVersion = $"Version {version?.Major}.{version?.Minor}.{version?.Build}";
            
            DataContext = this;
        }

        private void Batch_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).NavigateTo(new FolderBatchView());            
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
