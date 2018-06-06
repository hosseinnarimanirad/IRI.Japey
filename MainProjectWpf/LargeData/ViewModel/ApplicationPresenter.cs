using IRI.Jab.Common;
using IRI.MainProjectWPF.LargeData.Model;
using IRI.Msh.Common.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Jab.Common.Model;

namespace IRI.MainProjectWPF.LargeData.ViewModel
{
    public class ApplicationPresenter : Notifier
    {

        //public List<double> StandardScales
        //{
        //    get
        //    {
        //        return new List<double> { 1128.497176, 2256.994353, 4513.988705, 9027.977411, 18055.954822, 36111.909643, 72223.819286, 144447.638572, 288895.277144, 577790.554289, 1155581.108577, 2311162.217155, 4622324.434309, 9244648.868618, 18489297.737236, 36978595.474472, 73957190.948944, 147914381.897889, 295828763.795777, 591657527.591555, };
        //    }
        //}
        public List<GoogleScale> StandardScales
        {
            get
            {
                return GoogleScale.Scales;
            }
        }

        private ObservableCollection<ShapeCollection> _shapeCollections;

        public ObservableCollection<ShapeCollection> ShapeCollections
        {
            get { return _shapeCollections; }
            set
            {
                _shapeCollections = value;
                RaisePropertyChanged();
            }
        }

        private ShapeCollection _currentShapeCollection;

        public ShapeCollection CurrentShapeCollection
        {
            get { return _currentShapeCollection; }
            set
            {
                _currentShapeCollection = value;
                RaisePropertyChanged();
            }
        }

        private int _zoomLevel;

        public int ZoomLevel
        {
            get { return _zoomLevel; }
            set
            {
                _zoomLevel = value;
                RaisePropertyChanged();

                var unitDistance = WebMercatorUtility.CalculateGroundResolution(value, 35);

                this.AreaThreshold = unitDistance * unitDistance;
            }
        }


        private double _areaThreshold;

        public double AreaThreshold
        {
            get { return _areaThreshold; }
            set
            {
                _areaThreshold = value;
                RaisePropertyChanged();
            }
        }


        private double _angleThreshold;

        public double AngleThreshold
        {
            get { return _angleThreshold; }
            set
            {
                _angleThreshold = value;
                RaisePropertyChanged();
            }
        }


        public ApplicationPresenter()
        {
            this.ShapeCollections = new ObservableCollection<ShapeCollection>();

            this.AngleThreshold = .98;
        }

        private double _estimatedScale;

        public double EstimatedScale 
        {
            get { return _estimatedScale; }
            set
            {
                _estimatedScale = value;
                RaisePropertyChanged();
            }
        }

         
    }
}
