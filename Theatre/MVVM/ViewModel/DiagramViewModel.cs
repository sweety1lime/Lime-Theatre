using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Theatre.Core;
using Theatre.DBcontext;
using Theatre.MVVM.Model;

namespace Theatre.MVVM.ViewModel
{
    class DiagramViewModel : ObservableObject
    {
        private SeriesCollection _values;

        public SeriesCollection Values
        {
            get { return _values; }
            set
            {
                _values = value;
                OnPropertyChanged();
            }
        }

        public DiagramViewModel()
        {
            Init();
        }

        private async void Init()
        {
            var fullTableList = await Converter.Getter<Film>("Films");
            var AxisY = fullTableList.Select(x => x.GenreId);
            var AxisX = fullTableList.Select(x => x.GenreId).ToHashSet().OrderBy(x => x);

            var values = new ChartValues<Point>();

            foreach (var x in AxisX)
            {
                var point = new Point { X = x, Y = AxisY.Count(y => y == x) };
                values.Add(point);
            }
            Values = new SeriesCollection
            {
                new LineSeries
                {
                    Configuration = new CartesianMapper<Point>()
                        .X(point => point.X)
                        .Y(point => point.Y),
                    Values = values,

                    LineSmoothness = 0,
                    StrokeThickness = 2,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.White,
                    PointGeometrySize = 8
                }
            };
        }

    }
}