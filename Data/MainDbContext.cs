using Microsoft.EntityFrameworkCore;
using SkillBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.Data
{
    internal class MainDbContext : DbContext
    {
        public DbSet<Skill> Skills => Set<Skill>();
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
