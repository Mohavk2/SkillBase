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

namespace SkillBase.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        readonly IServiceProvider? _serviceProvider;
        public MainViewModel(IServiceProvider? serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        UserControl _skillTreeUC;
        public UserControl SkillTreeUC
        {
            get => _skillTreeUC;
            set {
                _skillTreeUC = value;
                RaisePropertyChanged(nameof(SkillTreeUC));
            }
        }
    }
}
