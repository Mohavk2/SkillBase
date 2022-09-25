using SkillBase.Data;
using SkillBase.Models;
using SkillBase.ViewModels.Common;
using SkillBase.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SkillBase.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;
using SkillBase.ViewModels.Factories;
using System.Windows.Threading;

namespace SkillBase.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        readonly IServiceProvider _serviceProvider;
        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            SkillsUC.DataContext = _serviceProvider.GetRequiredService<SkillsViewModel>();
        }

        public UserControl ScheduleUC { get; set; } = new ScheduleUC();
        public UserControl SkillsUC { get; set; } = new SkillsUC();
    }
}
