using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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
    }
}
