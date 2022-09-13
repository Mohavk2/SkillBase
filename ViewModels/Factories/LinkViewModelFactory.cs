using SkillBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.ViewModels.Factories
{
    internal class LinkViewModelFactory
    {
        IServiceProvider _serviceProvider;
        public LinkViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public LinkViewModel Create(Link link)
        {
            return new LinkViewModel(link, _serviceProvider);
        }
    }
}
