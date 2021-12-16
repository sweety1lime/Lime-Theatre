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
   public class RowViewModel : ObservableObject, ICRUD
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
        public RelayCommand ExportCommand { get; set; }
        public RelayCommand BackCommand { get; set; }

        private ObservableCollection<Row> _deleteList;
        public ObservableCollection<Row> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private Row _deleted;
        public Row Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public RowViewModel()
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
            Row.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Row.IsDeleted = true;
            UpdateAsync();
        }
        private Row _row;

        public Row Row
        {
            get { return _row; }
            set
            {
                _row = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Row> Rows;

        public ObservableCollection<Row> lists
        {
            get { return Rows; }
            set
            {
                Rows = value;
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
            if (Row != null)
            {
                Row.IdRow = null;
                var listemployee = await Converter.Creatter<Row>("Rows", Row);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdRow != null)
            {
                var deleted = await Converter.Deletter("Rows", Deleted.IdRow.Value);
                MessageBox.Show($"{Deleted.CategoryRow}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            
            Row = new Row();
            var fullTableList = await Converter.Getter<Row>("Rows");
            lists = new ObservableCollection<Row>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<Row>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Row.IdRow != null)
            {
                await Converter.Updatter("Rows", Row, Row.IdRow.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdRow}, {item.CategoryRow},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Employees");
        }

        public string ValidationErrorMessage()
        {
            if (Row == null) return String.Empty;
            if (string.IsNullOrWhiteSpace(Row.CategoryRow)) return "Поле \"Категория и Ряд\" незаполнено";
           

            return String.Empty;
        }
    }
}
