using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using QuickVid.Services;
using Serilog;

namespace QuickVid
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var logsDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

            Log.Logger = new LoggerConfiguration()
                        .WriteTo
                        .File(Path.Combine(logsDirectory, "log_.txt"),
                                 rollingInterval: RollingInterval.Day,
                                 retainedFileCountLimit: 5)
                        .CreateLogger();

            Dispatcher.UnhandledException += OnUnhandledException;

            var serviceProvider = ServiceProvider.Instance;

            base.OnStartup(e);
        }

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error(e.Exception, "An unhandled exception occurred");
            Log.CloseAndFlush();
            MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            throw e.Exception;
        }
    }
}