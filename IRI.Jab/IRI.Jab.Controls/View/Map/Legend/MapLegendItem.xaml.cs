using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using IRI.Jab.Common.Model;

namespace IRI.Jab.Controls.View.Map;


/// <summary>
/// Interaction logic for legendItem.xaml
/// </summary>
public partial class MapLegendItem : UserControl
{
    public delegate void layerOpacityValueChangedEventHandler(object sender, RoutedPropertyChangedEventArgs<double> e);

    public event layerOpacityValueChangedEventHandler LayerOpacityValueChanged;

    public delegate void layerVisibilityChangedEventHandler(object sender, RoutedEventArgs e);

    public event layerVisibilityChangedEventHandler LayerVisibilityChanged;

    public delegate void layerStyleChangedEventHandler(object sender, MouseButtonEventArgs e);

    public event layerStyleChangedEventHandler LayerStyleChanged;

    //private bool IsPolylineFeature;
    LayerType type;

    //FeatureType _featureType;
    //private string layerName_2;
    //private CartographyParameters parameters;
    //private LayerType layerType;

    //private FeatureType featureType;

    public MapLegendItem(string layerName, IRI.Jab.Common.VisualParameters parameters, LayerType type)
    {
        InitializeComponent();

        //this._featureType = featureType;
        //this.isFeatureType = false;

        this.type = type;

        Initialize(layerName, parameters);
    }

    //public MapLegendItem(string layerName, CartographyParameters parameters, FeatureType featureType)
    //{
    //    InitializeComponent();

    //    this.type = LayerType.Feature;

    //    this.featureType = featureType;

    //    this.isFeatureType = true;

    //    Initialize(layerName, parameters);
    //}

    private void Initialize(string layerName, IRI.Jab.Common.VisualParameters parameters)
    {
        this.layerName.Text = layerName;

        this.layerName.ToolTip = layerName;

        this.layerOpacity.Value = parameters.Opacity;

        this.layerVisibility.IsChecked = parameters.Visibility == System.Windows.Visibility.Visible;

        if (this.type != LayerType.Feature)
        {
            this.layerSymbol.Child = new Rectangle()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Stroke = parameters.Stroke,
                StrokeThickness = 1.0,
                Fill = new SolidColorBrush(Colors.LightBlue),
                Width = 5,
                Height = 5
            };

            return;
        }

        Shape featuer;

        //this.type = featureType;

        if (this.type == LayerType.Polyline)
        {
            featuer = new Line()
            {
                StrokeThickness = 3,
                X1 = 0,
                Y1 = 10,
                X2 = 10,
                Y2 = 10,
                //Stretch = Stretch.Fill,
                Stroke = parameters.Stroke,
                //Fill = parameters.FillBrush,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };
        }
        else if (this.type == LayerType.Point)
        {
            featuer = new Rectangle()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Stroke = parameters.Stroke,
                StrokeThickness = 1.0,
                Fill = parameters.Fill,
                Width = 5,
                Height = 5
            };
        }
        else if (this.type == LayerType.Polygon)
        {
            featuer = new Path()
            {
                Data = Geometry.Parse("F1 M 0.499,10.500L 9.769,10.500L 6.342,6.005L 9.825,1.230L 4.264,0.499L 0.837,4.938L 0.499,10.500 Z"),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Stroke = parameters.Stroke,
                StrokeThickness = parameters.StrokeThickness,
                Fill = parameters.Fill,
                Width = 15,
                //Height = 15
            };
        }
        else
        {
            throw new NotImplementedException();
        }

        this.layerSymbol.Child = featuer;
    }

    public string LayerName
    {
        get { return this.layerName.Text; }
    }

    public double LayerOpacityValue
    {
        get { return this.layerOpacity.Value; }
    }

    public bool IsLayerVisible
    {
        get { return this.layerVisibility.IsChecked == true; }
    }

    private void layerOpacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (this.LayerOpacityValueChanged != null)
        {
            this.LayerOpacityValueChanged(this, e);
        }
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        if (this.LayerVisibilityChanged != null)
        {
            this.LayerVisibilityChanged(this, e);
        }
    }

    private void layerVisibility_Unchecked(object sender, RoutedEventArgs e)
    {
        if (this.LayerVisibilityChanged != null)
        {
            this.LayerVisibilityChanged(this, e);
        }
    }

    private void layerSymbol_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (this.LayerStyleChanged != null)
        {
            this.LayerStyleChanged(this, e);
        }
    }

    public void SetLayerFill(Brush brush)
    {
        ((Shape)this.layerSymbol.Child).Fill = brush;

        //if (type == FeatureType.PolylineFeature)
        //{
        //    throw new NotImplementedException();
        //}

        //((Rectangle)(this.layerSymbol.Child)).Fill = brush;
    }

    public void SetLayerStoke(Brush brush)
    {
        ((Shape)this.layerSymbol.Child).Stroke = brush;

        //if (type == FeatureType.PolylineFeature)
        //{
        //    ((Line)(this.layerSymbol.Child)).Stroke = brush;
        //}
        //else if(type== FeatureType.PointFeature)
        //{
        //    ((Rectangle)(this.layerSymbol.Child)).Stroke = brush;
        //}
        //else
        //{

        //}

    }

    public void SetLayerStrokeThickness(double value)
    {
        ((Shape)this.layerSymbol.Child).StrokeThickness = value;

        //if (type == FeatureType.PolylineFeature)
        //{
        //    ((Line)(this.layerSymbol.Child)).StrokeThickness = value;
        //}
        //else
        //{
        //    ((Rectangle)(this.layerSymbol.Child)).StrokeThickness = value;
        //}
    }
}
