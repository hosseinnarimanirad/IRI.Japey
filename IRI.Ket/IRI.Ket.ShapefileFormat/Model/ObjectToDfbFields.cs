using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.ShapefileFormat.Model
{
    public class ObjectToDfbFields<T>
    {
        public Func<T, List<Object>> ExtractAttributesFunc { get; set; }

        public List<Dbf.DbfFieldDescriptor> Fields { get; set; }
    }
}
