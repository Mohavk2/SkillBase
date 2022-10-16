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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SkillBase.Views.Skills
{
    /// <summary>
    /// Interaction logic for SkillCardFaceUC.xaml
    /// </summary>
    public partial class SkillCardFaceUC : UserControl
    {
        public SkillCardFaceUC()
        {
            InitializeComponent();
        }

        private void AddSkill_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void Header_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(this, new DataObject(DataFormats.Serializable, this), DragDropEffects.Move);
            }
        }
    }
}
