using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DemoWpf.ViewModels;
using DemoWpf.Views;
using Microsoft.Extensions.DependencyInjection;

namespace DemoWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;
        private readonly IServiceProvider _serviceProvider;
        private readonly ThemeViewModel _themeViewModel;

        public MainWindow(MainWindowViewModel viewModel, IServiceProvider serviceProvider, ThemeViewModel themeViewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _serviceProvider = serviceProvider;
            _themeViewModel = themeViewModel;
            DataContext = _viewModel;

            _viewModel.OpenTareasRequested += OnOpenTareasRequested;
            _viewModel.OpenNewTareaRequested += OnOpenNewTareaRequested;

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadEstadisticasCommand.ExecuteAsync(null);
        }

        private void OnOpenTareasRequested(object? sender, EventArgs e)
        {
            var window = _serviceProvider.GetRequiredService<TareaListWindow>();
            window.Owner = this;
            window.ShowDialog();

            // Recargar estadísticas al cerrar
            _ = _viewModel.LoadEstadisticasCommand.ExecuteAsync(null);
        }

        private void OnOpenNewTareaRequested(object? sender, EventArgs e)
        {
            var window = _serviceProvider.GetRequiredService<TareaEditWindow>();
            window.Owner = this;
            var result = window.ShowDialog();

            if (result == true)
            {
                // Recargar estadísticas
                _ = _viewModel.LoadEstadisticasCommand.ExecuteAsync(null);
            }
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_Etiquetas_Click(object sender, RoutedEventArgs e)
        {
            var window = _serviceProvider.GetRequiredService<EtiquetaManagerWindow>();
            window.Owner = this;
            window.ShowDialog();
        }

        private void MenuItem_ToggleTheme_Click(object sender, RoutedEventArgs e)
        {
            _themeViewModel.ToggleThemeCommand.Execute(null);
        }
    }
}