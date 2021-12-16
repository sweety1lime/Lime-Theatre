using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theatre.Core;
using Theatre.MVVM.Model;
using Theatre.DBcontext;
using System.Collections.ObjectModel;
using System.Windows;

namespace Theatre.MVVM.ViewModel
{
   public class EmployeeViewModel : ObservableObject, ICRUD
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

        private ObservableCollection<Employee> employees;

        private Employee _employee;

        public Employee Employee
        {
            get { return _employee; }
            set
            {

                if (value != null)
                {

                    if (ListPost != null && ListPost.Any())
                        Post = ListPost.FirstOrDefault(x => x.IdPost == value.PostId)?? new Post();
                    if (ListUser != null && ListUser.Any())
                        User = ListUser.FirstOrDefault(x => x.IdUser == value.UserId) ?? new User();
                }
                _employee = value;
                OnPropertyChanged();
            }
        }


        private User _user;

        public User User
        {
            get { return _user; }
            set
            {
                _user = value;


                Employee.UserId = value.IdUser??Employee.UserId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<User> _listuser;

        public ObservableCollection<User> ListUser
        {
            get { return _listuser; }
            set
            {
                _listuser = value;

                OnPropertyChanged();
            }
        }

        private Post _post;

        public Post Post
        {
            get { return _post; }
            set
            {
                _post = value;


                Employee.PostId = value.IdPost??Employee.PostId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Post> _listpost;

        public ObservableCollection<Post> ListPost
        {
            get { return _listpost; }
            set
            {
                _listpost = value;

                OnPropertyChanged();
            }
        }

        public ObservableCollection<Employee> lists
        {
            get { return employees; }
            set
            {
                employees = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Employee> _deleteList;
        public ObservableCollection<Employee> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private Employee _deleted;
        public Employee Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public EmployeeViewModel()
        {
            InitAsync();
            ReadAsync();
        }
        private async void InitAsync()
        {
            CreateCommand = new RelayCommand(x=> { CreateAsync(); });
            UpdateCommand = new RelayCommand(x=> { UpdateAsync(); });
            DeleteCommand = new RelayCommand(x=> { DeleteAsync(); });
            ListPost = await Converter.Getter<Post>("Posts");
            ListUser = await Converter.Getter<User>("Users");
            LogicalDeleteCommand = new RelayCommand(x => { LogicalDelete(); });
            BackCommand = new RelayCommand(x => { Back(); });
            ExportCommand = new RelayCommand(x => { ExportTable(); });
        }

        public void Back()
        {
            Employee.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Employee.IsDeleted = true;
            UpdateAsync();
        }
        public async void CreateAsync()
        {
            if (ValidationErrorMessage() is string message && !string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message);
                return;
            }
            if (Employee != null)
            {
                Employee.IdEmployee = null;
                var listemployee = await Converter.Creatter<Employee>("Employees", Employee);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdEmployee != null)
            {
                var deleted = await Converter.Deletter("Employees", Deleted.IdEmployee.Value);
                MessageBox.Show($"{Deleted.IdEmployee}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            Employee = new Employee();
            var fullTableList = await Converter.Getter<Employee>("Employees");
            lists = new ObservableCollection<Employee>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<Employee>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Employee.IdEmployee != null)
            {
                await Converter.Updatter("Employees", Employee, Employee.IdEmployee.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdEmployee}, {item.LastName},{item.Name},{item.MiddleName},{item.PostId},{item.UserId},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Employees");
        }


        public string ValidationErrorMessage()
        {
            if (Employee == null) return String.Empty;
            if (string.IsNullOrWhiteSpace(Employee.Name)) return "Поле \"Имя\" незаполнено";
            if (string.IsNullOrWhiteSpace(Employee.LastName)) return "Поле \"Фамилия\" незаполнено";
            if (string.IsNullOrWhiteSpace(Employee.MiddleName)) return "Поле \"Отчество\" незаполнено";
            if (!ListPost.Select(x => x.IdPost).Contains(Post.IdPost)) return "Поле \"Должность\" не выбрано";
            if (!ListUser.Select(x => x.IdUser).Contains(User.IdUser)) return "Поле \"Пользователь\" не выбрано";

            return String.Empty;
        }
    }
}
