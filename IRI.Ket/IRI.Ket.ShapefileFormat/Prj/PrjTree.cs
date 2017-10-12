using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.ShapefileFormat.Prj
{
    public class PrjTree
    {
        const string specialCharacter = "~";

        static readonly char[] trimCharacters = new char[] { '\0', ' ', '\n', '\r', '\"' };

        //e.g. GEOGCS
        public string Name { get; set; }

        public List<PrjTree> Children { get; set; }

        public List<string> Values { get; set; }

        public string ValueString
        {
            get
            {
                if (Values.Count > 0)
                {
                    return string.Join(",", Values.Select((i, index) => index == 0 ? $"\"{i}\"" : i));
                }

                return string.Empty;
            }
        }

        public static PrjTree Parse(string wktString)
        {
            wktString = wktString.Replace(Environment.NewLine, string.Empty).Trim();

            var start = wktString.IndexOf('[');

            var end = wktString.LastIndexOf(']');

            var content = wktString.Substring(start + 1, end - start - 1);

            var name = wktString.Replace($"[{content}]", string.Empty);

            return new PrjTree(name, content);
        }

        public PrjTree()
        {

        }

        public PrjTree(string name, params string[] values)
        {
            this.Name = name;

            this.Values = values.ToList();
        }

        public PrjTree(IRI.Ham.CoordinateSystem.IEllipsoid ellipsoid, string title = null)
        {
            this.Name = PrjFile._geogcs;

            this.Values = new List<string>() { title ?? $"GCS_{ellipsoid.EsriName}" };

            //esri write zero for Inverse Flattening of shperes!
            var inverseFlattening = double.IsInfinity(ellipsoid.InverseFlattening) ? 0 : ellipsoid.InverseFlattening;

            var spheroid = new PrjTree(PrjFile._spheroid, ellipsoid.EsriName, ellipsoid.SemiMajorAxis.Value.AsExactString(), inverseFlattening.AsExactString());

            var datum = new PrjTree(PrjFile._datum, $"D_{ellipsoid.EsriName}")
            {
                Children = new List<PrjTree>() { spheroid }
            };

            var primem = new PrjTree(PrjFile._primem, PrjFile._greenwich, "0.0");

            var unit = new PrjTree(PrjFile._unit, PrjFile._degree, PrjFile._degreeValue);

            this.Children = new List<PrjTree>() { datum, primem, unit };
        }

        private PrjTree(string name, string content)
        {
            this.Name = name.Replace(specialCharacter, string.Empty).Trim(trimCharacters);

            this.Children = new List<PrjTree>();

            this.Values = new List<string>();

            var matches = GetParts(content);

            string temporary = content;

            foreach (string match in matches)
            {
                temporary = temporary.Replace($"[{match}]", specialCharacter);
            }

            var parts = temporary.Split(',');

            int matchIndex = 0;

            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Contains(specialCharacter))
                {
                    this.Children.Add(new PrjTree(parts[i], matches[matchIndex]));

                    matchIndex++;
                }
                else
                {
                    this.Values.Add(parts[i].Trim(trimCharacters));
                }
            }

        }

        private static List<string> GetParts(string input)
        {
            var brackets = input.Select((i, index) => new Bracket { Character = i, Index = index }).Where(i => i.Character == ']' || i.Character == '[').ToList();

            int level = 0;

            for (int i = 0; i < brackets.Count; i++)
            {
                if (brackets[i].Character == '[')
                {
                    brackets[i].Level = level;

                    level++;
                }
                else
                {
                    level--;

                    brackets[i].Level = level;
                }
            }

            var zeroLevelBrakets = brackets.Where(i => i.Level == 0).ToList();

            List<Tuple<int, int>> ranges = new List<Tuple<int, int>>();

            for (int i = 0; i < zeroLevelBrakets.Count / 2; i++)
            {
                ranges.Add(new Tuple<int, int>(zeroLevelBrakets[2 * i].Index, zeroLevelBrakets[2 * i + 1].Index));
            }

            return ranges.Select(i => input.Substring(i.Item1 + 1, i.Item2 - i.Item1 - 1)).ToList();
        }

        public override string ToString()
        {
            return $"Name: {Name}, Children: {Children.Count}, Values:{string.Join(",", Values)}";
        }

        public string AsEsriCrsWkt()
        {
            if (Children?.Count > 0)
            {
                return $"{Name}[{ValueString},{string.Join(",", Children.Select(i => i.AsEsriCrsWkt()))}]";
            }
            else
            {
                return $"{Name}[{ValueString}]";
            }
        }

        private static PrjTree _meter;

        public static PrjTree MeterUnit
        {
            get
            {
                if (_meter == null)
                {
                    _meter = new PrjTree()
                    {
                        Name = PrjFile._unit,
                        Values = new List<string>() { "Meter", "1.0" }
                    };
                }

                return _meter;
            }
        }

        class Bracket
        {
            public int Level { get; set; }

            public char Character { get; set; }

            public int Index { get; set; }
        }
    }
}

