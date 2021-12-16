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
   public class RateViewModel : ObservableObject, ICRUD
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

        public RateViewModel()
        {
            InitAsync();
            ReadAsync();
        }

        public RelayCommand LogicalDeleteCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand ExportCommand { get; set; }

        private Rate _rate;

        public Rate Rate
        {
            get { return _rate; }
            set
            {
                _rate = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<Rate> _deleteList;
        public ObservableCollection<Rate> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private Rate _deleted;
        public Rate Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Rate> Rates;

        public ObservableCollection<Rate> lists
        {
            get { return Rates; }
            set
            {
                Rates = value;
                OnPropertyChanged();
            }
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
            Rate.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Rate.IsDeleted = true;
            UpdateAsync();
        }
       
      

        public async void CreateAsync()
        {
            if (ValidationErrorMessage() is string message && !string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message);
                return;
            }
            if (Rate != null)
            {
                Rate.IdRate = null;
                var listemployee = await Converter.Creatter<Rate>("Rates", Rate);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdRate != null)
            {
                var deleted = await Converter.Deletter("Rates", Deleted.IdRate.Value);
                MessageBox.Show($"{Deleted.NameRate}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
           
            Rate = new Rate();
            var fullTableList = await Converter.Getter<Rate>("Rates");
            lists = new ObservableCollection<Rate>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<Rate>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Rate.IdRate != null)
            {
                await Converter.Updatter("Rates", Rate, Rate.IdRate.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdRate}, {item.NameRate},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Rates");
        }
        public string ValidationErrorMessage()
        {
            if (Rate == null) return String.Empty;
            if (string.IsNullOrWhiteSpace(Rate.NameRate)) return "Поле \"Название\" незаполнено";
            
            return String.Empty;
        }
    }
}
