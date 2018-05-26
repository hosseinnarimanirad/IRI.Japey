using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.Common.Model
{
    public class ImageSource
    {
        private Func<int, int, int, string> MakeFileName { get; set; }

        //public string ImageDirectory { get; set; }

        //public string ImagePrefix { get; set; }

        //public string FileNamePattern { get; set; }

        //public string FileExtension { get; set; }

        public int ZoomLevel { get; set; }

        //public bool IsZoomLevelIncluded { get; set; }

        public ImageSource(int zoomLevel, Func<int, int, int, string> makeFileName)
        {
            this.ZoomLevel = zoomLevel;

            this.MakeFileName = makeFileName;

            //this.ImageDirectory = imageDirectory;

            //this.ImagePrefix = imagePrefix;

            //this.FileExtension = fileExtension;

            //this.FileNamePattern = FileNamePattern;

            //this.IsZoomLevelIncluded = isZoomLevelIncluded;
        }

        public string GetFileName(int row, int column)
        {
            return this.MakeFileName(row, column, this.ZoomLevel);
        }
    }
}
