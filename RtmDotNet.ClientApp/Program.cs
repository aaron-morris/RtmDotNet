using System;
using Avalonia;
using Avalonia.Logging.Serilog;
using RtmDotNet.ClientApp.ViewModels;
using RtmDotNet.ClientApp.Views;

namespace RtmDotNet.ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}
