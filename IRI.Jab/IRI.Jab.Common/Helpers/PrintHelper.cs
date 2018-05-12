using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace IRI.Jab.Common.Helpers
{
    public static class PrintHelper
    {
        public static void Print(FrameworkElement visual, bool applyRtl = true)
        {
            var printDlg = new System.Windows.Controls.PrintDialog();

            if (printDlg.ShowDialog() == true)
            {
                //get selected printer capabilities
                System.Printing.PrintCapabilities capabilities = printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);

                //get scale of the print wrt to screen of WPF visual
                double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / visual.ActualWidth, capabilities.PageImageableArea.ExtentHeight / visual.ActualHeight);

                var oldTransfomr = visual.LayoutTransform?.Clone();

                if (applyRtl)
                {
                    //Transform the Visual to scale
                    visual.LayoutTransform = new ScaleTransform(-scale, scale);
                }
                else
                {
                    //Transform the Visual to scale
                    visual.LayoutTransform = new ScaleTransform(scale, scale);
                }

                //get the size of the printer page
                Size size = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

                //update the layout of the visual to the printer page size.
                visual.Measure(size);

                visual.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), size));

                //now print the visual to printer to fit on the one page.
                printDlg.PrintVisual(visual, "First Fit to Page WPF Print");

                visual.LayoutTransform = oldTransfomr;
            }
        }

        //public static void PrintLargeObject(FrameworkElement visual, bool applyRtl = true)
        //{
        //    Print(GetLargeObject(visual));
        //}

        //private static FrameworkElement GetLargeObject(FrameworkElement element)
        //{
        //    FlowDocumentScrollViewer flowDocumentScrollViewer = new FlowDocumentScrollViewer();

        //    FlowDocument doc = new FlowDocument();

        //    doc.Blocks.Add(new Section(new BlockUIContainer(XamlClone(element))));

        //    flowDocumentScrollViewer.Document = doc;

        //    return flowDocumentScrollViewer;
        //}

        //public static UIElement XamlClone(UIElement original) 
        //{
        //    if (original == null)
        //        return (null);

        //    string s = XamlWriter.Save(original);
        //    StringReader stringReader = new StringReader(s);
        //    XmlReader xmlReader = XmlTextReader.Create(stringReader, new XmlReaderSettings());
        //    return (UIElement)XamlReader.Load(xmlReader);

        //}
    }
}
