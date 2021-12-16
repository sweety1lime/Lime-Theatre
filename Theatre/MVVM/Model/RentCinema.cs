using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class RentCinema
    {
        public int? IdRent { get; set; }
        public int Cost { get; set; }
        public string RentDuration { get; set; }
        public int FilmId { get; set; }
        public int EmployeeId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
