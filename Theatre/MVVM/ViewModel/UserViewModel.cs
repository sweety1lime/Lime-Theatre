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
   public class UserViewModel : ObservableObject, ICRUD
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

        private User _user;

        public User User
        {
            get { return _user; }
            set
            {
                if (value != null)
                {
                   
                    if (ListPost != null && ListPost.Any())
                        Post = ListPost.FirstOrDefault(x => x.IdPost == value.PostId)?? new Post();    
                }
                _user = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<User> users;

        public ObservableCollection<User> lists
        {
            get { return users; }
            set
            {
                users = value;
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


                User.PostId = value.IdPost??User.PostId;
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

        public RelayCommand LogicalDeleteCommand { get; set; }

        private ObservableCollection<User> _deleteList;
        public ObservableCollection<User> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private User _deleted;
        public User Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public UserViewModel()
        {
            InitAsync();
            ReadAsync();
        }
        private async void InitAsync()
        {
            CreateCommand = new RelayCommand(x => { CreateAsync(); });
            UpdateCommand = new RelayCommand(x => { UpdateAsync(); });
            DeleteCommand = new RelayCommand(x => { DeleteAsync(); });
            ListPost = await Converter.Getter<Post>("Posts");
            LogicalDeleteCommand = new RelayCommand(x => { LogicalDelete(); });
            BackCommand = new RelayCommand(x => { Back(); });
            ExportCommand = new RelayCommand(x => { ExportTable(); });
        }

        public void Back()
        {
            User.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            User.IsDeleted = true;
            UpdateAsync();
        }
        public async void CreateAsync()
        {
            if (ValidationErrorMessage() is string message && !string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message);
                return;
            }
            if (User != null)
            {
                User.IdUser = null;
                if (!Hash.IsHash(User.Password))
                    User.Password = Hash.Hashing(User.Password);
                var listemployee = await Converter.Creatter<User>("Users", User);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdUser != null)
            {
                var deleted = await Converter.Deletter("Users", Deleted.IdUser.Value);
                MessageBox.Show($"{Deleted.IdUser}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            User = new User();
            var fullTableList = await Converter.Getter<User>("Users");
            lists = new ObservableCollection<User>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<User>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {

            if (User.IdUser != null)
            {
                if (!Hash.IsHash(User.Password))
                    User.Password = Hash.Hashing(User.Password);
                await Converter.Updatter("Users", User, User.IdUser.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdUser}, {item.Login},{item.Password},{item.PostId},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Employees");
        }

        public string ValidationErrorMessage()
        {
            if (User == null) return String.Empty;
            if (string.IsNullOrWhiteSpace(User.Login)) return "Поле \"Логин\" незаполнено";
            if (string.IsNullOrWhiteSpace(User.Password)) return "Поле \"Пароль\" незаполнено";
            if (!ListPost.Select(x => x.IdPost).Contains(Post.IdPost)) return "Поле \"Должность\" не выбрано";
            

            return String.Empty;
        }
    }
}
