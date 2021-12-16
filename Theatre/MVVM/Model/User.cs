using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class User
    {


        public int? IdUser { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int PostId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
