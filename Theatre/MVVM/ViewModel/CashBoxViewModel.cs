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
    public class CashBoxViewModel : ObservableObject, ICRUD
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

        public RelayCommand LogicalDeleteCommand { get; set; }
        public RelayCommand ExportCommand { get; set; }

        private ObservableCollection<Cashbox> Cashboxes;

        private Cashbox _cashbox;

        public Cashbox Cashbox
        {
            get { return _cashbox; }
            set
            {

                if (value != null)
                {

                    if (ListEmployee != null && ListEmployee.Any())
                        Employee = ListEmployee.FirstOrDefault(x => x.IdEmployee == value.EmployeeId)??new Employee();
                    if (ListTicket != null && ListTicket.Any())
                        Ticket = ListTicket.FirstOrDefault(x => x.IdTicket == value.TicketId)?? new Ticket();
                }
                _cashbox = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Cashbox> lists
        {
            get { return Cashboxes; }
            set
            {
                Cashboxes = value;
                OnPropertyChanged();
            }
        }

        private Ticket _ticket;

        public Ticket Ticket
        {
            get { return _ticket; }
            set
            {
                _ticket = value;


                Cashbox.TicketId = value.IdTicket??Cashbox.TicketId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Ticket> _listTicket;

        public ObservableCollection<Ticket> ListTicket
        {
            get { return _listTicket; }
            set
            {
                _listTicket = value;

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


                Cashbox.EmployeeId = value.IdEmployee??Cashbox.EmployeeId;
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

        private ObservableCollection<Cashbox> _deleteList;
        public ObservableCollection<Cashbox> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private Cashbox _deleted;
        public Cashbox Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public CashBoxViewModel()
        {
            InitAsync();
            ReadAsync();
        }
        private async void InitAsync()
        {
            CreateCommand = new RelayCommand(x => { CreateAsync(); });
            UpdateCommand = new RelayCommand(x => { UpdateAsync(); });
            DeleteCommand = new RelayCommand(x => { DeleteAsync(); });
            ListEmployee = await Converter.Getter<Employee>("Employees");
            ListTicket = await Converter.Getter<Ticket>("Tickets");
            BackCommand = new RelayCommand(x => { Back(); });
            ExportCommand = new RelayCommand(x => { ExportTable(); });
            LogicalDeleteCommand = new RelayCommand(x => { LogicalDelete(); });
        }
        public void Back()
        {
            Cashbox.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Cashbox.IsDeleted = true;
            UpdateAsync();
        }
        public async void CreateAsync()
        {
            if (ValidationErrorMessage() is string message && !string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message);
                return;
            }
            if (Cashbox != null)
            {
                Cashbox.IdCashBox = null;
                var listemployee = await Converter.Creatter<Cashbox>("Cashboxes", Cashbox);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdCashBox != null)
            {
                var deleted = await Converter.Deletter("Cashboxes", Deleted.IdCashBox.Value);
                MessageBox.Show($"{Deleted.IdCashBox}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            Cashbox = new Cashbox();
            var fullTableList = await Converter.Getter<Cashbox>("Cashboxes");
            lists = new ObservableCollection<Cashbox>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<Cashbox>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Cashbox.IdCashBox != null)
            {
                await Converter.Updatter("Cashboxes", Cashbox, Cashbox.IdCashBox.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdCashBox}, {item.Proceeds},{item.TicketId},{item.EmployeeId},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Caffes");
        }
        public string ValidationErrorMessage()
        {
            if (Cashbox == null) return String.Empty;
            if (Cashbox.Proceeds < 0) return "Поле \"Выручка\" не должно быть отрицательным";
            if (!ListEmployee.Select(x => x.IdEmployee).Contains(Employee.IdEmployee)) return "Поле \"Сотрудник\" не выбрано";
            if (!ListTicket.Select(x => x.IdTicket).Contains(Ticket.IdTicket)) return "Поле \"Номер билета\" не выбрано";
            return String.Empty;
        }
    }
}
