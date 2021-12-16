using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class Rate
    {


        public int? IdRate { get; set; }
        public string NameRate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
