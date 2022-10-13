using SkillBase.Models;
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
    /// Interaction logic for WeekLinkUC.xaml
    /// </summary>
    public partial class WeekLinkUC : UserControl
    {
        public WeekLinkUC()
        {
            InitializeComponent();
        }
        private void FollowLink_Click(object sender, RoutedEventArgs e)
        {
            var sInfo = new System.Diagnostics.ProcessStartInfo(Link.Text)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(sInfo);
        }
    }
}
