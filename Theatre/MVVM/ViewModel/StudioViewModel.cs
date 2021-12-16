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
   public class StudioViewModel: ObservableObject, ICRUD
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

        private ObservableCollection<Studio> _deleteList;
        public ObservableCollection<Studio> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private Studio _deleted;
        public Studio Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public StudioViewModel()
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
            Studio.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Studio.IsDeleted = true;
            UpdateAsync();
        }

        private Studio _studio;

        public Studio Studio
        {
            get { return _studio; }
            set
            {
                _studio = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Studio> studios;

        public ObservableCollection<Studio> lists
        {
            get { return studios; }
            set
            {
                studios = value;
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
            if (Studio != null)
            {
                Studio.IdStudio = null;
                var listemployee = await Converter.Creatter<Studio>("Studios", Studio);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdStudio != null)
            {
                var deleted = await Converter.Deletter("Studios", Deleted.IdStudio.Value);
                MessageBox.Show($"{Deleted.NameStudio}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
           
            Studio = new Studio();

            var fullTableList = await Converter.Getter<Studio>("Studios");
            lists = new ObservableCollection<Studio>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<Studio>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Studio.IdStudio != null)
            {
                await Converter.Updatter("Studios", Studio, Studio.IdStudio.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdStudio}, {item.NameStudio},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Employees");
        }

        public string ValidationErrorMessage()
        {
            if (Studio == null) return String.Empty;
            if (string.IsNullOrWhiteSpace(Studio.NameStudio)) return "Поле \"Название\" незаполнено";
            

            return String.Empty;
        }
    }
}
