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
   public class CinemaHallViewModel : ObservableObject, ICRUD
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

        private CinemaHall _cinemaHall;

        public CinemaHall CinemaHall
        {
            get { return _cinemaHall; }
            set
            {

                if (value != null)
                {

                    if (ListTypeHall != null && ListTypeHall.Any())
                        TypeHall = ListTypeHall.FirstOrDefault(x => x.IdType == value.TypeId)??new TypeHall();
                }
                _cinemaHall = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CinemaHall> CinemaHalls;

        public ObservableCollection<CinemaHall> lists
        {
            get { return CinemaHalls; }
            set
            {
                CinemaHalls = value;
                OnPropertyChanged();
            }
        }

        private TypeHall _typeHall;

        public TypeHall TypeHall
        {
            get { return _typeHall; }
            set
            {
                _typeHall = value;


                CinemaHall.TypeId = value.IdType??CinemaHall.TypeId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TypeHall> _listTypeHall;

        public ObservableCollection<TypeHall> ListTypeHall
        {
            get { return _listTypeHall; }
            set
            {
                _listTypeHall = value;

                OnPropertyChanged();
            }
        }

        private ObservableCollection<CinemaHall> _deleteList;
        public ObservableCollection<CinemaHall> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private CinemaHall _deleted;
        public CinemaHall Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public CinemaHallViewModel()
        {
            InitAsync();
            ReadAsync();
        }
        private async void InitAsync()
        {
            CreateCommand = new RelayCommand(x => { CreateAsync(); });
            UpdateCommand = new RelayCommand(x => { UpdateAsync(); });
            DeleteCommand = new RelayCommand(x => { DeleteAsync(); });
            ListTypeHall = await Converter.Getter<TypeHall>("TypeHalls");
            LogicalDeleteCommand = new RelayCommand(x => { LogicalDelete(); });
            BackCommand = new RelayCommand(x => { Back(); });
            ExportCommand = new RelayCommand(x => { ExportTable(); });
        }

        public void Back()
        {
            CinemaHall.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            CinemaHall.IsDeleted = true;
            UpdateAsync();
        }
        public async void CreateAsync()
        {
            if (ValidationErrorMessage() is string message && !string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message);
                return;
            }
            if (CinemaHall != null)
            {
                CinemaHall.IdHall = null;
                var listemployee = await Converter.Creatter<CinemaHall>("CinemaHalls", CinemaHall);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdHall != null)
            {
                var deleted = await Converter.Deletter("CinemaHalls", Deleted.IdHall.Value);
                MessageBox.Show($"{Deleted.NameHall}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            CinemaHall = new CinemaHall();
            var fullTableList = await Converter.Getter<CinemaHall>("CinemaHalls");
            lists = new ObservableCollection<CinemaHall>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<CinemaHall>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (CinemaHall.IdHall != null)
            {
                await Converter.Updatter("CinemaHalls", CinemaHall, CinemaHall.IdHall.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdHall}, {item.NameHall},{item.CountSeat},{item.LeftSeat},{item.TypeId},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "CinemaHalls");
        }

        public string ValidationErrorMessage()
        {
            if (CinemaHall == null) return String.Empty;
            if (string.IsNullOrWhiteSpace(CinemaHall.NameHall)) return "Поле \"Название\" незаполнено";
            if (CinemaHall.CountSeat < 0) return "Поле \"Количество мест\" не должно быть отрицательным";
            if (CinemaHall.LeftSeat < 0) return "Поле \"Оставшиеся места\" не должно быть отрицательным";
            if (!ListTypeHall.Select(x => x.IdType).Contains(TypeHall.IdType)) return "Поле \"Тип зала\" не выбрано";
            return String.Empty;
        }
    }
}
