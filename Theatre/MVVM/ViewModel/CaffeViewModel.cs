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
  public  class CaffeViewModel : ObservableObject, ICRUD
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

        private ObservableCollection<Caffe> Caffes;

        private Caffe _caffe;

        public Caffe Caffe
        {
            get { return _caffe; }
            set
            {

                if (value != null)
                {

                    if (ListEmployee != null && ListEmployee.Any())
                        Employee = ListEmployee.FirstOrDefault(x => x.IdEmployee == value.EmployeeId)??new Employee();
                }
                _caffe = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Caffe> lists
        {
            get { return Caffes; }
            set
            {
                Caffes = value;
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


                Caffe.EmployeeId = value.IdEmployee??Caffe.EmployeeId;
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

        private ObservableCollection<Caffe> _deleteList;
        public ObservableCollection<Caffe> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private Caffe _deleted;
        public Caffe Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public CaffeViewModel()
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
            BackCommand = new RelayCommand(x => { Back(); });
            LogicalDeleteCommand = new RelayCommand(x => { LogicalDelete(); });
            ExportCommand = new RelayCommand(x => { ExportTable(); });
        }
        public void Back()
        {
            Caffe.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Caffe.IsDeleted = true;
            UpdateAsync();
        }
        public async void CreateAsync()
        {
            if (ValidationErrorMessage() is string message && !string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message);
                return;
            }
            if (Caffe != null)
            {
                Caffe.IdCaffe = null;
                var listemployee = await Converter.Creatter<Caffe>("Caffes", Caffe);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdCaffe != null)
            {
                var deleted = await Converter.Deletter("Caffes", Deleted.IdCaffe.Value);
                MessageBox.Show($"{Deleted.IdCaffe}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            Caffe = new Caffe();
            var fullTableList = await Converter.Getter<Caffe>("Caffes");
            lists = new ObservableCollection<Caffe>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<Caffe>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Caffe.IdCaffe != null)
            {
                await Converter.Updatter("Caffes", Caffe, Caffe.IdCaffe.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdCaffe}, {item.Goods},{item.EmployeeId},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Caffes");
        }

        public string ValidationErrorMessage()
        {
            if (Caffe == null) return String.Empty;
            if (string.IsNullOrWhiteSpace(Caffe.Goods)) return "Поле \"Товар\" незаполнено";
            if (!ListEmployee.Select(x => x.IdEmployee).Contains(Employee.IdEmployee)) return "Поле \"Сотрудник\" не выбрано";

            return String.Empty;
        }
    }
}
