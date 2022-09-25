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
    /// Interaction logic for DayTaskUC.xaml
    /// </summary>
    public partial class SkillTaskUC : UserControl
    {
        public SkillTaskUC()
        {
            InitializeComponent();
        }

        private void AddLink_Click(object sender, RoutedEventArgs e)
        {
            LinksExpander.IsExpanded = true;
        }

        private void LinksToggle_Checked(object sender, RoutedEventArgs e)
        {
            SkillLinks.Visibility = Visibility.Visible;
        }

        private void LinksToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            SkillLinks.Visibility = Visibility.Collapsed;
        }

    }
}
