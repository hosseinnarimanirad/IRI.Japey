using IRI.Ham.CoordinateSystem;
using IRI.Ham.CoordinateSystem.MapProjection;
using IRI.Ham.SpatialBase;
using IRI.Ham.SpatialBase.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ham.SpatialBase.Model
{
    public class TileInfo
    {
        public int RowNumber { get; private set; }

        public int ColumnNumber { get; private set; }

        public int ZoomLevel { get; private set; }

        public BoundingBox GeodeticExtent { get; private set; }

        //public BoundingBox MercatorExtent
        //{
        //    get
        //    {
        //        //var topLeft = IRI.Ham.CoordinateSystem.Projection.GeodeticToMercator(GeodeticExtent.TopLeft);
        //        //var bottomRight = IRI.Ham.CoordinateSystem.Projection.GeodeticToMercator(GeodeticExtent.BottomRigth);

        //        //return new BoundingBox(Math.Min(topLeft.X, bottomRight.X), Math.Min(topLeft.Y, bottomRight.Y), Math.Max(topLeft.X, bottomRight.X), Math.Max(topLeft.Y, bottomRight.Y));
        //        return GeodeticExtent.Transform(i => MapProjects.GeodeticToMercator(i));
        //    }
        //}

        public BoundingBox WebMercatorExtent
        {
            get
            {
                return GeodeticExtent.Transform(i => MapProjects.GeodeticWgs84ToWebMercator(i));
            }
        }        

        /// <summary>
        /// Extent based on sphere
        /// </summary>
        public BoundingBox GeocentricExtent
        {
            get
            {
                return GeodeticExtent.Transform(i => Transformation.ChangeDatum(i, Ellipsoids.WGS84, Ellipsoids.Sphere));
            }
        }

        public TileInfo(int row, int column, int zoomLevel)
        {
            this.RowNumber = row;

            this.ColumnNumber = column;

            this.ZoomLevel = zoomLevel;

            this.GeodeticExtent = WebMercatorUtility.GetWgs84ImageBoundingBox(row, column, zoomLevel);
        }

        public override string ToString()
        {
            return $"Z:{ZoomLevel}, R: {RowNumber}, C: {ColumnNumber}, W:{WebMercatorExtent.Width}, H:{WebMercatorExtent.Height}, X:{WebMercatorExtent.TopLeft.X}, Y:{WebMercatorExtent.TopLeft.Y}";
        }

        public string ToShortString()
        {
            return $"{ZoomLevel}, {RowNumber}, {ColumnNumber}";
        }

        public override bool Equals(object obj)
        {
            var other = obj as TileInfo;

            if (other == null)
            {
                return false;
            }

            return this.ZoomLevel == other.ZoomLevel && this.RowNumber == other.RowNumber && this.ColumnNumber == other.ColumnNumber;
        }

        public override int GetHashCode()
        {
            return $"{ZoomLevel}{RowNumber}{ColumnNumber}".GetHashCode();
        }

        
    }
}
