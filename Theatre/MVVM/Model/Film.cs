using System;
using System.Collections.Generic;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class Film
    {
        public int? IdFilm { get; set; }
        public string NameFlim { get; set; }
        public DateTime Date { get; set; }
        public string DurationFilm { get; set; }
        public int GenreId { get; set; }
        public int RatingId { get; set; }
        public int StudioId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
