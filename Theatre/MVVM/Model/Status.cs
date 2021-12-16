using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class Status
    {


        public int? IdStatus { get; set; }
        public string StatusName { get; set; }

        public bool IsDeleted { get; set; }
    }
}
