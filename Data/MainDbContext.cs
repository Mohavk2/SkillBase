using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SkillBase.Models;

namespace SkillBase.Data
{
    internal class MainDbContext : DbContext
    {
        public DbSet<Skill> Skills => Set<Skill>();
        public DbSet<SkillTask> Tasks => Set<SkillTask>();
        public DbSet<Link> Links => Set<Link>();

        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>()
                .HasOne(c => c.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
