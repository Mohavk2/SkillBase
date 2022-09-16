using MaterialDesignThemes.Wpf;
using System;
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

        private void AddLink_Click(object sender, RoutedEventArgs e)
        {
            LinksToggle.IsChecked = true;
        }


        private void LinksToggle_Checked(object sender, RoutedEventArgs e)
        {
            SkillLinks.Visibility = Visibility.Visible;
        }

        private void LinksToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            SkillLinks.Visibility = Visibility.Collapsed;
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
    }
}
