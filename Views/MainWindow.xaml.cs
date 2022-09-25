using Microsoft.Extensions.DependencyInjection;
using SkillBase.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SkillBase.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if((sender as TabControl)?.SelectedContent is SkillsUC stuc)
            { 
                if(stuc.DataContext is SkillsViewModel stvm)
                {
                    //stvm.SetStateToLoading();
                    Task.Run(()=> stvm.InitSkills());
                }
            }
        }
    }
}
