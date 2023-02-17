using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Msh.Common.Primitives; 
using IRI.Msh.Common.Model;

namespace IRI.Ket.Persistence.RasterDataSources
{
    //Note: Extent is NaN for this class
    public class ImagePyramidDataSource : OfflineGoogleMapDataSource<object>
    {
        public ImagePyramidDataSource(string directory, Func<int, int, int, string>? makeFileName = null) : base(new List<ImageSource>())
        {
            var availableZoomLevels = System.IO.Directory.EnumerateDirectories(directory, "*.*", System.IO.SearchOption.TopDirectoryOnly).ToList();

            if (makeFileName == null)
            {
                makeFileName = (r, c, z) => $"{directory}\\{z}\\{z}, {r}, {c}.jpg";
            }

            var sources = new List<ImageSource>();

            foreach (var zoomLevelDirectory in availableZoomLevels)
            {
                int zoom;

                var folderName = System.IO.Path.GetFileName(zoomLevelDirectory);

                int.TryParse(folderName, out zoom);

                this.ImageSources.Add(new ImageSource(zoom, makeFileName));
            }
        }

        
    }
}
