using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Theatre.Core;
using Theatre.DBcontext;
using Theatre.MVVM.Model;

namespace Theatre.MVVM.ViewModel
{
    public class BookKeepingViewModel : ObservableObject, ICRUD
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

        private BookKeeping _bookKeeping;

        public BookKeeping BookKeeping
        {
            get { return _bookKeeping; }
            set
            {

                if (value != null)
                {

                    if (ListPayment != null && ListPayment.Any())
                        Payment = ListPayment.FirstOrDefault(x => x.IdPayment == value.PaymentId)??new Payment();
                    if (ListRecovery != null && ListRecovery.Any())
                        Recovery = ListRecovery.FirstOrDefault(x => x.IdRecovery == value.RecoveryId)?? new Recovery();
                }
                _bookKeeping = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BookKeeping> BookKeepings;

        public ObservableCollection<BookKeeping> lists
        {
            get { return BookKeepings; }
            set
            {
                BookKeepings = value;
                OnPropertyChanged();
            }
        }

        private Recovery _recovery;

        public Recovery Recovery
        {
            get { return _recovery; }
            set
            {
                _recovery = value;


                BookKeeping.RecoveryId = value.IdRecovery??BookKeeping.RecoveryId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Recovery> _listRecovery;

        public ObservableCollection<Recovery> ListRecovery
        {
            get { return _listRecovery; }
            set
            {
                _listRecovery = value;

                OnPropertyChanged();
            }
        }

        private Payment _payment;

        public Payment Payment
        {
            get { return _payment; }
            set
            {
                _payment = value;


                BookKeeping.PaymentId = value.IdPayment??BookKeeping.PaymentId;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Payment> _listPayment;

        public ObservableCollection<Payment> ListPayment
        {
            get { return _listPayment; }
            set
            {
                _listPayment = value;

                OnPropertyChanged();
            }
        }

        private ObservableCollection<BookKeeping> _deleteList;
        public ObservableCollection<BookKeeping> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private BookKeeping _deleted;
        public BookKeeping Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
                OnPropertyChanged();
            }
        }

        public BookKeepingViewModel()
        {
            InitAsync();
            ReadAsync();
        }

        private async void InitAsync()
        {
            CreateCommand = new RelayCommand(x => { CreateAsync(); });
            UpdateCommand = new RelayCommand(x => { UpdateAsync(); });
            DeleteCommand = new RelayCommand(x => { DeleteAsync(); });
            ListPayment = await Converter.Getter<Payment>("Payments");
            ListRecovery = await Converter.Getter<Recovery>("Recoveries");
            LogicalDeleteCommand = new RelayCommand(x => { LogicalDelete(); });
            BackCommand = new RelayCommand(x => { Back(); });
            ExportCommand = new RelayCommand(x => { ExportTable(); });
        }
        public void Back()
        {
            BookKeeping.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            BookKeeping.IsDeleted = true;
            UpdateAsync();
        }
        public async void CreateAsync()
        {
            if (ValidationErrorMessage() is string message && !string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(message);
                return;
            }
            if (BookKeeping != null)
            {
                BookKeeping.IdNote = null;
                var listemployee = await Converter.Creatter<BookKeeping>("BookKeepings", BookKeeping);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async void DeleteAsync()
        {
            if (Deleted.IdNote != null)
            {
                var deleted = await Converter.Deletter("BookKeepings", Deleted.IdNote.Value);
                MessageBox.Show($"{Deleted.IdNote}: {deleted}\n");
                ReadAsync();
            }
        }

        public async void ReadAsync()
        {
            BookKeeping = new BookKeeping();
            var fullTableList = await Converter.Getter<BookKeeping>("BookKeepings");
            lists = new ObservableCollection<BookKeeping>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<BookKeeping>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (BookKeeping.IdNote != null)
            {
                await Converter.Updatter("BookKeepings", BookKeeping, BookKeeping.IdNote.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdNote}, {item.PaymentId},{item.RecoveryId},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "BookKeepings");
        }
        public string ValidationErrorMessage()
        {
            if (BookKeeping == null) return String.Empty;

            if (!ListRecovery.Select(x => x.IdRecovery).Contains(Recovery.IdRecovery)) return "Поле \"Вычеты\" не выбрано";
            if (!ListPayment.Select(x => x.IdPayment).Contains(Payment.IdPayment)) return "Поле \"Зарплаты\" не выбрано";

            return String.Empty;
        }
    }
}