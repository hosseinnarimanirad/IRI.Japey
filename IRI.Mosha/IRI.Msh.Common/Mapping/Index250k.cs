using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Mapping
{
    public class Index250k : IndexBase
    {
        public override double Height { get => MapIndexes._250kHeight; }

        public override double Width { get => MapIndexes._250kWidth; }

    }
}
