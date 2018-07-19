using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;

namespace IRI.Ket.SqlServerSpatialExtension.Model
{
    public class SqlFeature : ISqlGeometryAware, ICustomTypeDescriptor
    {
        const string _defaultLabelAttributeName = "Label";

        private SqlGeometry _geometry;

        public SqlGeometry TheSqlGeometry
        {
            get
            {
                return _geometry;
            }

            set
            {
                _geometry = value;
            }
        }

        private Dictionary<string, object> _attributes;

        public Dictionary<string, object> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        public override string ToString()
        {
            return $"Geometry: {TheSqlGeometry?.STGeometryType()}, Attributes: {Attributes?.Count}";
        }

        public SqlFeature(SqlGeometry geometry)
        {
            this.TheSqlGeometry = geometry;

            this.Attributes = new Dictionary<string, object>();
        }

        public SqlFeature(SqlGeometry geometry, string label)
        {
            this.TheSqlGeometry = geometry;

            this.Attributes = new Dictionary<string, object>() { { _defaultLabelAttributeName, label } };
        }

        public SqlFeature()
        {

        }

        public string LabelAttribute { get; set; } = _defaultLabelAttributeName;

        public string Label
        {
            get
            {
                if (Attributes?.Keys.Any(k => k == LabelAttribute) == true)
                {
                    return Attributes[LabelAttribute]?.ToString();
                }

                return string.Empty;
            }
        }


        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return Attributes;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        PropertyDescriptorCollection
            System.ComponentModel.ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(new Attribute[0]);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            System.Collections.ArrayList properties = new System.Collections.ArrayList();
            foreach (var e in Attributes)
            {
                properties.Add(new DictionaryPropertyDescriptor(Attributes, e.Key));
            }

            PropertyDescriptor[] props =
                (PropertyDescriptor[])properties.ToArray(typeof(PropertyDescriptor));

            return new PropertyDescriptorCollection(props);
        }

    }




    public class DictionaryPropertyDescriptor : PropertyDescriptor
    {
        System.Collections.IDictionary _dictionary;
        object _key;

        internal DictionaryPropertyDescriptor(System.Collections.IDictionary d, object key)
            : base(key.ToString(), null)
        {
            _dictionary = d;
            _key = key;
        }

        public override Type PropertyType
        {
            get { return _dictionary[_key]?.GetType() ?? typeof(string); }
        }

        public override void SetValue(object component, object value)
        {
            _dictionary[_key] = value;
        }

        public override object GetValue(object component)
        {
            return _dictionary[_key];
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type ComponentType
        {
            get { return null; }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
