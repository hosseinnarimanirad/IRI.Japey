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
    public abstract class CountyBase
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ProvinceName { get; set; }

        [Required]
        public DbGeometry BoundaryWm { get; set; }
    }

    [Table("County", Schema = "Tagsimat")]
    public class County : CountyBase
    {
        public Province Province { get; set; }

        public City Center { get; set; }

        public virtual List<District> Districts { get; set; }
         
    }
}
