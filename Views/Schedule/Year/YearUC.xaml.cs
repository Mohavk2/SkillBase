using SkillBase.ViewModels.Schedule.Week;
using SkillBase.ViewModels.Schedule.Year;
using SkillBase.Views.Schedule.Week;
using System;
using System.Collections.Generic;
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

namespace SkillBase.Views.Schedule.Year
{
    /// <summary>
    /// Interaction logic for YearUC.xaml
    /// </summary>
    public partial class YearUC : UserControl
    {
        public YearUC()
        {
            InitializeComponent();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is YearUC yuc)
            {
                if (yuc.DataContext is YearViewModel yvm)
                {
                    if (yuc.IsVisible)
                    {
                        Task.Run(() => yvm.Init());
                    }
                    else
                    {
                        yvm.DisposeResources();
                    }
                }
            }
        }
        Point _placementPoint;
        bool _isMouseScrolling = false;  //To prevent other events when scrolling

        private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                e.Handled = true;
                _isMouseScrolling = true;
                var currentPoint = GetMousePos();

                var shiftX = currentPoint.X - _placementPoint.X;
                var shiftY = currentPoint.Y - _placementPoint.Y;

                double step = 1.5;

                if (currentPoint.X < _placementPoint.X) Scroll.ScrollToHorizontalOffset(Scroll.HorizontalOffset + step);
                if (currentPoint.X > _placementPoint.X) Scroll.ScrollToHorizontalOffset(Scroll.HorizontalOffset - step);
                if (currentPoint.Y < _placementPoint.Y) Scroll.ScrollToVerticalOffset(Scroll.VerticalOffset + step);
                if (currentPoint.Y > _placementPoint.Y) Scroll.ScrollToVerticalOffset(Scroll.VerticalOffset - step);

                _placementPoint = GetMousePos();
            }
            else this.Cursor = Cursors.Arrow;
        }

        private void ScrollViewer_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _placementPoint = GetMousePos();
            this.Cursor = Cursors.ScrollAll;
        }

        Point GetMousePos() => this.PointToScreen(Mouse.GetPosition(this));

        private void Scroll_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            if (_isMouseScrolling) e.Handled = true;
            _isMouseScrolling = false;
        }
    }
}
