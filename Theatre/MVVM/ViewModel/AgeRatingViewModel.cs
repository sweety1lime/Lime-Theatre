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
   public class AgeRatingViewModel : ObservableObject, ICRUD
    {
        public RelayCommand CreateCommand { get;set;}
        public RelayCommand UpdateCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand LogicalDeleteCommand { get; set; }

        public RelayCommand ExportCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public AgeRatingViewModel()
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

        private AgeRating _rating;

        public AgeRating Rating
        {
            get { return _rating; }
            set
            {
                _rating = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AgeRating> AgeRatings;

        public ObservableCollection<AgeRating> lists
        {
            get { return AgeRatings; }
            set
            {
                AgeRatings = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AgeRating> _deleteList;
        public ObservableCollection<AgeRating> DeleteList
        {
            get => _deleteList;
            set
            {
                _deleteList = value;
                OnPropertyChanged();
            }
        }

        private AgeRating _deleted;
        public AgeRating Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
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
            if (Rating != null)
            {
                Rating.IdRating = null;
                var listemployee = await Converter.Creatter<AgeRating>("AgeRatings", Rating);
                if (listemployee != null)
                {
                    lists = listemployee;
                }
            }
        }

        public async  void DeleteAsync()
        {
            if (Deleted.IdRating != null)
            {
               var deleted= await Converter.Deletter("AgeRatings", Deleted.IdRating.Value);
                MessageBox.Show($"{Deleted.NameRating}: {deleted}\n");
                ReadAsync();
            }
        }

        public void Back()
        {
            Rating.IsDeleted = false;
            UpdateAsync();
        }
        public void LogicalDelete()
        {
            Rating.IsDeleted = true;
            UpdateAsync();
        }

        public async void ReadAsync()
        {
            DeleteList = new ObservableCollection<AgeRating>();
            Rating = new AgeRating();

            var fullTableList = await Converter.Getter<AgeRating>("AgeRatings");
            lists = new ObservableCollection<AgeRating>(fullTableList.Where(x => !x.IsDeleted));
            DeleteList = new ObservableCollection<AgeRating>(fullTableList.Where(x => x.IsDeleted));
        }

        public async void UpdateAsync()
        {
            if (Rating.IdRating != null)
            {
                await Converter.Updatter("AgeRatings", Rating, Rating.IdRating.Value);
                ReadAsync();
            }
        }

        public void ExportTable()
        {
            List<string> exportList = new List<string>();
            foreach (var item in lists)
                exportList.Add($"{item.IdRating}, {item.NameRating},{item.IsDeleted}");
            CreateCSV.WriteCSV(exportList, "AgeRatings");
        }

       


        public string ValidationErrorMessage()
        {
            if (Rating == null) return String.Empty;

            if (String.IsNullOrWhiteSpace(Rating.NameRating)) return "Поле \"Название\" не заполнено";

            return String.Empty;
        }
    }
}
