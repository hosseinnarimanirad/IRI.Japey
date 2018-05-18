using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Data.Model
{
    public abstract class DistrictBase
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ProvinceName { get; set; }

        public string CountryName { get; set; }

        public DbGeometry BoundaryWm { get; set; }
    }

    public class District : DistrictBase
    {
        //public County County { get; set; }
   

        public City Center { get; set; }

        public virtual List<RuralDistrict> RuralDistricts { get; set; }
         
    }
}
