using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace TextProcess.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddHttpClient("local", (ctx, httpClient) =>
            {
                httpClient.BaseAddress = new Uri("https://localhost:5001/api/");
                httpClient.Timeout = TimeSpan.FromSeconds(10);
            });

            services.AddTransient(typeof(MainWindow));
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();

            mainWindow.Show();
        }
    }
}
