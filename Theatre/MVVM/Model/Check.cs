using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class Check
    {
        public int? IdCheck { get; set; }
        public DateTime DatePayment { get; set; }
        public int Amount { get; set; }
        public int CountGoods { get; set; }
        public int TypePaymentId { get; set; }
        public int CashBoxId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
