using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkillBase.Views
{
    /// <summary>
    /// Interaction logic for SkillListUC.xaml
    /// </summary>
    public partial class SkillTreeUC : UserControl
    {
        public static readonly DependencyProperty SkillDropCommandProperty = DependencyProperty
        .Register("SkillDropCommand", typeof(ICommand), typeof(SkillTreeUC), new PropertyMetadata(null));

        public ICommand SkillDropCommand
        {
            get { return (ICommand)GetValue(SkillDropCommandProperty); }
            set { SetValue(SkillDropCommandProperty, value); }
        }

        public SkillTreeUC()
        {
            InitializeComponent();
        }
    }
}
