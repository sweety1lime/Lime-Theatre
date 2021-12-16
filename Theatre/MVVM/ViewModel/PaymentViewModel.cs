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
   public class PaymentViewModel : ObservableObject, ICRUD
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

        private ObservableCollection<Payment> _deleteList;
        public ObservableCollection<Payment> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private Payment _deleted;
        public Payment Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        private Payment _payment;

        public Payment Payment
        {
            get { return _payment; }
            set
            {

                if (value != null)
                {

                    if (ListEmployee != null && ListEmployee.Any())
                        Employee = ListEmployee.FirstOrDefault(x => x.IdEmployee == value.EmployeeId)??new Employee();
                }
                _payment = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Payment> Payments;

        public ObservableCollection<Payment> lists
        {
            get { return Payments; }
            set
            {
                Payments = value;
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


                Payment.EmployeeId = value.IdEmployee??Payment.EmployeeId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Employee> _listEmployee;

        public ObservableCollection<Employee> ListEmployee
        {
            get { return _listEmployee; }
            set
            {
                _listEmployee = value;

                OnPropertyChanged();
            }
        }

        public PaymentViewModel()
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
            LogicalDeleteCommand = new RelayCommand(x => { LogicalDelete(); });
            BackCommand = new RelayCommand(x => { Back(); });
            ExportCommand = new RelayCommand(x => { ExportTable(); });
        }

        public void Back()
        {
            Payment.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Payment.IsDeleted = true;
            UpdateAsync();
        }
        public async void CreateAsync()
        {
            if (ValidationErrorMessage() is string message && !string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message);
                return;
            }
            if (Payment != null)
            {
                Payment.IdPayment = null;
                var listemployee = await Converter.Creatter<Payment>("Payments", Payment);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdPayment != null)
            {
                var deleted = await Converter.Deletter("Payments", Deleted.IdPayment.Value);
                MessageBox.Show($"{Deleted.IdPayment}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            Payment = new Payment();
            var fullTableList = await Converter.Getter<Payment>("Payments");
            lists = new ObservableCollection<Payment>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<Payment>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Payment.IdPayment != null)
            {
                await Converter.Updatter("Payments", Payment, Payment.IdPayment.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdPayment}, {item.DatePayment},{item.SumPayment},{item.EmployeeId},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Employees");
        }
        public string ValidationErrorMessage()
        {
            if (Payment == null) return String.Empty;
            
            if (!ListEmployee.Select(x => x.IdEmployee).Contains(Employee.IdEmployee)) return "Поле \"Работник\" не выбрано";
            if (Payment.DatePayment.Year < 2010) return "Минимальное значение поля \"Время оплаты\" - 2010 год";
            if (Payment.SumPayment < 0) return "Поле \"Зарплата\" не должно быть отрицательным";

            return String.Empty;
        }
    }
}
