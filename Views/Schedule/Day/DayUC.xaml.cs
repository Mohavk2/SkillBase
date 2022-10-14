using SkillBase.ViewModels.Schedule.Day;
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

namespace SkillBase.Views.Schedule.Day
{
    /// <summary>
    /// Interaction logic for DayUC.xaml
    /// </summary>
    public partial class DayUC : UserControl
    {
        public DayUC()
        {
            InitializeComponent();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DayUC duc)
            {
                if (duc.DataContext is DayViewModel dvm)
                {
                    if (duc.IsVisible)
                    {
                        Task.Run(() => dvm.Init());
                        UpdateTimeOffset();
                    }
                    else
                    {
                        dvm.DisposeResources();
                    }
                }
            }
        }

        void UpdateTimeOffset()
        {
            var minutes = (DateTime.Now - DateTime.Today).TotalMinutes;
            double timeOffset = (1000d / 1440d) * minutes;
            Scroll.ScrollToVerticalOffset((int)timeOffset - 20);
            Canvas.SetTop(ClockArrow, minutes);
        }
    }
}
