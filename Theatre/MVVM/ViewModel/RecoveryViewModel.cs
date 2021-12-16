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
   public class RecoveryViewModel : ObservableObject, ICRUD
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

        private Recovery _recovery;

        public Recovery Recovery
        {
            get { return _recovery; }
            set
            {
                if (value != null)
                {

                    if (ListEmployee != null && ListEmployee.Any())
                        Employee = ListEmployee.FirstOrDefault(x => x.IdEmployee == value.EmployeeId)?? new Employee();
                }
                _recovery = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Recovery> Recoveryes;

        public ObservableCollection<Recovery> lists
        {
            get { return Recoveryes; }
            set
            {
                Recoveryes = value;
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


                Recovery.EmployeeId = value.IdEmployee??Recovery.EmployeeId;
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

        public RecoveryViewModel()
        {
            InitAsync();
            ReadAsync();
        }

        public RelayCommand LogicalDeleteCommand { get; set; }

        private ObservableCollection<Recovery> _deleteList;
        public ObservableCollection<Recovery> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private Recovery _deleted;
        public Recovery Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        private async void InitAsync()
        {
            CreateCommand = new RelayCommand(x => { CreateAsync(); });
            UpdateCommand = new RelayCommand(x => { UpdateAsync(); });
            DeleteCommand = new RelayCommand(x => { DeleteAsync(); });
            ListEmployee = await Converter.Getter<Employee>("Employees"); 
            LogicalDeleteCommand = new RelayCommand(x => { LogicalDelete(); });
            BackCommand = new RelayCommand(x => { Back(); });
        }

        public void Back()
        {
            Recovery.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Recovery.IsDeleted = true;
            UpdateAsync();
        }
        public async void CreateAsync()
        {
            if (ValidationErrorMessage() is string message && !string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message);
                return;
            }
            if (Recovery != null)
            {
                Recovery.IdRecovery = null;
                var listemployee = await Converter.Creatter<Recovery>("Recoveries", Recovery);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdRecovery != null)
            {
                var deleted = await Converter.Deletter("Recoveries", Deleted.IdRecovery.Value);
                MessageBox.Show($"{Deleted.IdRecovery}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            Recovery = new Recovery();
            var fullTableList = await Converter.Getter<Recovery>("Recoveries");
            lists = new ObservableCollection<Recovery>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<Recovery>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Recovery.IdRecovery != null)
            {
                await Converter.Updatter("Recoveries", Recovery, Recovery.IdRecovery.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdRecovery}, {item.NameRecovery},{item.DateRecovery},{item.SumRecovery},{item.EmployeeId},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Recoveries");
        }

        public string ValidationErrorMessage()
        {
            if (Recovery == null) return String.Empty;

            if (string.IsNullOrWhiteSpace(Recovery.NameRecovery)) return "Поле \"Причина взыскания\" незаполнено";
            if (!ListEmployee.Select(x => x.IdEmployee).Contains(Employee.IdEmployee)) return "Поле \"Работник\" не выбрано";
            if (Recovery.DateRecovery.Year < 2010) return "Минимальное значение поля \"Время взыскания\" - 2010 год";
            if (Recovery.SumRecovery < 0) return "Поле \"Сумма взыскания\" не должно быть отрицательным";

            return String.Empty;
        }
    }
}
