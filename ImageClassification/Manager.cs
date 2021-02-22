using ImageClassification.ImageDataStructures;
using ImageClassification.ModelScorer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageClassification.Score
{
    public class Manager
    {
        private string assetsRelativePath;
        private string assetsPath;
        private string tagsTsv;
        private string imagesFolder;
        private string inceptionPb;
        private string labelsTxt;

        public ObservableCollection<ImageNetDataProbability> ImagePrediction { get; set; }
        private ObservableCollection<ImageNetDataProbability> SavedList { get; set; }
        public ObservableCollection<String> LabelPrediction { get; set; } = new ObservableCollection<string>();

        public void SortResultsBy(String label)
        {
            ImagePrediction.Clear();
            foreach (ImageNetDataProbability image in SavedList)
            {
                if(image.PredictedLabel == label) { ImagePrediction.Add(image); }
            }
        }
        public Manager()
        {
            assetsRelativePath = @"../../../assets";
            assetsPath = GetAbsolutePath(assetsRelativePath);
            tagsTsv = Path.Combine(assetsPath, "inputs", "images", "tags.tsv");
            imagesFolder = Path.Combine(assetsPath, "inputs", "images");
            inceptionPb = Path.Combine(assetsPath, "inputs", "inception", "tensorflow_inception_graph.pb");
            labelsTxt = Path.Combine(assetsPath, "inputs", "inception", "imagenet_comp_graph_label_strings.txt");
            ImagePrediction = new ObservableCollection<ImageNetDataProbability>();
        }

        public void SaveResults()
        {
            String path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ClassificationScorer");
            
            String sessionPath = Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff"));
            int number;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Directory.CreateDirectory(sessionPath);
            foreach (String label in LabelPrediction)
            {
                Directory.CreateDirectory(Path.Combine(sessionPath, label));
                number = 0;
                foreach (ImageNetDataProbability image in SavedList)
                {
                    number++;
                    if(label == image.PredictedLabel)
                        File.Copy(image.ImagePath, Path.Combine(sessionPath,label + "/" + label + number.ToString()+".jpg"), true);
                }
            }
        }

        public void PredictFolder(string folderPath)
        {
            LabelPrediction.Clear();
            try
            {
                foreach (string imagePath in Directory.GetFiles(folderPath))
                {
                    if (Regex.IsMatch(imagePath, @".jpg|.jpeg|.jpe|.jfif|.png|.bin$"))
                    {
                        PredictImage(imagePath);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            foreach (String str in ImagePrediction.Select(o => o.PredictedLabel).Distinct())
            {
                LabelPrediction.Add(str);
            }
            SavedList = new ObservableCollection<ImageNetDataProbability>(ImagePrediction);

            SaveResults();
        }
        public void PredictImage(string imagePath)
        {
            try
            {
                var modelScorer = new TFModelScorer(tagsTsv, imagesFolder, inceptionPb, labelsTxt);
                Debug.WriteLine(imagePath);
                ImagePrediction.Add(modelScorer.ScoreImage(imagePath)[0]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;
            string fullPath = Path.Combine(assemblyFolderPath, relativePath);
            return fullPath;
        }
    }
}
