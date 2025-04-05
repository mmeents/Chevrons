using Chevron.Core.Startup;
using Microsoft.Extensions.DependencyInjection;

namespace Chevron.Coil
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            using var host = AppServiceExts.BuildHost(); // Var host needs to live for lifetime of app, useing
            Application.Run(host.Services.GetRequiredService<TrayAppContext>());
        }
    }
}