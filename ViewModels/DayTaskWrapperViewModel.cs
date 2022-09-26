using SkillBase.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.ViewModels
{
    internal class DayTaskWrapperViewModel : BaseViewModel
    {
        SkillTaskViewModel _skillTaskVM;
        public DayTaskWrapperViewModel(SkillTaskViewModel skillTaskVM)
        {
            _skillTaskVM = skillTaskVM;
        }

        public string SkillName => _skillTaskVM.SkillName;
        public DateTime StartDateTime => _skillTaskVM.StartDateTime;
        public DateTime EndDateTime => _skillTaskVM.EndDateTime;
    }
}
