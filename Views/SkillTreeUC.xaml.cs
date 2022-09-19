using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkillBase.Views
{
    /// <summary>
    /// Interaction logic for SkillListUC.xaml
    /// </summary>
    public partial class SkillTreeUC : UserControl
    {
        public static readonly DependencyProperty SkillDropCommandProperty = DependencyProperty
        .Register("SkillDropCommand", typeof(ICommand), typeof(SkillTreeUC), new PropertyMetadata(null));

        public ICommand SkillDropCommand
        {
            get { return (ICommand)GetValue(SkillDropCommandProperty); }
            set { SetValue(SkillDropCommandProperty, value); }
        }

        public SkillTreeUC()
        {
            InitializeComponent();
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

                double step = 0.7;

                if (shiftX < 0) Scroll.ScrollToHorizontalOffset(Scroll.HorizontalOffset + (step + Math.Abs(shiftX) * 0.01));
                if (shiftX > 0) Scroll.ScrollToHorizontalOffset(Scroll.HorizontalOffset - (step + Math.Abs(shiftX) * 0.01));
                if (shiftY < 0) Scroll.ScrollToVerticalOffset(Scroll.VerticalOffset + (step + Math.Abs(shiftY) * 0.01));
                if (shiftY > 0) Scroll.ScrollToVerticalOffset(Scroll.VerticalOffset - (step + Math.Abs(shiftY) * 0.01));
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
