using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Theatre.Core;
using Theatre.DBcontext;
using Theatre.MVVM.Model;
using Theatre.MVVM.View;

namespace Theatre.MVVM.ViewModel
{
   public class FilmViewModel : ObservableObject, ICRUD
    {
        public RelayCommand CreateCommand
        {
            get;
            set;
        }

        public RelayCommand UpdateCommand
        {
            get;
            set;
        }

        public RelayCommand DeleteCommand
        {
            get;
            set;
        }
        public RelayCommand LogicalDeleteCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand ExportCommand { get; set; }
        public RelayCommand DiagrammCommand { get; set; }

        private Film _film;

        public Film Film
        {
            get { return _film; }
            set
            {
                if (value != null)
                {

                    if (ListGenres != null && ListGenres.Any())
                        FilmGenre = ListGenres.FirstOrDefault(x => x.IdGenre == value.GenreId)?? new FilmGenre();
                    if (ListRating != null && ListRating.Any())
                        AgeRating = ListRating.FirstOrDefault(x => x.IdRating == value.RatingId)?? new AgeRating();
                    if (ListStudio != null && ListStudio.Any())
                        Studio = ListStudio.FirstOrDefault(x => x.IdStudio == value.StudioId)??new Studio();
                }

                _film = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Film> Films;

        public ObservableCollection<Film> lists
        {
            get { return Films; }
            set
            {
                Films = value;
                OnPropertyChanged();
            }
        }

        private Studio _studio;

        public Studio Studio
        {
            get { return _studio; }
            set
            {
                _studio = value;


                Film.StudioId = value.IdStudio??Film.StudioId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Studio> _liststudio;

        public ObservableCollection<Studio> ListStudio
        {
            get { return _liststudio; }
            set
            {
                _liststudio = value;

                OnPropertyChanged();
            }
        }


        private FilmGenre _genre;

        public FilmGenre FilmGenre
        {
            get { return _genre; }
            set
            {
                _genre = value;


                Film.GenreId = value.IdGenre??Film.GenreId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<FilmGenre> _listFilmGenre;

        public ObservableCollection<FilmGenre> ListGenres
        {
            get { return _listFilmGenre; }
            set
            {
                _listFilmGenre = value;

                OnPropertyChanged();
            }
        }

        private ObservableCollection<Film> _deleteList;
        public ObservableCollection<Film> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private Film _deleted;
        public Film Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public FilmViewModel()
        {
            InitAsync();
            ReadAsync();
        }

        public AgeRating _ageRating;

        public AgeRating AgeRating
        {
            get { return _ageRating; }
            set
            {
                _ageRating = value;


                Film.RatingId = value.IdRating??Film.RatingId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AgeRating> _listRating;

        public ObservableCollection<AgeRating> ListRating
        {
            get { return _listRating; }
            set
            {
                _listRating = value;

                OnPropertyChanged();
            }
        }
        private async void InitAsync()
        {
            CreateCommand = new RelayCommand(x => { CreateAsync(); });
            UpdateCommand = new RelayCommand(x => { UpdateAsync(); });
            DeleteCommand = new RelayCommand(x => { DeleteAsync(); });
            ListStudio = await Converter.Getter<Studio>("Studios");
            ListGenres = await Converter.Getter<FilmGenre>("FilmGenres");
            ListRating = await Converter.Getter<AgeRating>("AgeRatings");
            LogicalDeleteCommand = new RelayCommand(x => { LogicalDelete(); });
            BackCommand = new RelayCommand(x => { Back(); });
            ExportCommand = new RelayCommand(x => { ExportTable(); });
            DiagrammCommand = new RelayCommand(o => { ShowDiagramm(); });
        }

        public void Back()
        {
            Film.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Film.IsDeleted = true;
            UpdateAsync();
        }
        public async void CreateAsync()
        {
            if (ValidationErrorMessage() is string message && !string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message);
                return;
            }
            if (Film != null)
            {
                Film.IdFilm = null;
                var listemployee = await Converter.Creatter<Film>("Films", Film);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        private void ShowDiagramm()
        {
            DiagramWindow diagramm = new DiagramWindow();
            diagramm.ShowDialog();
        }
        public async void DeleteAsync()
        {
            if (Deleted.IdFilm != null)
            {
                var deleted = await Converter.Deletter("Films", Deleted.IdFilm.Value);
                MessageBox.Show($"{Deleted.NameFlim}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            Film = new Film();
            var fullTableList = await Converter.Getter<Film>("Films");
            lists = new ObservableCollection<Film>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<Film>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Film.IdFilm != null)
            {
                await Converter.Updatter("Films", Film, Film.IdFilm.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdFilm}, {item.NameFlim},{item.Date},{item.DurationFilm},{item.RatingId},{item.GenreId},{item.StudioId},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Films");
        }

        public string ValidationErrorMessage()
        {
            if (Film == null) return String.Empty;
            if (string.IsNullOrWhiteSpace(Film.NameFlim)) return "Поле \"Название\" незаполнено";
            if (string.IsNullOrWhiteSpace(Film.DurationFilm)) return "Поле \"Длительность\" незаполнено";
            if (!ListGenres.Select(x => x.IdGenre).Contains(FilmGenre.IdGenre)) return "Поле \"Жанр\" не выбрано";
            if (!ListRating.Select(x => x.IdRating).Contains(AgeRating.IdRating)) return "Поле \"Ограничение\" не выбрано";
            if (!ListStudio.Select(x => x.IdStudio).Contains(Studio.IdStudio)) return "Поле \"Студия\" не выбрано";
            if (Film.Date.Year < 1910) return "Минимальное значение поля \"Выпуск фильма\" - 1910 год";
            return String.Empty;
        }
    }
}
