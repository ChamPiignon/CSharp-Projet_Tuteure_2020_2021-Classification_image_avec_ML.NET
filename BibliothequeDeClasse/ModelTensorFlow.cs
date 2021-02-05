using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BibliothequeDeClasse
{
    public class ModelTensorFlow
    {
        static readonly string _assetsPath = Path.Combine(Environment.CurrentDirectory, "assets");
        static readonly string _imagesFolder = Path.Combine(_assetsPath, "images");
        static readonly string _trainTagsTsv = Path.Combine(_imagesFolder, "tags.tsv");
        static readonly string _testTagsTsv = Path.Combine(_imagesFolder, "test-tags.tsv");
        static readonly string _predictSingleImage = Path.Combine(_imagesFolder, "toaster3.jpg");
        static readonly string _inceptionTensorFlowModel = Path.Combine(_assetsPath, "inception", "tensorflow_inception_graph.pb");

        private MLContext mlContext = new MLContext();
    }
}
