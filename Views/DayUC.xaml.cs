﻿using SkillBase.ViewModels;
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
    /// Interaction logic for DayUC.xaml
    /// </summary>
    public partial class DayUC : UserControl
    {
        public DayUC()
        {
            InitializeComponent();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender is DayUC duc){
                if(duc.DataContext is DayViewModel dvm)
                {
                    Task.Run(() => dvm.Init());
                }
            }
        }
    }
}
