using IRI.Ket.Data.Model;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Data.Feeds
{
    public static class AdminFeeder
    {
        public static List<City> GetCities()
        {
            return new List<City>();
        }

        public static List<Province> GetProvinces93Wm()
        {
            var fileName = @"E:\Programming\100. IRI.Japey\IRI.Ket\IRI.Ket.Data\Data\ostan93Wm.txt";

            var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<prov>>(System.IO.File.ReadAllText(fileName));

            List<Province> result = new List<Province>();

            foreach (var ostan in jsonObject)
            {
                result.Add(new Province()
                {
                    Name = ostan.Ostan,
                    BoundaryWm = DbGeometry.FromBinary(Convert.FromBase64String(ostan.Border), 3857)
                });
            }

            return result;
        }

        public static List<County> GetCounties93Wm()
        {
            var fileName = @"E:\Programming\100. IRI.Japey\IRI.Ket\IRI.Ket.Data\Data\shahrestan93Wm.txt";

            var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<prov>>(System.IO.File.ReadAllText(fileName));

            var result = new List<County>();

            foreach (var county in jsonObject)
            {
                result.Add(new County()
                {
                    Name = county.Shahrestan,
                    ProvinceName = county.Ostan,
                    BoundaryWm = DbGeometry.FromBinary(Convert.FromBase64String(county.Border), 3857)
                });
            }

            return result;
        }

        public static List<District> GetDistricts93Wm()
        {
            var fileName = @"E:\Programming\100. IRI.Japey\IRI.Ket\IRI.Ket.Data\Data\tagsimat93Wm.txt";

            var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<prov>>(System.IO.File.ReadAllText(fileName));

            var result = new List<District>();

            foreach (var district in jsonObject)
            {
                result.Add(new District()
                {
                    Name = district.Bakhsh,
                    ProvinceName = district.Ostan,
                    CountryName = district.Shahrestan,
                    BoundaryWm = DbGeometry.FromBinary(Convert.FromBase64String(district.Border), 3857)
                });
            }

            return result;
        }



        public class prov
        {
            public string Ostan { get; set; }

            public string Shahrestan { get; set; }

            public string Bakhsh { get; set; }

            public string Markaz { get; set; }

            public string Border { get; set; }

            [JsonIgnore]
            public SqlGeometry BorderGeo { get; set; }
        }
    }
}
