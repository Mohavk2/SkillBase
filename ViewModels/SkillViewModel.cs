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
    internal class SkillViewModel : BaseViewModel
    {
        public SkillViewModel(Skill skill)
        {
            Name = skill.Name;
            Description = skill.Description ?? "";
            Notes = skill.Notes ?? "";
            var chilldren = skill.Children ?? new();
            foreach (var child in chilldren)
            {
                SkillVMs.Add(new(child));
            }
        }
        public string _name;
        public string Name
        {
            get { return _name; }
            set 
            { 
                _name = value; 
                RaisePropertyChanged(nameof(Name));
            }
        }
        public string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }
        public string _notes;
        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                RaisePropertyChanged(nameof(Notes));
            }
        }
        public string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                RaisePropertyChanged(nameof(ImagePath));
            }
        }
        public ObservableCollection<ReferenceUrl> _references = new();
        public ObservableCollection<ReferenceUrl> References
        {
            get { return _references; }
            set
            {
                _references = value;
                RaisePropertyChanged(nameof(References));
            }
        }
        public bool _isCompleted;
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                _isCompleted = value;
                RaisePropertyChanged(nameof(IsCompleted));
            }
        }
        public ObservableCollection<SkillViewModel> _skillVMs = new();
        public ObservableCollection<SkillViewModel> SkillVMs
        {
            get => _skillVMs;
            set
            {
                _skillVMs = value;
                RaisePropertyChanged(nameof(SkillVMs));
            }
        }
    }
}
