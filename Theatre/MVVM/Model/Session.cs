using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class Session
    {
        public int? IdSession { get; set; }
        public DateTime DateTime { get; set; }
        public int HallId { get; set; }
        public int FilmId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
