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
   public class SessionViewModel : ObservableObject, ICRUD
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
        public RelayCommand BackCommand { get; set; }
        public RelayCommand ExportCommand { get; set; }

        private Session _session;

        public Session Session
        {
            get { return _session; }
            set
            {
                if (value != null)
                {

                    if (ListFilm != null && ListFilm.Any())
                        Film = ListFilm.FirstOrDefault(x => x.IdFilm == value.FilmId)?? new Film();
                    if (ListHall != null && ListHall.Any())
                        CinemaHall = ListHall.FirstOrDefault(x => x.IdHall == value.HallId)?? new CinemaHall();
                }

                _session = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Session> Sessions;

        public ObservableCollection<Session> lists
        {
            get { return Sessions; }
            set
            {
                Sessions = value;
                OnPropertyChanged();
            }
        }

        private CinemaHall _hall;

        public CinemaHall CinemaHall
        {
            get { return _hall; }
            set
            {
                _hall = value;


                Session.HallId = value.IdHall??Session.HallId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<CinemaHall> _listHall;

        public ObservableCollection<CinemaHall> ListHall
        {
            get { return _listHall; }
            set
            {
                _listHall = value;

                OnPropertyChanged();
            }
        }

        private Film _film;

        public Film Film
        {
            get { return _film; }
            set
            {
                _film = value;


                Session.FilmId = value.IdFilm??Session.FilmId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Film> _listFilm;

        public ObservableCollection<Film> ListFilm
        {
            get { return _listFilm; }
            set
            {
                _listFilm = value;

                OnPropertyChanged();
            }
        }

        public RelayCommand LogicalDeleteCommand { get; set; }

        private ObservableCollection<Session> _deleteList;
        public ObservableCollection<Session> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private Session _deleted;
        public Session Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public SessionViewModel()
        {
            InitAsync();
            ReadAsync();
        }
        private async void InitAsync()
        {
            CreateCommand = new RelayCommand(x => { CreateAsync(); });
            UpdateCommand = new RelayCommand(x => { UpdateAsync(); });
            DeleteCommand = new RelayCommand(x => { DeleteAsync(); });
            ListHall = await Converter.Getter<CinemaHall>("CinemaHalls");
            ListFilm = await Converter.Getter<Film>("Films");
            LogicalDeleteCommand = new RelayCommand(x => { LogicalDelete(); });
            BackCommand = new RelayCommand(x => { Back(); });
            ExportCommand = new RelayCommand(x => { ExportTable(); });
        }

        public void Back()
        {
            Session.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Session.IsDeleted = true;
            UpdateAsync();
        }

        public async void CreateAsync()
        {
            if (ValidationErrorMessage() is string message && !string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message);
                return;
            }
            if (Session != null)
            {
                Session.IdSession = null;
                var listemployee = await Converter.Creatter<Session>("Sessions", Session);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdSession != null)
            {
                var deleted = await Converter.Deletter("Sessions", Deleted.IdSession.Value);
                MessageBox.Show($"{Deleted.IdSession}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            Session = new Session();
            var fullTableList = await Converter.Getter<Session>("Sessions");
            lists = new ObservableCollection<Session>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<Session>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Session.IdSession != null)
            {
                await Converter.Updatter("Sessions", Session, Session.IdSession.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdSession}, {item.DateTime},{item.FilmId},{item.HallId},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Sessions");
        }

        public string ValidationErrorMessage()
        {
            if (Session == null) return String.Empty;

            if (Session.DateTime.Year < 2010) return "Минимальное значение поля \"Время Сеанса\" - 2010 год";
            if (!ListFilm.Select(x => x.IdFilm).Contains(Film.IdFilm)) return "Поле \"Фильм\" не выбрано";
            if (!ListHall.Select(x => x.IdHall).Contains(CinemaHall.IdHall)) return "Поле \"Зал\" не выбрано";

            return String.Empty;
        }
    }
}
