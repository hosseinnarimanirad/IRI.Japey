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
    public abstract class ProvinceBase
    {
        public int Id { get; set; }

        [Required]
        public string Name{ get; set; }

        [Required]
        public DbGeometry BoundaryWm { get; set; }
    }

    [Table("Province", Schema = "Tagsimat")]
    public class Province : ProvinceBase
    {
        public City Center { get; set; }

        public virtual List<County> Counties { get; set; }
         
    }
}
