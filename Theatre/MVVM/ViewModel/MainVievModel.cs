using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theatre.Core;
using Theatre.MVVM.Model;

namespace Theatre.MVVM.ViewModel
{
    class MainVievModel : ObservableObject
    {
        #region Команды
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand GenreViewCommand { get; set; }
        public RelayCommand StudioViewCommand { get; set; }
        public RelayCommand RowViewCommand { get; set; }
        public RelayCommand SeatViewCommand { get; set; }
        public RelayCommand StatusViewCommand { get; set; }
        public RelayCommand HallViewCommand { get; set; }
        public RelayCommand TypeHallViewCommand { get; set; }
        public RelayCommand TypePaymentViewCommand { get; set; }
        public RelayCommand AgeRatingViewCommand { get; set; }
        public RelayCommand BookKeepingViewCommand { get; set; }
        public RelayCommand CaffeViewCommand { get; set; }
        public RelayCommand CaffeCheckViewCommand { get; set; }
        public RelayCommand CheckViewCommand { get; set; }
        public RelayCommand CashBoxViewCommand { get; set; }
        public RelayCommand EmployeeViewCommand { get; set; }
        public RelayCommand PaymentViewCommand { get; set; }
        public RelayCommand PostViewCommand { get; set; }
        public RelayCommand RateViewCommand { get; set; }
        public RelayCommand RecoveryViewCommand { get; set; }
        public RelayCommand RentViewCommand { get; set; }
        public RelayCommand SessionViewCommand { get; set; }
        public RelayCommand TicketViewCommand { get; set; }
        public RelayCommand UserViewCommand { get; set; }
        #endregion

        #region Верстка
        public GanreViewModel GenreVM { get; set; }
        public FilmViewModel HomeVM { get; set; }
        public AgeRatingViewModel AgeVM { get; set; }
        public BookKeepingViewModel BookVM { get; set; }
        public CaffeCheckViewModel CaffeCheckVM { get; set; }
        public CaffeViewModel CafeVM { get; set; }
        public CashBoxViewModel CashBoxVM { get; set; }
        public CheckViewModel CheckVM { get; set; }
        public CinemaHallViewModel HallVM { get; set; }
        public EmployeeViewModel EmployeeVM { get; set; }
        public PaymentViewModel PaymentVM { get; set; }
        public PostViewModel PostVM { get; set; }
        public RateViewModel RateVM { get; set; }
        public RecoveryViewModel RecoveryVM { get; set; }
        public RentCinemaViewModel RentVM { get; set; }
        public RowViewModel RowVM { get; set; }
        public SeatViewModel SeatVM { get; set; }
        public SessionViewModel SessionVM { get; set; }
        public StatusViewModel StatusVM { get; set; }
        public StudioViewModel StudioVM { get; set; }
        public TicketsViewModel TicketVM { get; set; }
        public TypeHallViewModel TypeHallVM { get; set; }
        public TypePaymentViewModel TypePaymentVM { get; set; }
        public UserViewModel UserVM { get; set; }
        #endregion


        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged();
            
            }
        }
       

        public MainVievModel()
        {
            HomeVM = new FilmViewModel();
            GenreVM = new GanreViewModel();
            AgeVM = new AgeRatingViewModel();
            BookVM = new BookKeepingViewModel();
            CafeVM = new CaffeViewModel();
            CaffeCheckVM = new CaffeCheckViewModel();
            CashBoxVM = new CashBoxViewModel();
            CheckVM = new CheckViewModel();
            HallVM = new CinemaHallViewModel();
            EmployeeVM = new EmployeeViewModel();
            PaymentVM = new PaymentViewModel();
            PostVM = new PostViewModel();
            RateVM = new RateViewModel();
            RecoveryVM = new RecoveryViewModel();
            RentVM = new RentCinemaViewModel();
            RowVM = new RowViewModel();
            SeatVM = new SeatViewModel();
            SessionVM = new SessionViewModel();
            StatusVM = new StatusViewModel();
            StudioVM = new StudioViewModel();
            TicketVM = new TicketsViewModel();
            TypeHallVM = new TypeHallViewModel();
            TypePaymentVM = new TypePaymentViewModel();
            UserVM = new UserViewModel();
            ChangeCurrentView(HomeVM);

            HomeViewCommand = new RelayCommand(o => { ChangeCurrentView(HomeVM); });
            GenreViewCommand = new RelayCommand(o => { ChangeCurrentView(GenreVM); });
            AgeRatingViewCommand = new RelayCommand(o => { ChangeCurrentView(AgeVM); });
            BookKeepingViewCommand = new RelayCommand(o => { ChangeCurrentView(BookVM); });
            CaffeViewCommand = new RelayCommand(o => { ChangeCurrentView(CafeVM); });
            CaffeCheckViewCommand = new RelayCommand(o => { ChangeCurrentView(CaffeCheckVM); });
            CashBoxViewCommand = new RelayCommand(o => { ChangeCurrentView(CashBoxVM); });
            CheckViewCommand = new RelayCommand(o => { ChangeCurrentView(CheckVM); });
            HallViewCommand = new RelayCommand(o => { ChangeCurrentView(HallVM); });
            EmployeeViewCommand = new RelayCommand(o => { ChangeCurrentView(EmployeeVM); });
            PaymentViewCommand = new RelayCommand(o => { ChangeCurrentView(PaymentVM); });
            PostViewCommand = new RelayCommand(o => { ChangeCurrentView(PostVM); });
            RateViewCommand = new RelayCommand(o => { ChangeCurrentView(RateVM); });
            RecoveryViewCommand = new RelayCommand(o => { ChangeCurrentView(RecoveryVM); });
            RentViewCommand = new RelayCommand(o => { ChangeCurrentView(RentVM); });
            RowViewCommand = new RelayCommand(o => { ChangeCurrentView(RowVM); });
            SeatViewCommand = new RelayCommand(o => { ChangeCurrentView(SeatVM); });
            SessionViewCommand = new RelayCommand(o => { ChangeCurrentView(SessionVM); });
            StatusViewCommand = new RelayCommand(o => { ChangeCurrentView(StatusVM); });
            StudioViewCommand = new RelayCommand(o => { ChangeCurrentView(StudioVM); });
            TicketViewCommand = new RelayCommand(o => { ChangeCurrentView(TicketVM); });
            TypeHallViewCommand = new RelayCommand(o => { ChangeCurrentView(TypeHallVM); });
            TypePaymentViewCommand = new RelayCommand(o => { ChangeCurrentView(TypePaymentVM); });
            UserViewCommand = new RelayCommand(o => { ChangeCurrentView(UserVM); });
        }

        private void ChangeCurrentView(object viewToChange)
        {
            CurrentView = viewToChange;
        }
    }
}
