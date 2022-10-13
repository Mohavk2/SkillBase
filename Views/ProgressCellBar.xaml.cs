using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SkillBase.Views
{
    /// <summary>
    /// Interaction logic for ProgressCellBar.xaml6+
    /// </summary>
    public partial class ProgressCellBar : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        const Orientation ORIENTATION = Orientation.Vertical;
        const int TOTAL_CELLS = 5;
        const int FILLED_CELLS = 0;

        public static readonly DependencyProperty CellsOrientationProperty =
            DependencyProperty.Register(
                name: "CellsOrientation",
                propertyType: typeof(Orientation),
                ownerType: typeof(ProgressCellBar),
                typeMetadata: new FrameworkPropertyMetadata(
                    defaultValue: ORIENTATION,
                    flags: FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    propertyChangedCallback: new PropertyChangedCallback(OnCellsOrientationChanged)));

        public Orientation CellsOrientation
        {
            get => (Orientation)GetValue(CellsOrientationProperty);
            set { SetValue(CellsOrientationProperty, value); RaisePropertyChanged(nameof(CellsOrientation)); }
        }
        private static void OnCellsOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is ProgressCellBar bar)
            {
                bar.InitItemsPanelTemplate();
            }
        }

        private void InitItemsPanelTemplate()
        {
            if(CellsOrientation == Orientation.Horizontal)
            {
                string xaml = @"<ItemsPanelTemplate   xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
                            <UniformGrid Rows=""1"">
                            </UniformGrid>
                    </ItemsPanelTemplate>";
                UGridControl.ItemsPanel = XamlReader.Parse(xaml) as ItemsPanelTemplate;
            }
            else
            {
                string xaml = @"<ItemsPanelTemplate   xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
                            <UniformGrid Columns=""1"">
                            </UniformGrid>
                    </ItemsPanelTemplate>";
                UGridControl.ItemsPanel = XamlReader.Parse(xaml) as ItemsPanelTemplate;
            }
        }

        public static readonly DependencyProperty TotalCellsCountProperty =
            DependencyProperty.Register(
                name: "TotalCellsCount",
                propertyType: typeof(int),
                ownerType: typeof(ProgressCellBar),
                typeMetadata: new FrameworkPropertyMetadata(
                    defaultValue: TOTAL_CELLS,
                    flags: FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    propertyChangedCallback: new PropertyChangedCallback(OnTotalCellsChanged)));

        public int TotalCellsCount
        {
            get => (int)GetValue(TotalCellsCountProperty);
            set => SetValue(TotalCellsCountProperty, value);
        }
        private static void OnTotalCellsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProgressCellBar bar)
            {
                bar.FillCells();
            }
        }

        public static readonly DependencyProperty FilledCellsCountProperty =
            DependencyProperty.Register(
                name: "FilledCellsCount",
                propertyType: typeof(int),
                ownerType: typeof(ProgressCellBar),
                typeMetadata: new FrameworkPropertyMetadata(
                    defaultValue: FILLED_CELLS,
                    flags: FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    propertyChangedCallback: new PropertyChangedCallback(OnFilledCellsChanged)));

        public int FilledCellsCount
        {
            get => (int)GetValue(FilledCellsCountProperty);
            set => SetValue(FilledCellsCountProperty, value);
        }
        private static void OnFilledCellsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is ProgressCellBar bar)
            {
                bar.FillCells();
            }
        }

        public ProgressCellBar()
        {
            InitializeComponent();
            InitItemsPanelTemplate();
        }

        void FillCells()
        {
            var cells = new ObservableCollection<Border>();
            for (int i = 0; i < TotalCellsCount; i++)
            {
                Border border = new()
                {
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    BorderThickness = new Thickness(1),
                    Background = new SolidColorBrush(Colors.Yellow),
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                };
                if (i < FilledCellsCount)
                {
                    border.Background = Brushes.LightGreen;
                }
                if(CellsOrientation == Orientation.Horizontal)
                {
                    cells.Add(border);
                }
                else
                {
                    cells.Insert(0, border);
                }
            }
            Cells = cells;
        }

        ObservableCollection<Border> _cells = new();
        public ObservableCollection<Border> Cells
        {
            get => _cells;
            set
            {
                _cells = value;
                RaisePropertyChanged(nameof(Cells));
            }
        }
    }
}
