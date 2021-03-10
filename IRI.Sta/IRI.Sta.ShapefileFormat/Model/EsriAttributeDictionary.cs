using IRI.Ket.ShapefileFormat.Dbf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.ShapefileFormat.Model
{
    public class EsriAttributeDictionary
    {
        public List<Dictionary<string, object>> Attributes { get; set; }

        public List<DbfFieldDescriptor> Fields { get; set; }

        public EsriAttributeDictionary()
        {

        }

        public EsriAttributeDictionary(List<Dictionary<string,object>> attributes, List<DbfFieldDescriptor> fields)
        {
            this.Attributes = attributes;

            this.Fields = fields;
        }
    }
}
