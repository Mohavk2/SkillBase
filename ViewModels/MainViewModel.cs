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

namespace SkillBase.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        readonly IServiceProvider? _serviceProvider;
        public MainViewModel(IServiceProvider? serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        bool _isDataLoaded = false;
        public async void InitAppData()
        {
            _skillTreeVM = _serviceProvider?.GetRequiredService<SkillTreeViewModel>();
            await _skillTreeVM.InitSkillTree();
            SkillTreeUC = new SkillTreeUC();
            SkillTreeUC.DataContext = _skillTreeVM;

            _isDataLoaded = true;
        }

        UserControl _skillTreeUC = new LoadingUC();
        public UserControl SkillTreeUC
        {
            get => _skillTreeUC;
            set
            {
                _skillTreeUC = value;
                RaisePropertyChanged(nameof(SkillTreeUC));
            }
        }

        SkillTreeViewModel? _skillTreeVM;

        public ICommand AddSkill => new UICommand((parameter) =>
        {
            _skillTreeVM?.CreateSkill();
        }, (parameter) => _isDataLoaded);
    }
}
