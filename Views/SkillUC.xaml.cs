using SkillBase.ViewModels;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SkillBase.Views
{
    /// <summary>
    /// Interaction logic for SkillUC.xaml
    /// </summary>
    public partial class SkillUC : UserControl
    {
        public SkillUC()
        {
            InitializeComponent();
        }

        public double InitWidth;
        public double InitHeight;

        private void CardHeader_Click(object sender, RoutedEventArgs e)
        {
            SkillBox.Visibility = SkillBox.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
