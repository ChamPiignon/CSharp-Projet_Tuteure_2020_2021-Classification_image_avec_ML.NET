using ImageClassification.ImageDataStructures;
using ImageClassification.ModelScorer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

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

        public Manager()
        {
            assetsRelativePath = @"../../../assets";//Relative path
            assetsPath = GetAbsolutePath(assetsRelativePath);
            tagsTsv = Path.Combine(assetsPath, "inputs", "images", "tags.tsv");
            imagesFolder = Path.Combine(assetsPath, "inputs", "images");
            inceptionPb = Path.Combine(assetsPath, "inputs", "inception", "tensorflow_inception_graph.pb");
            labelsTxt = Path.Combine(assetsPath, "inputs", "inception", "imagenet_comp_graph_label_strings.txt");

            ImagePrediction = new ObservableCollection<ImageNetDataProbability>();
        }

        public void PredictFolder(string imagePath)
        {
            try
            {
                var modelScorer = new TFModelScorer(tagsTsv, imagesFolder, inceptionPb, labelsTxt);
                //ImagePrediction.Add();

            }
            catch (Exception ex)
            {

            }
        }
        public void PredictImage(string imagePath)
        {
            try
            {
                var modelScorer = new TFModelScorer(tagsTsv, imagesFolder, inceptionPb, labelsTxt);
                ImagePrediction.Add(modelScorer.Score(imagePath)[0]);
            }
            catch (Exception ex)
            {

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
