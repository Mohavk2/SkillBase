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

namespace SkillBase.Views
{
    /// <summary>
    /// Interaction logic for ReferenceUrlUC.xaml
    /// </summary>
    public partial class ReferenceUrlUC : UserControl
    {
        public ReferenceUrlUC()
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
