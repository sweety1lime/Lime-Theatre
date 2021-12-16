using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class Ticket
    {


        public int? IdTicket { get; set; }
        public DateTime Date { get; set; }
        public int StatusId { get; set; }
        public int RowId { get; set; }
        public int SeatId { get; set; }
        public int HallId { get; set; }
        public int RateId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
