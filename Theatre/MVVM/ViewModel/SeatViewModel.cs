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
   public class SeatViewModel : ObservableObject, ICRUD
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

        private ObservableCollection<Seat> _deleteList;
        public ObservableCollection<Seat> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private Seat _deleted;
        public Seat Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public SeatViewModel()
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
            Seat.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Seat.IsDeleted = true;
            UpdateAsync();
        }

        private Seat _seat;

        public Seat Seat
        {
            get { return _seat; }
            set
            {
                _seat = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Seat> Seats;

        public ObservableCollection<Seat> lists
        {
            get { return Seats; }
            set
            {
                Seats = value;
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
            if (Seat != null)
            {
                Seat.IdSeat = null;
                var listemployee = await Converter.Creatter<Seat>("Seats", Seat);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdSeat != null)
            {
                var deleted = await Converter.Deletter("Seats", Deleted.IdSeat.Value);
                MessageBox.Show($"{Deleted.CategorySeat}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            Seat = new Seat();
            var fullTableList = await Converter.Getter<Seat>("Seats");
            lists = new ObservableCollection<Seat>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<Seat>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Seat.IdSeat != null)
            {
                await Converter.Updatter("Seats", Seat, Seat.IdSeat.Value);
                ReadAsync();
            }
        }


        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdSeat}, {item.CategorySeat},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Seats");
        }

        public string ValidationErrorMessage()
        {
            if (Seat == null) return String.Empty;
            if (string.IsNullOrWhiteSpace(Seat.CategorySeat)) return "Поле \"Категория и место\" незаполнено";
           
            return String.Empty;
        }
    }
}
