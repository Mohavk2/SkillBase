using SkillBase.ViewModels.Schedule;
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

namespace SkillBase.Views.Schedule.Month
{
    /// <summary>
    /// Interaction logic for MonthUC.xaml
    /// </summary>
    public partial class MonthUC : UserControl
    {
        public MonthUC()
        {
            InitializeComponent();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is MonthUC muc && muc.IsVisible)
            {
                if (muc.DataContext is MonthViewModel mwm)
                {
                    Task.Run(() => mwm.Init());
                }
            }
        }
    }
}
