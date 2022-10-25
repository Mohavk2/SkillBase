using SkillBase.ViewModels;
using SkillBase.ViewModels.Skills;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkillBase.Views.Skills
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
                    if (suc.IsVisible)
                    {
                        Task.Run(async () => {
                            await svm.Init();
                        });
                    }
                    else
                    {
                        svm.DisposeResources();
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

                double step = 3 / ScaleSlyder.Value;

                if (currentPoint.X < _placementPoint.X) ScrollView.ScrollToHorizontalOffset(ScrollView.HorizontalOffset + step);
                if (currentPoint.X > _placementPoint.X) ScrollView.ScrollToHorizontalOffset(ScrollView.HorizontalOffset - step);
                if (currentPoint.Y < _placementPoint.Y) ScrollView.ScrollToVerticalOffset(ScrollView.VerticalOffset + step);
                if (currentPoint.Y > _placementPoint.Y) ScrollView.ScrollToVerticalOffset(ScrollView.VerticalOffset - step);

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

        private void Scroll_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                e.Handled = true;
                if(e.Delta > 0)
                {
                    ScaleSlyder.Value += 0.1;
                }
                else
                {
                    ScaleSlyder.Value -= 0.1;
                }
            }
        }
    }
}
