using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

namespace IRI.Jab.Common.Assets.Converters
{
    public class FlowDocumentXmlToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            var flowDocument = value as FlowDocument;

            //return new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd).Text;

            var range = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);

            string result = string.Empty;

            using (MemoryStream buffer = new MemoryStream())
            {
                range.Save(buffer, DataFormats.Xaml);

                result = Encoding.UTF8.GetString(buffer.ToArray());
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            //return value;
            FlowDocument result = new FlowDocument();

            if (!string.IsNullOrEmpty(value as string))
            {
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(value as string)))
                {
                    result = XamlReader.Load(stream) as FlowDocument;
                }
            }

            return result;
        }
    }
}
