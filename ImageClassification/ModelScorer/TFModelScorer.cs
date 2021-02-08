using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using ImageClassification.ImageDataStructures;
using static ImageClassification.ModelScorer.ConsoleHelpers;
using static ImageClassification.ModelScorer.ModelHelpers;

namespace ImageClassification.ModelScorer
{
    public class TFModelScorer
    {
        private readonly string dataLocation;
        private readonly string imagesFolder;
        private readonly string modelLocation;
        private readonly string labelsLocation;
        private readonly MLContext mlContext;
        private static string ImageReal = nameof(ImageReal);

        //public PredictionEngine<ImageNetData, ImageNetPrediction> Model { get; set; }

        public TFModelScorer(string dataLocation, string imagesFolder, string modelLocation, string labelsLocation)
        {
            this.dataLocation = dataLocation;
            this.imagesFolder = imagesFolder;
            this.modelLocation = modelLocation;
            this.labelsLocation = labelsLocation;
            mlContext = new MLContext();

            //Model = LoadModel(dataLocation, imagesFolder, modelLocation);
        }

        public struct ImageNetSettings
        {
            public const int imageHeight = 224;
            public const int imageWidth = 224;
            public const float mean = 117;
            public const bool channelsLast = true;
        }

        public struct InceptionSettings
        {
            // for checking tensor names, you can use tools like Netron,
            // which is installed by Visual Studio AI Tools

            // input tensor name
            public const string inputTensorName = "input";

            // output tensor name
            public const string outputTensorName = "softmax2";
        }

        public void Score()
        {
            var model = LoadModel(dataLocation, imagesFolder, modelLocation);

            var predictions = PredictDataUsingModel(dataLocation, imagesFolder, labelsLocation, model).ToArray();

        }
        public ImageNetDataProbability[] Score(string imagePath)
        {
            var model = LoadModel(dataLocation, imagesFolder, modelLocation);

            var predictions = PredictImageUsingModel(dataLocation, imagePath, labelsLocation, model).ToArray();

            return predictions;
        }

        private PredictionEngine<ImageNetData, ImageNetPrediction> LoadModel(string dataLocation, string imagesFolder, string modelLocation)
        { 
            var data = mlContext.Data.LoadFromTextFile<ImageNetData>(dataLocation, hasHeader: true);

            var pipeline = mlContext.Transforms.LoadImages(outputColumnName: "input", imageFolder: imagesFolder, inputColumnName: nameof(ImageNetData.ImagePath))
                            .Append(mlContext.Transforms.ResizeImages(outputColumnName: "input", imageWidth: ImageNetSettings.imageWidth, imageHeight: ImageNetSettings.imageHeight, inputColumnName: "input"))
                            .Append(mlContext.Transforms.ExtractPixels(outputColumnName: "input", interleavePixelColors: ImageNetSettings.channelsLast, offsetImage: ImageNetSettings.mean))
                            .Append(mlContext.Model.LoadTensorFlowModel(modelLocation).
                            ScoreTensorFlowModel(outputColumnNames: new[] { "softmax2" },
                                                inputColumnNames: new[] { "input" }, addBatchDimensionInput:true));
                        
            ITransformer model = pipeline.Fit(data);

            var predictionEngine = mlContext.Model.CreatePredictionEngine<ImageNetData, ImageNetPrediction>(model);

            return predictionEngine;
        }

        protected IEnumerable<ImageNetData> PredictDataUsingModel(string testLocation, 
                                                                  string imagesFolder, 
                                                                  string labelsLocation, 
                                                                  PredictionEngine<ImageNetData, ImageNetPrediction> model)
        {
            ConsoleWriteHeader("Classify images");
            Console.WriteLine($"Images folder: {imagesFolder}");
            Console.WriteLine($"Training file: {testLocation}");
            Console.WriteLine($"Labels file: {labelsLocation}");

            var labels = ModelHelpers.ReadLabels(labelsLocation);

            var testData = ImageNetData.ReadFromCsv(testLocation, imagesFolder);///////////////////////////////////////Chargement des images

            foreach (var sample in testData)
            {
                var probs = model.Predict(sample).PredictedLabels;
                var imageData = new ImageNetDataProbability()
                {
                    ImagePath = sample.ImagePath,
                    Label = sample.Label
                };
                (imageData.PredictedLabel, imageData.Probability) = GetBestLabel(labels, probs);
                imageData.ConsoleWrite();
                yield return imageData;
            }
        }
        protected IEnumerable<ImageNetDataProbability> PredictImageUsingModel(string testLocation,
                                                                  string imageFile,
                                                                  string labelsLocation,
                                                                  PredictionEngine<ImageNetData, ImageNetPrediction> model)
        {
            var labels = ModelHelpers.ReadLabels(labelsLocation);

            var testData = new ImageNetData { ImagePath = imageFile };

            var probs = model.Predict(testData).PredictedLabels;
            var imageData = new ImageNetDataProbability()
            {
                ImagePath = testData.ImagePath,
                Label = testData.Label
            };
            (imageData.PredictedLabel, imageData.Probability) = GetBestLabel(labels, probs);
            yield return imageData;
        }
    }
}
