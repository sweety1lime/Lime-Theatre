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
   public class StatusViewModel : ObservableObject, ICRUD
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

        private ObservableCollection<Status> _deleteList;
        public ObservableCollection<Status> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private Status _deleted;
        public Status Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public StatusViewModel()
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
            Status.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Status.IsDeleted = true;
            UpdateAsync();
        }
        private Status _status;

        public Status Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Status> statuses;

        public ObservableCollection<Status> lists
        {
            get { return statuses; }
            set
            {
                statuses = value;
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
            if (Status != null)
            {
                Status.IdStatus = null;
                var listemployee = await Converter.Creatter<Status>("Status", Status);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdStatus != null)
            {
                var deleted = await Converter.Deletter("Status", Deleted.IdStatus.Value);
                MessageBox.Show($"{Deleted.StatusName}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            Status = new Status();
            var fullTableList = await Converter.Getter<Status>("Status");
            lists = new ObservableCollection<Status>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<Status>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Status.IdStatus != null)
            {
                await Converter.Updatter("Status", Status, Status.IdStatus.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdStatus}, {item.StatusName},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Status");
        }
        public string ValidationErrorMessage()
        {
            if (Status == null) return String.Empty;
            if (string.IsNullOrWhiteSpace(Status.StatusName)) return "Поле \"Название\" незаполнено";
           
            return String.Empty;
        }
    }
}
