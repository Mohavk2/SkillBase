using MaterialDesignThemes.Wpf;
using SkillBase.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Connector.xaml
    /// </summary>
    public partial class Connector : UserControl
    {
        public static readonly DependencyProperty ChildrenProperty = DependencyProperty
        .Register("Children", typeof(ObservableCollection<SkillViewModel>), typeof(Connector));

        internal ObservableCollection<SkillViewModel> Children
        {
            get { return (ObservableCollection<SkillViewModel>)GetValue(ChildrenProperty); }
            set { SetValue(ChildrenProperty, value); }
        }

        public Connector()
        {
            InitializeComponent();
        }

        private void SkillConnector_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is Connector c)
            {
                var fullH = ChildConnectors.ActualHeight;
                if(Children != null && Children.Count != 0)
                {
                    if (Children.Count > 1)
                    {
                        var topOffset = Children.First().Height / 2 + 3;
                        var botOffset = Children.Last().Height / 2 + 3;
                        VerticalLink.VerticalAlignment = VerticalAlignment.Top;
                        VerticalLink.Margin = new Thickness(0, topOffset, 0, 0);
                        VerticalLink.Height = fullH - topOffset - botOffset;
                    }
                    else if (Children.Count == 1)
                    {
                        VerticalLink.VerticalAlignment = VerticalAlignment.Center;
                        VerticalLink.Margin = new Thickness(0, 0, 0, 0);
                        VerticalLink.Height = 4;
                    }
                    NodeLink.Visibility = Visibility.Visible;
                    VerticalLink.Visibility = Visibility.Visible;
                }
                else
                {
                    NodeLink.Visibility = Visibility.Collapsed;
                    VerticalLink.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
