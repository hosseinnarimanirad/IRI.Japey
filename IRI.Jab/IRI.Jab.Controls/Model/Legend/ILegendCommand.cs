using IRI.Jab.Common.Assets.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Controls.Model.Legend
{
    public interface ILegendCommand
    {
        RelayCommand Command { get; set; }

        string PathMarkup { get; set; }
    }
}
