using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class FilmGenre
    {

        public int? IdGenre { get; set; }
        public string NameGenre { get; set; }

        public bool IsDeleted { get; set; }
    }
}
