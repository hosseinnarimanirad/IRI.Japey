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
    public abstract class RuralDistrictBase
    {
        public int Id { get; set; }

        [Required]
        public string Name{ get; set; }

        public string CountryName{ get; set; }

        public string ProvinceName{ get; set; }

        public string DistrictName{ get; set; }

        [Required]
        public DbGeometry BoundaryWm { get; set; }
    }

    [Table("RuralDistrict", Schema = "Tagsimat")]
    public class RuralDistrict : RuralDistrictBase
    {
        public District District { get; set; }

        public Village Center { get; set; }

        public virtual List<Village> Villages { get; set; }
    }
}
