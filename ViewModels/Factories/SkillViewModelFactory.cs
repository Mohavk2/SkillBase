using SkillBase.Models;
using System;

namespace SkillBase.ViewModels.Factories
{
    internal class SkillViewModelFactory
    {
        IServiceProvider _serviceProvider;
        public SkillViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public SkillViewModel Create(Skill skill, SkillViewModel? parentVM)
        {
            return new SkillViewModel(skill, parentVM, _serviceProvider);
        }
    }
}
