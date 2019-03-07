using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DesktopBridgeTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //openFileDialog = new OpenFileDialog()
            //{
            //    FileName = "Select a text file",
            //    Filter = "Text files (*.txt)|*.txt",
            //    Title = "Open text file"
            //};
        }
        private OpenFileDialog openFileDialog;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name.Equals("save"))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                    File.WriteAllText(saveFileDialog.FileName, "This is Josh");

            }
            else
            {
                openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        Path.Text = openFileDialog.FileName;

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
    }
}
