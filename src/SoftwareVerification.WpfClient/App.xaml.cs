using System.Windows;
using SoftwareVerification.WpfClient.Services;
using SoftwareVerification.WpfClient.ViewModels;

namespace SoftwareVerification.WpfClient;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var apiClient = new ApiClient("http://localhost:8001");
        var viewModel = new MainWindowViewModel(apiClient);

        var window = new MainWindow
        {
            DataContext = viewModel
        };
        window.Show();
    }
}