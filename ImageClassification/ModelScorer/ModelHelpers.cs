using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ImageClassification.Score.ModelScorer
{
        public static class ModelHelpers
        {
            public static (string, float) GetBestLabel(string[] labels, float[] probs)
            {
                var max = probs.Max();
                var index = probs.AsSpan().IndexOf(max);
                return (labels[index], max);
            }

            public static string[] ReadLabels(string labelsLocation)
            {
                return File.ReadAllLines(labelsLocation);
            }

        }

}
