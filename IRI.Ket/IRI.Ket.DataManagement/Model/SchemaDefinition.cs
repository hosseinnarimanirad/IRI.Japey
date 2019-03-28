//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace IRI.Ket.DataManagement.Model
//{
//    public class SchemaDefinition<T>
//    {
//        private string _columnName;

//        public string ColumnName
//        {
//            get { return _columnName; }
//            set { _columnName = value; }
//        }

//        private string _type;

//        public string Type
//        {
//            get { return _type; }
//            set { _type = value; }
//        }

//        private Func<T, object> _extractFunc;

//        public Func<T, object> ExtractFunc
//        {
//            get { return _extractFunc; }
//            set { _extractFunc = value; }
//        }

//        private bool _isSpatialColumn;

//        public bool IsSpatialColumn
//        {
//            get { return _isSpatialColumn; }
//            set { _isSpatialColumn = value; }
//        }

//        private bool _isWkbType = false;

//        public bool IsWkbType
//        {
//            get { return _isWkbType; }
//            set { _isWkbType = value; }
//        }

//        private bool _isWktType = false;

//        public bool IsWktType
//        {
//            get { return _isWktType; }
//            set { _isWktType = value; }
//        }

//        public SchemaDefinition(string columnName, string type, Func<T, object> extractFunc, bool isSpatialColumn = false)
//        {
//            this.ColumnName = columnName;

//            this.Type = type;

//            this.ExtractFunc = extractFunc;

//            this.IsSpatialColumn = isSpatialColumn;
//        }
//    }
//}
