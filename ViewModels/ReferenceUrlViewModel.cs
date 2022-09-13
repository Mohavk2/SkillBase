using Microsoft.Extensions.DependencyInjection;
using SkillBase.Data;
using SkillBase.Models;
using SkillBase.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SkillBase.ViewModels
{
    delegate void UpdateReferenceHandler(ReferenceUrlViewModel reference);
    delegate void DeleteReferenceHandler(ReferenceUrlViewModel reference);

    internal class ReferenceUrlViewModel : BaseViewModel
    {
        public event DeleteReferenceHandler? OnDelete;

        IServiceProvider _serviceProvider;

        public ReferenceUrlViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public ReferenceUrlViewModel(ReferenceUrl link, IServiceProvider serviceProvider) : this(serviceProvider)
        {
            if (link != null)
            {
                Id = link.Id;
                Name = link.Name ?? "";
                Url = link.Url ?? "";
            }
        }

        public ICommand Delete
        {
            get => new UICommand((parameter) =>
            {
                using var dbContext = _serviceProvider.GetRequiredService<MainDbContext>();
                var link = dbContext.Find<ReferenceUrl>(Id);
                if(link != null)
                {
                    dbContext.Remove(link);
                    dbContext.SaveChanges();
                    OnDelete?.Invoke(this);
                }
            });
        }

        public int Id { get; private set; } = 0;

        string _name = "Google";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                Update(link => link.Name = _name);
                RaisePropertyChanged(nameof(Name));
            }
        }

        string _url = "https://google.com";
        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                Update(link => link.Url = _url);
                RaisePropertyChanged(nameof(Url));
            }
        }

        void Update(Action<ReferenceUrl> setter)
        {
            using var db = _serviceProvider.GetRequiredService<MainDbContext>();
            var entity = db.Find<ReferenceUrl>(Id);
            if(entity != null)
            {
                setter(entity);
                db.Update<ReferenceUrl>(entity);
                db.SaveChanges();
            }
        }
    }
}
