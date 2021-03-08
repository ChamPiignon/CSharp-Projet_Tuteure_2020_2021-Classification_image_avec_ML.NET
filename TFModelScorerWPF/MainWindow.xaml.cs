using ImageClassification.Score;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFCustomMessageBox;
using System.Net;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.ComponentModel;

namespace AppWindow{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private bool folderOpen = false;
        public Manager Manager { get; set; }


        public MainWindow(){
            InitializeComponent();
            Manager = new Manager();
            DataContext = Manager;
        }

        private void LoadButtonButton_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = CustomMessageBox.ShowYesNoCancel("load a folder or an image ? ", "Load file(s)", "Folder", "Image", "Url");
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            if (MessageBoxResult.Yes == result)
            {
                folderOpen = true;
                dialog.IsFolderPicker = folderOpen;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    pathTextBlock.Text = dialog.FileName;
                }
            }
            else if (MessageBoxResult.No == result)
            {
                folderOpen = false;
                dialog.Filters.Add(new CommonFileDialogFilter("Image files", "*.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.bin"));
                dialog.IsFolderPicker = folderOpen;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    pathTextBlock.Text = dialog.FileName;
                }
            }
            else if(MessageBoxResult.Cancel == result)
            {
                folderOpen = false;
                string url = Interaction.InputBox("Image URL:","Load url");
                if(url != "")
                {
                    try
                    {
                        Random random = new Random();
                        WebClient webClient = new WebClient();
                        string randomStr = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 28).Select(s => s[random.Next(s.Length)]).ToArray());
                        string fileName = System.IO.Path.GetTempPath() + randomStr;
                        webClient.DownloadFile(url, fileName);
                        Bitmap image = new Bitmap(fileName);
                        pathTextBlock.Text = fileName;
                    }
                    catch(Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Error Url.\n" + ex.Message,
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    }
                }
            }
        }
        private void PredictionButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MaxProgress = 1;
            Manager.ValueProgress = 0;
            Manager.ImagePrediction.Clear();
            if (pathTextBlock.Text != "")
            {
                if (folderOpen) Manager.PredictFolder(pathTextBlock.Text);
                else Manager.PredictImage(pathTextBlock.Text);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String sortByLabel = (sender as System.Windows.Controls.ComboBox).SelectedValue as String;
            Manager.SortResultsBy(sortByLabel);
        }
    }
}
