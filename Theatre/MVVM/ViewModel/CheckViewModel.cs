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
   public class CheckViewModel: ObservableObject, ICRUD
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

        private ObservableCollection<Check> Checks;

        private Check _check;

        public Check Check
        {
            get { return _check; }
            set
            {

                if (value != null)
                {

                    if (ListCashBox != null && ListCashBox.Any())
                        Cashbox = ListCashBox.FirstOrDefault(x => x.IdCashBox == value.CashBoxId)?? new Cashbox();
                    if (ListType != null && ListType.Any())
                        TypePayment = ListType.FirstOrDefault(x => x.IdType == value.TypePaymentId) ?? new TypePayment();
                }
                _check = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Check> lists
        {
            get { return Checks; }
            set
            {
                Checks = value;
                OnPropertyChanged();
            }
        }

        private Cashbox _caffe;

        public Cashbox Cashbox
        {
            get { return _caffe; }
            set
            {
                _caffe = value;


                Check.CashBoxId = value.IdCashBox??Check.CashBoxId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Cashbox> _listcashBox;

        public ObservableCollection<Cashbox> ListCashBox
        {
            get { return _listcashBox; }
            set
            {
                _listcashBox = value;

                OnPropertyChanged();
            }
        }

        private TypePayment _type;

        public TypePayment TypePayment
        {
            get { return _type; }
            set
            {
                _type = value;


                Check.TypePaymentId = value.IdType?? Check.TypePaymentId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TypePayment> _listType;

        public ObservableCollection<TypePayment> ListType
        {
            get { return _listType; }
            set
            {
                _listType = value;

                OnPropertyChanged();
            }
        }
        private ObservableCollection<Check> _deleteList;
        public ObservableCollection<Check> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private Check _deleted;
        public Check Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }
        public CheckViewModel()
        {
            InitAsync();
            ReadAsync();
        }
        private async void InitAsync()
        {
            CreateCommand = new RelayCommand(x => { CreateAsync(); });
            UpdateCommand = new RelayCommand(x => { UpdateAsync(); });
            DeleteCommand = new RelayCommand(x => { DeleteAsync(); });
            ListCashBox = await Converter.Getter<Cashbox>("Cashboxes");
            ListType = await Converter.Getter<TypePayment>("TypePayments");
            LogicalDeleteCommand = new RelayCommand(x => { LogicalDelete(); });
            BackCommand = new RelayCommand(x => { Back(); });
            ExportCommand = new RelayCommand(x => { ExportTable(); });
        }

        public void Back()
        {
            Check.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Check.IsDeleted = true;
            UpdateAsync();
        }


        public async void CreateAsync()
        {
            if (ValidationErrorMessage() is string message && !string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message);
                return;
            }
            if (Check != null)
            {
                Check.IdCheck = null;
                var listemployee = await Converter.Creatter<Check>("Checks", Check);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdCheck != null)
            {
                var deleted = await Converter.Deletter("Checks", Deleted.IdCheck.Value);
                MessageBox.Show($"{Deleted.IdCheck}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            Check = new Check();
            var fullTableList = await Converter.Getter<Check>("Checks");
            lists = new ObservableCollection<Check>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<Check>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Check.IdCheck != null)
            {
                await Converter.Updatter("Checks", Check, Check.IdCheck.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdCheck}, {item.DatePayment},{item.CountGoods},{item.CashBoxId},{item.CashBoxId},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "Checks");
        }
        public string ValidationErrorMessage()
        {
            if (Check == null) return String.Empty;
            if (Check.Amount < 0) return "Поле \"Сумма\" не должно быть отрицательным";
            if (Check.CountGoods < 0) return "Поле \"Количество товаров\" не должно быть отрицательным";
            if (!ListCashBox.Select(x => x.IdCashBox).Contains(Cashbox.IdCashBox)) return "Поле \"Касса\" не выбрано";
            if (!ListType.Select(x => x.IdType).Contains(TypePayment.IdType)) return "Поле \"Тип оплаты\" не выбрано";
            if (Check.DatePayment.Year < 2010) return "Минимальное значение поля \"Время оплаты\" - 2010 год";
            return String.Empty;
        }
    }
}
