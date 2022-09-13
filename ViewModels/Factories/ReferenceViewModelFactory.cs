using SkillBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.ViewModels.Factories
{
    internal class ReferenceViewModelFactory
    {
        IServiceProvider _serviceProvider;
        public ReferenceViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public ReferenceUrlViewModel Create(ReferenceUrl reference)
        {
            return new ReferenceUrlViewModel(reference, _serviceProvider);
        }
    }
}
