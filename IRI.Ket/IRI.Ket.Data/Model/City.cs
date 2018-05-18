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
    public abstract class CityBase
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Point Geometry Expected
        /// </summary>
        [Required]
        public DbGeometry LocationWm { get; set; }
    }

    public class City : CityBase
    {

        /// <summary>
        /// Polygon Geometry Expected
        /// </summary>
        public DbGeometry BoundaryWm { get; set; }
         
         
    }
}
