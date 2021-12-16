using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class Seat
    {


        public int? IdSeat { get; set; }
        public string CategorySeat { get; set; }

        public bool IsDeleted { get; set; }

    }
}
