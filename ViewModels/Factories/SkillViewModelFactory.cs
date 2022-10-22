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
        public SkillViewModel Create(Skill skill)
        {
            return new SkillViewModel(skill, _serviceProvider);
        }
    }
}
