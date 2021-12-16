using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class Post
    {


        public int? IdPost { get; set; }
        public string NamePost { get; set; }
        public int Salary { get; set; }

        public bool IsDeleted { get; set; }

    }
}
