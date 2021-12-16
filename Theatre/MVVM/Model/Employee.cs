using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class Employee
    {

        public int? IdEmployee { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
