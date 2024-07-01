using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Microsoft.Win32;

using RegistryManipulator.RegistryManager;

namespace RegistryManipulator.AppUI
{
    /// <summary>
    /// Interaction logic for FileRightClickContext.xaml
    /// </summary>
    public partial class FileRightClickContext : Window
    {
        public FileRightClickContext()
        {
            InitializeComponent();
        }

        private void textBlockAppPath_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string? path = GetPath("exe", "bat");
            if (path == null)
                return;

            textBlockAppPath.Text = path;
        }

        private void textBoxIconPath_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string? path = GetPath("ico");
            if (path == null)
                return;

            textBoxIconPath.Text = path;
        }

        private void comboBoxIconType_DropDownClosed(object sender, EventArgs e)
        {
            textBoxIconPath.Visibility = comboBoxIconType.SelectionBoxItem.ToString() == "Custom Icon" ? Visibility.Visible : Visibility.Hidden;
            textBoxIconPath.Text = "";
        }

        private async void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if(textBoxMenuItemName.Text == "")
            {
                MessageBox.Show("Please enter a menu item name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(textBlockAppPath.Text == "")
            {
                MessageBox.Show("Please select an application path", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(comboBoxIconPlacement.SelectionBoxItem.ToString() == "")
            {
                MessageBox.Show("Please select an icon placement", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(comboBoxIconType.SelectionBoxItem.ToString() == "Custom Icon" && textBoxIconPath.Text == "")
            {
                MessageBox.Show("Please select an icon path", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string registryKey = "*\\shell";
            string? appIconPath = null;
            if(comboBoxIconType.SelectionBoxItem.ToString() == "Custom Icon")
                appIconPath = textBoxIconPath.Text;
            if(comboBoxIconType.SelectionBoxItem.ToString() == "Application's own Icon")
                appIconPath = textBlockAppPath.Text;
            if(comboBoxIconType.SelectionBoxItem.ToString() == "No Icon")
                appIconPath = null;

            bool result = Manager.AddAppToRegistry(textBoxMenuItemName.Text, textBlockAppPath.Text, registryKey, comboBoxIconPlacement.SelectionBoxItem.ToString(), appIconPath);

            if(result)
                MessageBox.Show("Successfully saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Failed to save", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private string? GetPath(params string[] fileFormats)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = string.Join("|", fileFormats.Select(f => $"{f} files (*.{f})|*.{f}"));


            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

    }
}
