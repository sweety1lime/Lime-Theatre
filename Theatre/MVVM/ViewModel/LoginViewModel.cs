using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Theatre.Core;
using Theatre.DBcontext;
using Theatre.MVVM.Model;
using Theatre.MVVM.View;
using Theatre.MVVM.ViewModel;

namespace Theatre.MVVM.ViewModel
{
    class LoginViewModel : ObservableObject
    {
        public RelayCommand LoginCommand { get; set; }

        private ObservableCollection<User> _user;
        private ObservableCollection<Post> _post;

        private Auth _login;

        public Auth Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel()
        {
            InitAsync();
        }

        private async void InitAsync()
        {
            Login = new Auth();
            LoginCommand = new RelayCommand(o => { CheckLogin(); });
            _user = await Converter.Getter<User>("Users");
            _post = await Converter.Getter<Post>("Posts");

        }

        private void CheckLogin()
        {
            var human = _user.FirstOrDefault(x =>
                x.Login == Login.Login && x.Password == Hash.Hashing(Login.Password));
            if (human != null)
                OpenMainWindow(human);
            else
                MessageBox.Show("Неверный логин или пароль");
        }

        private void OpenMainWindow(User human)
        {
            var currentWindow = Application.Current.MainWindow;
            var role = _post.First(x => x.IdPost == human.PostId);
            var mainWindow = new MainWindow();
            mainWindow.VievModel.Post = role;
            Application.Current.MainWindow = mainWindow;
            Application.Current.MainWindow.Show();
            currentWindow.Close();
        }
    }
}
