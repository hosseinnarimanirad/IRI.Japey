using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Data.Model
{
    public abstract class VillageBase
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ProvinceName { get; set; }

        public string CountryName { get; set; }

        public string DistrictName { get; set; }

        public string RuralDistrictName { get; set; }

        /// <summary>
        /// Point Geometry Expected
        /// </summary>
        [Required]
        public DbGeometry LocationWm { get; set; }
    }

    public class Village : VillageBase
    {
        public RuralDistrict RuralDistrict { get; set; }

        public DbGeometry BoundaryWm { get; set; }
    }
}
