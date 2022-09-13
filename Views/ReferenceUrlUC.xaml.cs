using System.Windows;
using System.Windows.Controls;

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
