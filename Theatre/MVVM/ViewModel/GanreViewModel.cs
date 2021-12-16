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

namespace Theatre.MVVM.ViewModel
{
   public class GanreViewModel : ObservableObject, ICRUD
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

        private ObservableCollection<FilmGenre> _deleteList;
        public ObservableCollection<FilmGenre> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private FilmGenre _deleted;
        public FilmGenre Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }
        public GanreViewModel()
        {
            InitAsync();
            ReadAsync();
        }
        private void InitAsync()
        {
            CreateCommand = new RelayCommand(x => { CreateAsync(); });
            UpdateCommand = new RelayCommand(x => { UpdateAsync(); });
            DeleteCommand = new RelayCommand(x => { DeleteAsync(); });
            LogicalDeleteCommand = new RelayCommand(x => { LogicalDelete(); });
            BackCommand = new RelayCommand(x => { Back(); });
            ExportCommand = new RelayCommand(x => { ExportTable(); });
        }

        public void Back()
        {
            Genre.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Genre.IsDeleted = true;
            UpdateAsync();
        }
        private FilmGenre _genre;

        public FilmGenre Genre
        {
            get { return _genre; }
            set
            {
                _genre = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FilmGenre> filmGenres;

        public ObservableCollection<FilmGenre> lists
        {
            get { return filmGenres; }
            set
            {
                filmGenres = value;
                OnPropertyChanged();
            }
        }

        public async void CreateAsync()
        {
            if (ValidationErrorMessage() is string message && !string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message);
                return;
            }
            if (Genre != null)
            {
                Genre.IdGenre = null;
                var listemployee = await Converter.Creatter<FilmGenre>("FilmGenres", Genre);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdGenre != null)
            {
                var deleted = await Converter.Deletter("FilmGenres", Deleted.IdGenre.Value);
                MessageBox.Show($"{Deleted.NameGenre}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            Genre = new FilmGenre();

            var fullTableList = await Converter.Getter<FilmGenre>("FilmGenres");
            lists = new ObservableCollection<FilmGenre>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<FilmGenre>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Genre.IdGenre != null)
            {
                await Converter.Updatter("FilmGenres", Genre, Genre.IdGenre.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdGenre}, {item.NameGenre},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "FilmGenres");
        }

        public string ValidationErrorMessage()
        {
            if (Genre == null) return String.Empty;
            if (string.IsNullOrWhiteSpace(Genre.NameGenre)) return "Поле \"Название\" незаполнено";
     

            return String.Empty;
        }
    }
}
