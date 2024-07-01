using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Win32;

namespace RegistryManipulator.RegistryManager
{
    internal static class Manager
    {
        public static bool AddAppToRegistry(string keyName, string filePath, string registryKey, string entryPosition, string? appIconPath)
        {
            try
            {
                RegistryKey? key = Registry.ClassesRoot.OpenSubKey(registryKey, true);
                if (key == null)
                {
                    MessageBox.Show("Registry key not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (KeyExists(registryKey, keyName))
                {
                    MessageBox.Show("Key already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                key.CreateSubKey(keyName);
                key.Close();

                registryKey += "\\" + keyName;
                key = Registry.ClassesRoot.OpenSubKey(registryKey, true);
                if (key == null)
                {
                    MessageBox.Show("Failed to create key", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (appIconPath != null)
                {
                    key.SetValue("Icon", appIconPath);
                }

                key.SetValue("Position", entryPosition);
                key.CreateSubKey("command");
                key.Close();

                registryKey += "\\command";
                key = Registry.ClassesRoot.OpenSubKey(registryKey, true);
                if (key == null)
                {
                    MessageBox.Show("Failed to create command key", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                key.SetValue("", filePath);
                key.Close();

                key = Registry.ClassesRoot.OpenSubKey("RegistryManipulator\\StoredKeys", true);
                if (key == null)
                {
                    MessageBox.Show("Failed to open StoredKeys key", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                key.SetValue(keyName, registryKey);
                key.Close();

                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }

        public static void CreateStoredKeysRegistryIfNotExists()
        {
            try
            {
                RegistryKey? key = Registry.ClassesRoot.OpenSubKey("RegistryManipulator", true);
                if (key == null)
                {
                    key = Registry.ClassesRoot.CreateSubKey("RegistryManipulator");
                    key.CreateSubKey("StoredKeys");
                    key.Close();
                }
                else
                {
                    if (!key.GetSubKeyNames().Contains("StoredKeys"))
                    {
                        key.CreateSubKey("StoredKeys");
                        MessageBox.Show("Created StoredKeys registry", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    
                    key.Close();
                }

                
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static bool KeyExists(string registryPath, string key)
        {
            try
            {
                RegistryKey? regKey = Registry.ClassesRoot.OpenSubKey(registryPath);
                if (regKey == null)
                    return false;

                if (regKey.GetSubKeyNames().Contains(key))
                {
                    return true;
                }

                regKey.Close();
                return false;

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
