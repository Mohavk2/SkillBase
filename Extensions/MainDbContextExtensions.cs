﻿using Microsoft.EntityFrameworkCore;
using SkillBase.Data;
using SkillBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillBase.Extensions
{
    internal static class MainDbContextExtensions
    {
        public static List<Skill> GetTrees(this MainDbContext context)
        {
            var skills = context.Skills.ToList();
            return skills.Where(x => x.Parent == null).ToList();
        }
        public static async Task<List<Skill>> GetTreesAsync(this MainDbContext context)
        {
            var skills = await context.Skills.Include(x=>x.DayTasks).ThenInclude(c => c.Links).ToListAsync();
            return skills.Where(x => x.Parent == null).ToList();
        }
        public static async Task<List<SkillTask>> GetDayTasksAsync(this MainDbContext context, DateTime dateTime)
        {
            var currentDayStart = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
            var nextDateTime = dateTime.AddDays(1);
            var nextDayStart = new DateTime(nextDateTime.Year, nextDateTime.Month, nextDateTime.Day , 0, 0, 0);
            var tasks = await context.Tasks.Where(x =>
            (x.StartDate < nextDayStart && x.EndDate >= currentDayStart)).ToListAsync();
            return tasks;
        }
    }
}
