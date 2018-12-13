﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.MultiSelectItem.ViewModel
{
    public static class MultiSelectItemViewModelFactory
    {

        public static MultiSelectItemViewModel<string> Create(bool isRequired)
        {
            return new MultiSelectItemViewModel<string>(Enumerable.Empty<string>(), i => i, i => i) { IsRequired = isRequired };
        }

    }
}
