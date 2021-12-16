using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Theatre.MVVM.Model;
using Theatre.MVVM.ViewModel;

namespace Theatre.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для AgeRatingView.xaml
    /// </summary>
    public partial class AgeRatingView : UserControl
    {
        private GridViewColumnHeader _sortedColumn;
        private bool isAscending;
        public AgeRatingViewModel ViewModel => DataContext as AgeRatingViewModel;
        public AgeRatingView()
        {
            InitializeComponent();
        }

        private void ColumnSorting(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = sender as GridViewColumnHeader;

            string sortBy = column.Tag.ToString();
            if (_sortedColumn == column && !isAscending)
            {
                isAscending = true;
                ViewModel.lists = new ObservableCollection<AgeRating>(
                    ViewModel.lists.OrderBy(x => x.GetType().GetProperty(sortBy).GetValue(x, null)));
            }
            else
            {
                _sortedColumn = column;
                isAscending = false;
                ViewModel.lists = new ObservableCollection<AgeRating>(
                    ViewModel.lists.OrderByDescending(x => x.GetType().GetProperty(sortBy).GetValue(x, null)));
            }
        }
    }
}
