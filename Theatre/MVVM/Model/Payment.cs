using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class Payment
    {


        public int? IdPayment { get; set; }
        public int SumPayment { get; set; }
        public DateTime DatePayment { get; set; }
        public int EmployeeId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
