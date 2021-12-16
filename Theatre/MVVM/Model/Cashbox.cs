using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class Cashbox
    {
  

        public int? IdCashBox { get; set; }
        public int Proceeds { get; set; }
        public int TicketId { get; set; }
        public int EmployeeId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
