using System.Windows;
using DemoWpf.ViewModels;

namespace DemoWpf;

/// <summary>
/// Ventana principal de la aplicación
/// </summary>
public partial class MainWindow : Window
{
 public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
    }
}
