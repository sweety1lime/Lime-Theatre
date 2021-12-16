using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class CinemaHall
    {

        public int? IdHall { get; set; }
        public string NameHall { get; set; }
        public int LeftSeat { get; set; }
        public int CountSeat { get; set; }
        public int TypeId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
