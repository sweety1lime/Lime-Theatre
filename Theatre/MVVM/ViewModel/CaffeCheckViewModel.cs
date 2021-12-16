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
   public class CaffeCheckViewModel : ObservableObject, ICRUD
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

        private ObservableCollection<CaffeCheck> Checks;

        private CaffeCheck _check;

        public CaffeCheck CaffeCheck
        {
            get { return _check; }
            set
            {

                if (value != null)
                {

                    if (ListCaffe != null && ListCaffe.Any())
                        Caffe = ListCaffe.FirstOrDefault(x => x.IdCaffe == value.CaffeId)?? new Caffe();
                    if (ListType != null && ListType.Any())
                        TypePayment = ListType.FirstOrDefault(x => x.IdType == value.TypePaymentId)?? new TypePayment();
                }
                _check = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CaffeCheck> lists
        {
            get { return Checks; }
            set
            {
                Checks = value;
                OnPropertyChanged();
            }
        }

        private Caffe _caffe;

        public Caffe Caffe
        {
            get { return _caffe; }
            set
            {
                _caffe = value;


                CaffeCheck.CaffeId = value.IdCaffe??CaffeCheck.CaffeId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Caffe> _listCaffe;

        public ObservableCollection<Caffe> ListCaffe
        {
            get { return _listCaffe; }
            set
            {
                _listCaffe = value;

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


                CaffeCheck.TypePaymentId = value.IdType??CaffeCheck.TypePaymentId;
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

        private ObservableCollection<CaffeCheck> _deleteList;
        public ObservableCollection<CaffeCheck> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private CaffeCheck _deleted;
        public CaffeCheck Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public CaffeCheckViewModel()
        {
            InitAsync();
            ReadAsync();
        }
        private async void InitAsync()
        {
            CreateCommand = new RelayCommand(x => { CreateAsync(); });
            UpdateCommand = new RelayCommand(x => { UpdateAsync(); });
            DeleteCommand = new RelayCommand(x => { DeleteAsync(); });
            ListCaffe = await Converter.Getter<Caffe>("Caffes");
            ListType = await Converter.Getter<TypePayment>("TypePayments");
            BackCommand = new RelayCommand(x => { Back(); });
            LogicalDeleteCommand = new RelayCommand(x => { LogicalDelete(); });
            ExportCommand = new RelayCommand(x => { ExportTable(); });
        }
        public void Back()
        {
            CaffeCheck.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            CaffeCheck.IsDeleted = true;
            UpdateAsync();
        }
        public async void CreateAsync()
        {
            if (ValidationErrorMessage() is string message && !string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message);
                return;
            }
            if (CaffeCheck != null)
            {
                CaffeCheck.IdCheck = null;
                var listemployee = await Converter.Creatter<CaffeCheck>("CaffeChecks", CaffeCheck);
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
                var deleted = await Converter.Deletter("CaffeChecks", Deleted.IdCheck.Value);
                MessageBox.Show($"{Deleted.IdCheck}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            CaffeCheck = new CaffeCheck();
            var fullTableList = await Converter.Getter<CaffeCheck>("CaffeChecks");
            lists = new ObservableCollection<CaffeCheck>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<CaffeCheck>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (CaffeCheck.IdCheck != null)
            {
                await Converter.Updatter("CaffeChecks", CaffeCheck, CaffeCheck.IdCheck.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdCheck}, {item.DatePayment},{item.CountGoods},{item.Amount},{item.CaffeId},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "CaffeChecks");
        }
        public string ValidationErrorMessage()
        {
            if (CaffeCheck == null) return String.Empty;
            if (CaffeCheck.Amount < 0) return "Поле \"Сумма\" не должно быть отрицательным";
            if (CaffeCheck.CountGoods < 0) return "Поле \"Количество товаров\" не должно быть отрицательным";
            if (!ListCaffe.Select(x => x.IdCaffe).Contains(Caffe.IdCaffe)) return "Поле \"Касса\" не выбрано";
            if (!ListType.Select(x => x.IdType).Contains(TypePayment.IdType)) return "Поле \"Тип оплаты\" не выбрано";
            if (CaffeCheck.DatePayment.Year < 2010) return "Минимальное значение поля \"Время оплаты\" - 2010 год";
            return String.Empty;
        }
    }
}
