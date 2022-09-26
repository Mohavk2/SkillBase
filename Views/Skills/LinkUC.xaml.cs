using System.Windows;
using System.Windows.Controls;

namespace SkillBase.Views.Skills
{
    /// <summary>
    /// Interaction logic for LinkUC.xaml
    /// </summary>
    public partial class LinkUC : UserControl
    {
        public LinkUC()
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
