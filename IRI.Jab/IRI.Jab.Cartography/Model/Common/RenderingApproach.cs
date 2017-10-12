using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Cartography
{
    public enum RenderingApproach
    { 
        //Render the entire extent at once
        Default,
        //Tile the extent and render each tile separatedly
        Tiled
    }
}
