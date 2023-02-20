using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.PersonalGdbPersistence.Model;

public class GdbItem
{ 
    public string Name { get; set; }

    public string PhysicalName { get; set; }

    public string Path { get; set; }

    public string? DatasetSubtype1 { get; set; }

    public string? DatasetSubtype2 { get; set;}

    public string? DatasetInfo1 { get; set; }

    public string Definition { get; set; }
}
