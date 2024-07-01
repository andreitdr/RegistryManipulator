using System.Text;
using System.Windows;

using RegistryManipulator.Contexts;
using RegistryManipulator.RegistryManager;

namespace RegistryManipulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowContext();
            Manager.CreateStoredKeysRegistryIfNotExists();
        }

        private void buttonFilesRightClickMenu_Click(object sender, RoutedEventArgs e)
        {
            new AppUI.FileRightClickContext().Show();
        }

        private void buttonFolderRightClickMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonDesktopRightClickMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonWindows11Fix_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}