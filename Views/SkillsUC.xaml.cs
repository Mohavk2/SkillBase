using SkillBase.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkillBase.Views
{
    /// <summary>
    /// Interaction logic for SkillListUC.xaml
    /// </summary>
    public partial class SkillsUC : UserControl
    {
        public static readonly DependencyProperty SkillDropCommandProperty = DependencyProperty
        .Register("SkillDropCommand", typeof(ICommand), typeof(SkillsUC), new PropertyMetadata(null));

        public ICommand SkillDropCommand
        {
            get { return (ICommand)GetValue(SkillDropCommandProperty); }
            set { SetValue(SkillDropCommandProperty, value); }
        }

        public SkillsUC()
        {
            InitializeComponent();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is SkillsUC suc)
            {
                if (suc.DataContext is SkillsViewModel svm)
                {
                    Task.Run(() => svm.Init());
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
            if(_isMouseScrolling) e.Handled = true;
            _isMouseScrolling = false;
        }
    }
}
