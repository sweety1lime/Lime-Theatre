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
   public class RentCinemaViewModel : ObservableObject, ICRUD
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

        private RentCinema _rent;

        public RentCinema RentCinema
        {
            get { return _rent; }
            set
            {
                if (value != null)
                {

                    if (ListFilm != null && ListFilm.Any())
                        Film = ListFilm.FirstOrDefault(x => x.IdFilm == value.FilmId)?? new Film();

                    if (ListEmployee != null && ListEmployee.Any())
                        Employee = ListEmployee.FirstOrDefault(x => x.IdEmployee == value.EmployeeId) ?? new Employee();
                }

                _rent = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RentCinema> Rents;

        public ObservableCollection<RentCinema> lists
        {
            get { return Rents; }
            set
            {
                Rents = value;
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


                RentCinema.FilmId = value.IdFilm??RentCinema.FilmId;
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

        private Employee _employee;

        public Employee Employee
        {
            get { return _employee; }
            set
            {
                _employee = value;


                RentCinema.EmployeeId = value.IdEmployee??RentCinema.EmployeeId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Employee> _listemployee;

        public ObservableCollection<Employee> ListEmployee
        {
            get { return _listemployee; }
            set
            {
                _listemployee = value;

                OnPropertyChanged();
            }
        }

        public RelayCommand LogicalDeleteCommand { get; set; }

        private ObservableCollection<RentCinema> _deleteList;
        public ObservableCollection<RentCinema> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private RentCinema _deleted;
        public RentCinema Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public RentCinemaViewModel()
        {
            InitAsync();
            ReadAsync();
        }
        private async void InitAsync()
        {
            CreateCommand = new RelayCommand(x => { CreateAsync(); });
            UpdateCommand = new RelayCommand(x => { UpdateAsync(); });
            DeleteCommand = new RelayCommand(x => { DeleteAsync(); });
            ListFilm = await Converter.Getter<Film>("Films");
            ListEmployee = await Converter.Getter<Employee>("Employees");
            LogicalDeleteCommand = new RelayCommand(x => { LogicalDelete(); });
            BackCommand = new RelayCommand(x => { Back(); });
            ExportCommand = new RelayCommand(x => { ExportTable(); });
        }

        public void Back()
        {
            RentCinema.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            RentCinema.IsDeleted = true;
            UpdateAsync();
        }
        public async void CreateAsync()
        {
            if (ValidationErrorMessage() is string message && !string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message);
                return;
            }
            if (RentCinema != null)
            {
                RentCinema.IdRent = null;
                var listemployee = await Converter.Creatter<RentCinema>("RentCinemas", RentCinema);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdRent != null)
            {
                var deleted = await Converter.Deletter("RentCinemas", Deleted.IdRent.Value);
                MessageBox.Show($"{Deleted.IdRent}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            RentCinema = new RentCinema();
            var fullTableList = await Converter.Getter<RentCinema>("RentCinemas");
            lists = new ObservableCollection<RentCinema>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<RentCinema>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (RentCinema.IdRent != null)
            {
                await Converter.Updatter("RentCinemas", RentCinema, RentCinema.IdRent.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdRent}, {item.Cost},{item.RentDuration},{item.FilmId},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Employees");
        }

        public string ValidationErrorMessage()
        {
            if (RentCinema == null) return String.Empty;
            if (string.IsNullOrWhiteSpace(RentCinema.RentDuration)) return "Поле \"Длительность\" незаполнено";
            if (RentCinema.Cost < 0) return "Поле \"Цена аренды\" не должно быть отрицательным";
            if (!ListEmployee.Select(x => x.IdEmployee).Contains(Employee.IdEmployee)) return "Поле \"Сотрудник\" не выбрано";
            if (!ListFilm.Select(x => x.IdFilm).Contains(Film.IdFilm)) return "Поле \"Фильм\" не выбрано";

            return String.Empty;
        }
    }
}
