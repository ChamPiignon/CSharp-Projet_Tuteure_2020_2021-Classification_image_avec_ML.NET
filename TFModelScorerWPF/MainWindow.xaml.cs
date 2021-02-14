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

namespace AppWindow
{
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
        
        private void LoadButtonButton_Click(object sender, RoutedEventArgs e){

            MessageBoxResult result = CustomMessageBox.ShowYesNo("load a folder or an image ? ", "Load file(s)", "Folder", "Image");
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            if (MessageBoxResult.Yes == result){
                folderOpen = true;
            }
            else{
                folderOpen = false;
                dialog.Filters.Add(new CommonFileDialogFilter("Image files", "*.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.bin"));
            }
            dialog.IsFolderPicker = folderOpen;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok){
                pathTextBlock.Text = dialog.FileName;
            }
        }

        private void PredictionButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.ImagePrediction.Clear();
            if (pathTextBlock.Text != "")
            {
                if (folderOpen) Manager.PredictFolder(pathTextBlock.Text);
                else Manager.PredictImage(pathTextBlock.Text);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String sortByLabel = (sender as ComboBox).SelectedValue as String;
            Manager.SortResultsBy(sortByLabel);
        }
    }
}
