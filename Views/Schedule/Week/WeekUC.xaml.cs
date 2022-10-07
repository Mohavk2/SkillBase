using SkillBase.ViewModels.Schedule;
using SkillBase.Views.Schedule.Day;
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

namespace SkillBase.Views.Schedule.Week
{
    /// <summary>
    /// Interaction logic for WeekUC.xaml
    /// </summary>
    public partial class WeekUC : UserControl
    {
        public WeekUC()
        {
            InitializeComponent();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is WeekUC wuc && wuc.IsVisible)
            {
                if (wuc.DataContext is WeekViewModel wvm)
                {
                    Task.Run(() => wvm.Init());
                }
            }
        }
    }
}
