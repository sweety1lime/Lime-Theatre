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
   public class PostViewModel : ObservableObject, ICRUD
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
        private ObservableCollection<Post> _deleteList;
        public ObservableCollection<Post> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private Post _deleted;
        public Post Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public PostViewModel()
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
            Post.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Post.IsDeleted = true;
            UpdateAsync();
        }
        private Post _post;

        public Post Post
        {
            get { return _post; }
            set
            {
                _post = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Post> Posts;

        public ObservableCollection<Post> lists
        {
            get { return Posts; }
            set
            {
                Posts = value;
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
            if (Post != null)
            {
                Post.IdPost = null;
                var listemployee = await Converter.Creatter<Post>("Posts", Post);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdPost != null)
            {
                var deleted = await Converter.Deletter("Posts", Deleted.IdPost.Value);
                MessageBox.Show($"{Deleted.NamePost}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            
            Post = new Post();

            var fullTableList = await Converter.Getter<Post>("Posts");
            lists = new ObservableCollection<Post>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<Post>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Post.IdPost != null)
            {
                await Converter.Updatter("Posts", Post, Post.IdPost.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdPost}, {item.NamePost},{item.Salary},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Posts");
        }

        public string ValidationErrorMessage()
        {
            if (Post == null) return String.Empty;
            if (string.IsNullOrWhiteSpace(Post.NamePost)) return "Поле \"Название\" незаполнено";
            if (Post.Salary < 0) return "Поле \"Ставка\" не должно быть отрицательным";

            return String.Empty;
        }
    }
}
