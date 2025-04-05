using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Chevron.Core.Service;
using Chevron.Core.Forms;

namespace Chevron.Core.Startup {
  public static class AppServiceExts {
    public static IServiceCollection AddServices(this IServiceCollection services) {
      services.AddSingleton<ISettingsService, SettingsService>();
      services.AddSingleton<IWatchedEventService, WatchedEventService>();
      services.AddSingleton<IVirtualDirectoryService, VirtualDirectoryService>();
      services.AddTransient<Form1>();
      services.AddScoped<TrayAppContext>();
      return services;
    }
    public static IHost BuildHost() {
      return Host.CreateDefaultBuilder()
          .ConfigureServices((_, services) => services.AddServices())
          .Build();
    }

  }
}
