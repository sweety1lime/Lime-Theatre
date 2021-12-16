using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class Studio
    {


        public int? IdStudio { get; set; }
        public string NameStudio { get; set; }

        public bool IsDeleted { get; set; }
    }
}
