//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;

//namespace IRI.Jab.Common.Model
//{
//    public class MoveableItem : Locateable
//    {
//        public override FrameworkElement Element
//        {
//            get
//            {
//                return base.Element;
//            }

//            set
//            {
//                base.Element = value;
//            }
//        }

//        public MoveableItem()
//        {

//            //var element = item.Element;

//            //var screenLocation = item.AncherFunction(
//            //    MapToScreen(new Point(item.X, item.Y)), element.Width, element.Height);

//            ////element.RenderTransformOrigin = new Point(.5, .5);
//            //var tempPoint = item.AncherFunction(new Point(0, 0), element.Width, element.Height);

//            //element.RenderTransformOrigin = new Point(-tempPoint.X / element.Width, -tempPoint.Y / element.Height);

//            //var scaleTransform = ((TransformGroup)(element.RenderTransform)).Children.First();
//            //((TransformGroup)(element.RenderTransform)).Children.Clear();

//            //((TransformGroup)(element.RenderTransform)).Children.Add(scaleTransform);

//            //((TransformGroup)(element.RenderTransform)).Children.Add(this.panTransformForPoints);

//            //((TransformGroup)(element.RenderTransform)).Children.Add(new TranslateTransform(screenLocation.X, screenLocation.Y));

//            //element.Tag = new LayerTag(this.MapScale) { Layer = null, IsTiled = false, LayerType = LayerType.MoveableItem };

//            //AddToCanvasWithAnimation(element, element.Opacity);
//        }

//        public void Element_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
//        {
//            this.Element.CaptureMouse();

//            this.Element.MouseMove -= Element_MouseMove;
//            this.Element.MouseMove += Element_MouseMove;

//            this.Element.MouseUp -= Element_MouseUp;
//            this.Element.MouseUp += Element_MouseUp;
//        }

//        public void Element_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
//        {
//            throw new NotImplementedException();
//        }

//        public void Element_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)

//        {
//            throw new NotImplementedException();
//        }

//    }
//}
