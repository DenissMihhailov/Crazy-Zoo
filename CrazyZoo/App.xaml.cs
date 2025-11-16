using CrazyZoo.Data;
using CrazyZoo.Domain;
using CrazyZoo.Logging;
using CrazyZoo.Models;
using CrazyZoo.MVVM;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace CrazyZoo
{
    public partial class App : Application
    {
        public static ServiceProvider Services { get; private set; } = null!;

        public App()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ILogger, XmlLogger>();

            services.AddDbContext<ZooContext>(opt =>
                opt.UseSqlite("Data Source=zoo.db"));

            services.AddScoped<IAnimalRepository, EfAnimalRepository>();

            services.AddSingleton<Enclosure<Animal>>();
            services.AddSingleton<MainViewModel>();

            Services = services.BuildServiceProvider();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainViewModel>()
            };
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}

