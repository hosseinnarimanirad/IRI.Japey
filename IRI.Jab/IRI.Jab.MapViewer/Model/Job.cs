﻿using IRI.Jab.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace IRI.Jab.MapViewer.Model
{
    public class Job
    {
        public LayerTag Tag { get; set; }

        public DispatcherOperation Operation { get; set; }

        public Job(LayerTag tag, DispatcherOperation operation)
        {
            this.Tag = tag;

            this.Operation = operation;
        }
    }
}
