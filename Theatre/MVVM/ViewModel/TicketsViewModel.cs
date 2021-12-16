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
   public class TicketsViewModel : ObservableObject, ICRUD
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

        private ObservableCollection<Ticket> _deleteList;
        public ObservableCollection<Ticket> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private Ticket _deleted;
        public Ticket Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        private Ticket ticket;

        public Ticket Ticket
        {
            get { return ticket; }
            set
            {

                if (value != null)
                {

                    if (ListHall != null && ListHall.Any())
                        CinemaHall = ListHall.FirstOrDefault(x => x.IdHall == value.HallId)?? new CinemaHall();
                    if (ListRate != null && ListRate.Any())
                        Rate = ListRate.FirstOrDefault(x => x.IdRate == value.RateId)?? new Rate();
                    if (ListRow != null && ListRow.Any())
                        Row = ListRow.FirstOrDefault(x => x.IdRow == value.RowId)?? new Row();
                    if (ListSeat != null && ListSeat.Any())
                        Seat = ListSeat.FirstOrDefault(x => x.IdSeat == value.SeatId)?? new Seat();
                    if (ListStatus != null && ListStatus.Any())
                        Status = ListStatus.FirstOrDefault(x => x.IdStatus == value.StatusId)?? new Status();
                }
                ticket = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Ticket> Tickets;

        public ObservableCollection<Ticket> lists
        {
            get { return Tickets; }
            set
            {
                Tickets = value;
                OnPropertyChanged();
            }
        }

        private Rate _rate;

        public Rate Rate
        {
            get { return _rate; }
            set
            {
                _rate = value;


                Ticket.RateId = value.IdRate??Ticket.RateId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Rate> _listRate;

        public ObservableCollection<Rate> ListRate
        {
            get { return _listRate; }
            set
            {
                _listRate = value;

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


                Ticket.HallId = value.IdHall??Ticket.HallId;
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

        private Row _row;

        public Row Row
        {
            get { return _row; }
            set
            {
                _row = value;


                Ticket.RowId = value.IdRow??Ticket.RowId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Row> _listRow;

        public ObservableCollection<Row> ListRow
        {
            get { return _listRow; }
            set
            {
                _listRow = value;

                OnPropertyChanged();
            }
        }

        private Seat _seat;

        public Seat Seat
        {
            get { return _seat; }
            set
            {
                _seat = value;


                Ticket.SeatId = value.IdSeat??Ticket.SeatId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Seat> _listSeat;

        public ObservableCollection<Seat> ListSeat
        {
            get { return _listSeat; }
            set
            {
                _listSeat = value;

                OnPropertyChanged();
            }
        }

        private Status _status;

        public Status Status
        {
            get { return _status; }
            set
            {
                _status = value;


                Ticket.StatusId = value.IdStatus??Ticket.StatusId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Status> _listStatus;

        public ObservableCollection<Status> ListStatus
        {
            get { return _listStatus; }
            set
            {
                _listStatus = value;

                OnPropertyChanged();
            }
        }
        public TicketsViewModel()
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
            ListRate = await Converter.Getter<Rate>("Rates");
            ListRow = await Converter.Getter<Row>("Rows");
            ListSeat = await Converter.Getter<Seat>("Seats");
            ListStatus = await Converter.Getter<Status>("Status");
            LogicalDeleteCommand = new RelayCommand(x => { LogicalDelete(); });
            BackCommand = new RelayCommand(x => { Back(); });
            ExportCommand = new RelayCommand(x => { ExportTable(); });
        }

        public void Back()
        {
            Ticket.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Ticket.IsDeleted = true;
            UpdateAsync();
        }
        public async void CreateAsync()
        {
            if (ValidationErrorMessage() is string message && !string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message);
                return;
            }
            if (Ticket != null)
            {
                Ticket.IdTicket = null;
                var listemployee = await Converter.Creatter<Ticket>("Tickets", Ticket);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdTicket != null)
            {
                var deleted = await Converter.Deletter("Tickets", Deleted.IdTicket.Value);
                MessageBox.Show($"{Deleted.IdTicket}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            Ticket = new Ticket();
            var fullTableList = await Converter.Getter<Ticket>("Tickets");
            lists = new ObservableCollection<Ticket>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<Ticket>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Ticket.IdTicket != null)
            {
                await Converter.Updatter("Tickets", Ticket, Ticket.IdTicket.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdTicket}, {item.Date},{item.HallId},{item.RateId},{item.RowId},{item.SeatId},{item.StatusId},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Tickets");
        }

        public string ValidationErrorMessage()
        {
            if (Ticket == null) return String.Empty;
            
            if (!ListHall.Select(x => x.IdHall).Contains(CinemaHall.IdHall)) return "Поле \"Зал\" не выбрано";
            if (!ListRate.Select(x => x.IdRate).Contains(Rate.IdRate)) return "Поле \"Тариф\" не выбрано";
            if (!ListRow.Select(x => x.IdRow).Contains(Row.IdRow)) return "Поле \"Ряд\" не выбрано";
            if (!ListSeat.Select(x => x.IdSeat).Contains(Seat.IdSeat)) return "Поле \"Место\" не выбрано";
            if (!ListStatus.Select(x => x.IdStatus).Contains(Status.IdStatus)) return "Поле \"Статус\" не выбрано";

            if (Ticket.Date.Year < 2010) return "Минимальное значение поля \"Время выдачи\" - 2010 год";

            return String.Empty;
        }

    }
}
