using IRI.Ket.ShapefileFormat.Dbf;
using IRI.Ket.ShapefileFormat.EsriType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.ShapefileFormat.Model
{
    public class ObjectToDbfTypeMap<T>
    {
        public Func<T, object> MapFunction { get; set; }

        public DbfFieldDescriptor FieldType { get; set; }

        public ObjectToDbfTypeMap(DbfFieldDescriptor fieldType, Func<T, object> mapFunction)
        {
            FieldType = fieldType;

            MapFunction = mapFunction;
        }
    }
}
