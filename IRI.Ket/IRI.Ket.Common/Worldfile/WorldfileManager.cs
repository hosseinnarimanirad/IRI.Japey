using IRI.Sta.Common.Primitives;
using IRI.Sta.Common.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace IRI.Ket.WorldfileFormat
{
    public static class WorldfileManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldFileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static BoundingBox ReadImageBoundingBox(string worldFileName, int width, int height)
        {
            if (!System.IO.File.Exists(worldFileName))
            {
                throw new NotImplementedException();
            }

            string[] lines = System.IO.File.ReadAllLines(worldFileName);

            double xPixelSize = double.Parse(lines[0]);

            double rotationAboutY = double.Parse(lines[1]);

            double rotationAboutX = double.Parse(lines[2]);

            double yPixelSize = double.Parse(lines[3]);

            yPixelSize = (yPixelSize > 0) ? yPixelSize : -yPixelSize;

            double xOfCenterOfUpperLeftPixel = double.Parse(lines[4]);

            double yOfCenterOfUpperLeftPixel = double.Parse(lines[5]);

            return new BoundingBox(xMin: xOfCenterOfUpperLeftPixel - xPixelSize / 2.0,
                                                yMin: (yOfCenterOfUpperLeftPixel + yPixelSize / 2.0) - yPixelSize * height,
                                                xMax: (xOfCenterOfUpperLeftPixel - xPixelSize / 2.0) + xPixelSize * width,
                                                yMax: yOfCenterOfUpperLeftPixel + yPixelSize / 2.0);
        }

        public static void SaveWorldFile(string fileName, Worldfile worldfile)
        {
            StringBuilder lines = new StringBuilder();

            lines.AppendLine(worldfile.XPixelSize.ToInvariantString());
            lines.AppendLine(worldfile.YRotation.ToInvariantString());
            lines.AppendLine(worldfile.XRotation.ToInvariantString());
            lines.AppendLine((-worldfile.YPixelSize).ToInvariantString());

            lines.AppendLine(worldfile.CenterOfUpperLeftPixel.X.ToInvariantString());
            lines.AppendLine(worldfile.CenterOfUpperLeftPixel.Y.ToInvariantString());

            System.IO.File.WriteAllText(fileName, lines.ToString());
        }

        public static GeoReferencedImage ReadWorldfile(string imageFileName)
        {
            if (!System.IO.File.Exists(imageFileName))
                return null;

            var size = IRI.Ket.Common.Helpers.ImageHelper.GetSize(imageFileName);

            var worldFileName = TryGetAssociatedWorldfileName(imageFileName);

            //var extension = System.IO.Path.GetExtension(imageFileName);

            //var worldFileNameWithoutExtension = $"{System.IO.Path.GetDirectoryName(imageFileName)}\\{System.IO.Path.GetFileNameWithoutExtension(imageFileName)}";

            //95.01.18
            //string worldFileName = string.Empty;

            //switch (extension.ToLower())
            //{
            //    case ".bmp":
            //        worldFileName = $"{worldFileNameWithoutExtension}.bpw";

            //        if (!System.IO.File.Exists(worldFileName))
            //            worldFileName = $"{worldFileNameWithoutExtension}.bmpw";

            //        break;

            //    case ".jpg":
            //    case ".jpeg":
            //        worldFileName = $"{worldFileNameWithoutExtension}.jpgw";

            //        if (!System.IO.File.Exists(worldFileName))
            //            worldFileName = $"{worldFileNameWithoutExtension}.jpegw";

            //        break;

            //    case ".tif":
            //        worldFileName = $"{worldFileNameWithoutExtension}.tfw";

            //        break;

            //    default:
            //        break;
            //}

            try
            {
                var boundingBox = ReadImageBoundingBox(worldFileName, size.Width, size.Height);

                return new GeoReferencedImage(System.IO.File.ReadAllBytes(imageFileName), boundingBox);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static Worldfile TryReadAssociatedWorldfile(string imageFileName)
        {
            var worldFileName = TryGetAssociatedWorldfileName(imageFileName);

            if (worldFileName != null)
            {
                return Worldfile.Read(worldFileName);
            }

            return null;
        }

        public static string TryGetAssociatedWorldfileName(string imageFileName)
        {
            string worldFileName = string.Empty;

            var extension = System.IO.Path.GetExtension(imageFileName).ToLower();

            switch (extension)
            {
                case ".bmp":
                    worldFileName = System.IO.Path.ChangeExtension(imageFileName, "bpw");

                    if (!System.IO.File.Exists(worldFileName))
                        worldFileName = System.IO.Path.ChangeExtension(imageFileName, "bmpw");
                    break;

                case ".jpg":
                case ".jpeg":
                    worldFileName = System.IO.Path.ChangeExtension(imageFileName, "jpgw");

                    if (!System.IO.File.Exists(worldFileName))
                        worldFileName = System.IO.Path.ChangeExtension(imageFileName, "jpegw");
                    break;

                case ".tif":
                    worldFileName = System.IO.Path.ChangeExtension(imageFileName, "tfw");
                    break;

                case ".png":
                    worldFileName = System.IO.Path.ChangeExtension(imageFileName, "pgw");
                    break;

                default:
                    break;
            }

            if (System.IO.File.Exists(worldFileName))
            {
                return worldFileName;
            }

            return null;
        }

        public static string MakeAssociatedWorldfileName(string imageFileName)
        {
            var extension = Common.Helpers.PathHelper.GetExtensionWithoutDot(imageFileName);

            return System.IO.Path.ChangeExtension(imageFileName, $"{extension}w");
        }

        public static Worldfile Create(BoundingBox mapBoundingBox, int imagePixelWidth, int imagePixelHeight)
        {
            var xPixelSize = mapBoundingBox.Width / imagePixelWidth;

            var yPixelSize = mapBoundingBox.Height / imagePixelHeight;

            return new Worldfile(xPixelSize, yPixelSize, new Point(mapBoundingBox.XMin + xPixelSize / 2.0, mapBoundingBox.YMax - yPixelSize / 2.0));
        }

        public static Worldfile CreateByGeodeticValues(Sta.Common.Model.TileInfo tile, int imagePixelWidth = 256, int imagePixelHeight = 256)
        {
            return Create(tile.GeodeticExtent, imagePixelWidth, imagePixelHeight);
        }

        public static Worldfile CreateByWebMercatorValues(Sta.Common.Model.TileInfo tile, int imagePixelWidth = 256, int imagePixelHeight = 256)
        {
            return Create(tile.WebMercatorExtent, imagePixelWidth, imagePixelHeight);
        }
    }
}

