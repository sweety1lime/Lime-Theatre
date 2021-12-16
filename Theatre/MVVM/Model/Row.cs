using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class Row
    {


        public int? IdRow { get; set; }
        public string CategoryRow { get; set; }

        public bool IsDeleted { get; set; }

    }
}
