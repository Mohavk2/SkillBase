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

namespace SkillBase.Views
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
            SkillBox.Visibility = Visibility.Visible;
            ShowSubskills.IsChecked = true;
            e.Handled = true;
        }

        private void ShowSubskills_Click(object sender, RoutedEventArgs e)
        {
            SkillBox.Visibility = SkillBox.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            e.Handled = true;
        }

        private void UserControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var skillName = sender as TextBox;
            if (skillName != null && skillName.Name == "SkillName" && skillName.IsFocused)
            {
                skillName.IsEnabled = true;
            }
        }

        private void EditNameToggle_Click(object sender, RoutedEventArgs e)
        {
            if (EditNameToggle.IsChecked == true)
            {
                SkillName.IsReadOnly = false;
                SkillName.Focus();
                SkillName.CaretIndex = SkillName.Text.Length;
                SkillName.CaretBrush = null;
            }
            else
            {
                SkillName.IsReadOnly = true;
            }
            e.Handled = true;
        }

        private void SkillName_LostFocus(object sender, RoutedEventArgs e)
        {
            EditNameToggle.IsChecked = false;
            SkillName.IsReadOnly = true;
            SkillName.CaretBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        private void SkillName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                EditNameToggle.IsChecked = false;
                SkillName.IsReadOnly = true;
                SkillName.CaretBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            }
        }

        private void SkillDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SkillDescription.Text))
            {
                SkillDescription.Text = "Description...";
            }
        }

        private void SkillDescription_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SkillDescription.Text == "Description...")
            {
                SkillDescription.Text = "";
            }
        }

        private void AddDayTask_Click(object sender, RoutedEventArgs e)
        {
            DayTasksToggle.IsChecked = true;
        }

        private void DayTasksToggle_Checked(object sender, RoutedEventArgs e)
        {
            DayTasks.Visibility = Visibility.Visible;
        }

        private void DayTasksToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            DayTasks.Visibility = Visibility.Collapsed;
        }

        private void Notes_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Notes.Text == "Write your notes here...")
            {
                Notes.Text = "";
            }
        }

        private void Notes_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Notes.Text))
            {
                Notes.Text = "Write your notes here...";
            }
        }

        private void CardFooter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SkillCard.Visibility = SkillCard.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            CollapsIcon.Kind = CollapsIcon.Kind == PackIconKind.ArrowCollapseDown ? PackIconKind.ArrowCollapseUp : PackIconKind.ArrowCollapseDown;
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
