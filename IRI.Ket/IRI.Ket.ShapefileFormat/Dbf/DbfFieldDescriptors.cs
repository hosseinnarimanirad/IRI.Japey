using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ket.ShapefileFormat.Dbf
{
    public static class DbfFieldDescriptors
    {
        private const byte doubleLength = 19;

        private const byte doubleDecimalCount = 11;

        private const byte integerLength = 9;

        private const byte dateLength = 8;

        private const byte booleanLength = 1;

        private const byte maxStringLength = 255;

        public static DbfFieldDescriptor GetDoubleField(string fieldName)
        {
            return new DbfFieldDescriptor(fieldName, (char)DbfColumnType.FloatingPoint, doubleLength, doubleDecimalCount);
        }

        public static DbfFieldDescriptor GetIntegerField(string fieldName)
        {
            return new DbfFieldDescriptor(fieldName, (char)DbfColumnType.Number, integerLength, 0);
        }

        public static DbfFieldDescriptor GetStringField(string fieldName)
        {
            return GetStringField(fieldName, maxStringLength);
        }

        public static DbfFieldDescriptor GetStringField(string fieldName, byte length)
        {
            return new DbfFieldDescriptor(fieldName, (char)DbfColumnType.Character, length, 0);
        }

        public static DbfFieldDescriptor GetBooleanField(string fieldName)
        {
            return new DbfFieldDescriptor(fieldName, (char)DbfColumnType.Logical, booleanLength, 0);
        }
    }
}
