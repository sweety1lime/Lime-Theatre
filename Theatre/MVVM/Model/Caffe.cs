using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class Caffe
    {


        public int? IdCaffe { get; set; }
        public string Goods { get; set; }
        public int EmployeeId { get; set; }
        public bool IsDeleted { get; set; }

    }
}
