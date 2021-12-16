using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Theatre.MVVM.Model;
using Theatre.MVVM.ViewModel;

namespace Theatre.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для RentCinemaView.xaml
    /// </summary>
    public partial class RentCinemaView : UserControl
    {
        private GridViewColumnHeader _sortedColumn;
        private bool isAscending;
        public RentCinemaViewModel ViewModel => DataContext as RentCinemaViewModel;
        public RentCinemaView()
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
                ViewModel.lists = new ObservableCollection<RentCinema>(
                    ViewModel.lists.OrderBy(x => x.GetType().GetProperty(sortBy).GetValue(x, null)));
            }
            else
            {
                _sortedColumn = column;
                isAscending = false;
                ViewModel.lists = new ObservableCollection<RentCinema>(
                    ViewModel.lists.OrderByDescending(x => x.GetType().GetProperty(sortBy).GetValue(x, null)));
            }
        }
    }
}
