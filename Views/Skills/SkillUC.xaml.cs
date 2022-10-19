using MaterialDesignThemes.Wpf;
using SkillBase.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SkillBase.Views.Skills
{
    /// <summary>
    /// Interaction logic for SkillUC.xaml
    /// </summary>
    public partial class SkillUC : UserControl
    {
        public static readonly DependencyProperty SkillDropCommandProperty = DependencyProperty
            .Register("SkillDropCommand", typeof(ICommand), typeof(SkillUC), new PropertyMetadata(null));

        public ICommand SkillDropCommand
        {
            get { return (ICommand)GetValue(SkillDropCommandProperty); }
            set { SetValue(SkillDropCommandProperty, value); }
        }

        public SkillUC()
        {
            InitializeComponent();
        }

        private void AddSkill_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void AddDayTask_Click(object sender, RoutedEventArgs e)
        {
            TasksExpander.IsExpanded = true;
        }

        private void Button_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(this, new DataObject(DataFormats.Serializable, this), DragDropEffects.Move);
            }
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            var uc = e.Data.GetData(DataFormats.Serializable) as SkillUC;
            var vm = uc?.DataContext as SkillViewModel;

            if (vm != null)
            {
                SkillDropCommand?.Execute(vm);
            }
        }
    }
}
