using SkillBase.Models;
using SkillBase.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.ViewModels
{
    internal class SkillTreeViewModel : BaseViewModel
    {
        public SkillTreeViewModel(List<Skill> skills)
        {
            foreach(Skill skill in skills)
            {
                SkillVMs.Add(new(skill));
            }
        }
        public ObservableCollection<SkillViewModel> SkillVMs { get; set; } = new();
    }
}
