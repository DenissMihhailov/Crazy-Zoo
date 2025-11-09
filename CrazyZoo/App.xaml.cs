using CrazyZoo.Data;
using CrazyZoo.Domain;
using CrazyZoo.Logging;
using CrazyZoo.Models;
using CrazyZoo.MVVM;
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

            services.AddSingleton<ILogger, JsonLogger>();
            // services.AddSingleton<ILogger, XmlLogger>();

            services.AddSingleton<IAnimalRepository, SqlAnimalRepository>();
            // services.AddSingleton<IRepository<Animal>, InMemoryRepository<Animal>>();

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

