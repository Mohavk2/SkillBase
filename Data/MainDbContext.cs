using Microsoft.EntityFrameworkCore;
using SkillBase.Models;

namespace SkillBase.Data
{
    internal class MainDbContext : DbContext
    {
        public DbSet<Skill> Skills => Set<Skill>();
        public DbSet<SkillTask> Tasks => Set<SkillTask>();
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
