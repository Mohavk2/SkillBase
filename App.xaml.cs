using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkillBase.Data;
using SkillBase.ViewModels;
using SkillBase.ViewModels.Factories;
using SkillBase.ViewModels.Schedule;
using SkillBase.ViewModels.Schedule.Day;
using SkillBase.ViewModels.Schedule.Month;
using SkillBase.ViewModels.Schedule.Week;
using SkillBase.ViewModels.Skills;
using SkillBase.Views;
using System.IO;
using System.Windows;

namespace SkillBase
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IHost AppHost { get; set; }
        IConfiguration? Configuration { get; set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((config) =>
                {
                    var dir = Directory.GetCurrentDirectory();
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    Configuration = config.Build();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    string connectionString = Configuration.GetConnectionString("Sqlite");

                    services.AddDbContext<MainDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Transient);
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<SkillsViewModel>();

                    services.AddSingleton<ScheduleViewModel>();
                    services.AddSingleton<DayViewModel>();
                    services.AddSingleton<WeekViewModel>();
                    services.AddSingleton<MonthViewModel>();

                    services.AddTransient<SkillViewModelFactory>();
                    services.AddTransient<LinkViewModelFactory>();
                    services.AddTransient<SkillTaskViewModelFactory>();

                    services.AddTransient<DayOfWeekViewModelFactory>();
                    services.AddTransient<DayOfMonthViewModelFactory>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await AppHost!.StartAsync();

            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            var mainVM = AppHost.Services.GetRequiredService<MainViewModel>();
            mainWindow.DataContext = mainVM;
            mainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}
