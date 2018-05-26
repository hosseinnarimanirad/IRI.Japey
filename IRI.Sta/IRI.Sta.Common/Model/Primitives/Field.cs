using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Sta.Common.Model
{
    public class Field
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Alias { get; set; }

        public int Length { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}; Type: {Type}; Length: {Length}";
        }
    }
}
