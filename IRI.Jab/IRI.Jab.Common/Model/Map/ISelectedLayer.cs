using IRI.Ket.SqlServerSpatialExtension.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Model.Map
{
    public interface ISelectedLayer
    {
        Guid Id { get; }

        ILayer AssociatedLayer { get; set; }

        string LayerName { get;  }

        void UpdateSelectedFeatures(IEnumerable<ISqlGeometryAware> items);

        void UpdateHighlightedFeatures(IEnumerable<ISqlGeometryAware> items);

        bool ShowSelectedOnMap { get; set; }  

        IEnumerable<ISqlGeometryAware> GetSelectedFeatures();

        int CountOfSelectedFeatures();

        IEnumerable<ISqlGeometryAware> GetHighlightedFeatures();

        void UpdateSelectedFeaturesOnMap(IEnumerable<ISqlGeometryAware> enumerable);

        void UpdateHighlightedFeaturesOnMap(IEnumerable<ISqlGeometryAware> enumerable);

        Action<IEnumerable<ISqlGeometryAware>> FeaturesChangedAction { get; set; }

        Action<IEnumerable<ISqlGeometryAware>> HighlightFeaturesChangedAction { get; set; }

        Action<IEnumerable<ISqlGeometryAware>> ZoomTo { get; set; }
         
        Action RequestRemove { get; set; }
    }
}
