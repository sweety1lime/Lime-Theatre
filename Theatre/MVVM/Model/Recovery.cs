using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class Recovery
    {


        public int? IdRecovery { get; set; }
        public string NameRecovery { get; set; }
        public int SumRecovery { get; set; }
        public DateTime DateRecovery { get; set; }
        public int EmployeeId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
