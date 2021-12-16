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
   public class TypePaymentViewModel : ObservableObject, ICRUD
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

        private ObservableCollection<TypePayment> _deleteList;
        public ObservableCollection<TypePayment> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private TypePayment _deleted;
        public TypePayment Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public TypePaymentViewModel()
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
            Type.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Type.IsDeleted = true;
            UpdateAsync();
        }

        private TypePayment _typePayment;

        public TypePayment Type
        {
            get { return _typePayment; }
            set
            {
                _typePayment = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TypePayment> typePayments;

        public ObservableCollection<TypePayment> lists
        {
            get { return typePayments; }
            set
            {
                typePayments = value;
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
            if (Type != null)
            {
                Type.IdType = null;
                var listemployee = await Converter.Creatter<TypePayment>("TypePayments", Type);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdType != null)
            {
                var deleted = await Converter.Deletter("TypePayments", Deleted.IdType.Value);
                MessageBox.Show($"{Deleted.NameType}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
           
            Type = new TypePayment();
            var fullTableList = await Converter.Getter<TypePayment>("TypePayments");
            lists = new ObservableCollection<TypePayment>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<TypePayment>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Type.IdType != null)
            {
                await Converter.Updatter("TypePayments", Type, Type.IdType.Value);
                ReadAsync();
            }
        }


        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdType}, {item.NameType},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "TypePayments");
        }

        public string ValidationErrorMessage()
        {
            if (Type == null) return String.Empty;
            if (string.IsNullOrWhiteSpace(Type.NameType)) return "Поле \"Название\" незаполнено";
           

            return String.Empty;
        }
    }
}

