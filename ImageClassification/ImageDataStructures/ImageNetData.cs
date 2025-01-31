﻿using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageClassification.ImageDataStructures
{
    public class ImageNetData
    {
        [LoadColumn(0)]
        public string ImagePath { get; set; }

        [LoadColumn(1)]
        public string Label;

        public static IEnumerable<ImageNetData> ReadFromCsv(string file, string folder)//File -> tsv et folder -> imageFolder
        {
            return File.ReadAllLines(file)
             .Select(x => x.Split('\t'))
             .Select(x => new ImageNetData { ImagePath = Path.Combine(folder, x[0]), Label = x[1] } );
        }
        public static IEnumerable<ImageNetData> Read(string folder)
        {
            return Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories).ToList().Select(x => new ImageNetData { ImagePath = x, Label = "None" });
        }
    }

    public class ImageNetDataProbability : ImageNetData
    {
        public string PredictedLabel { get; set; }
        public float Probability { get; set; }
    }
}
