using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibliothequeDeClasse
{
    class ImageData
    {
        //Chemin de l'image
        [LoadColumn(0)] //<- ML.Data
        public string ImagePath;

        //Valeur de l'étiquette
        [LoadColumn(1)] //<- ML.Data
        public string Label;
    }
}
