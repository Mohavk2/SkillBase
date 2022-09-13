using Microsoft.Extensions.DependencyInjection;
using SkillBase.Data;
using SkillBase.Models;
using SkillBase.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkillBase.ViewModels
{
    delegate void UpdateReferenceHandler(ReferenceUrlViewModel reference);
    delegate void DeleteReferenceHandler(ReferenceUrlViewModel reference);

    internal class ReferenceUrlViewModel
    {
        public event DeleteReferenceHandler? OnDelete;

        ReferenceUrl _reference;

        IServiceProvider _serviceProvider;

        public ReferenceUrlViewModel(IServiceProvider serviceProvider)
        {
            _reference = new();
            _serviceProvider = serviceProvider;
        }
        public ReferenceUrlViewModel(ReferenceUrl reference, IServiceProvider serviceProvider)
        {
            _reference = reference;
            _serviceProvider = serviceProvider;
        }

        public ICommand Delete
        {
            get => new UICommand((parameter) =>
            {
                using var dbContext = _serviceProvider.GetRequiredService<MainDbContext>();
                dbContext.Remove(_reference);
                dbContext.SaveChanges();
                OnDelete?.Invoke(this);
            });
        }

        public string Name
        {
            get => _reference.Name ?? string.Empty;
            set
            {
                _reference.Name = value;
            }
        }
        public string Url
        {
            get => _reference.Url ?? string.Empty;
            set
            {
                _reference.Url = value;
            }
        }
    }
}
