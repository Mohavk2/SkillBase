using SkillBase.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.ViewModels.Factories
{
    internal class SkillTaskViewModelFactory
    {
        IServiceProvider _serviceProvider;
        public SkillTaskViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public SkillTaskViewModel Create(SkillTask task)
        {
            return new SkillTaskViewModel(task, _serviceProvider);
        }
    }
}
