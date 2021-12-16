using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class BookKeeping
    {
        public int? IdNote { get; set; }
        public int RecoveryId { get; set; }
        public int PaymentId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
