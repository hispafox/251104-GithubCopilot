using System.Windows;
using DemoWpf.ViewModels;

namespace DemoWpf.Views
{
    /// <summary>
    /// Interaction logic for EtiquetaManagerWindow.xaml
    /// </summary>
    public partial class EtiquetaManagerWindow : Window
    {
 private readonly EtiquetaManagerViewModel _viewModel;

        public EtiquetaManagerWindow(EtiquetaManagerViewModel viewModel)
  {
     InitializeComponent();
    _viewModel = viewModel;
  DataContext = _viewModel;

      Loaded += EtiquetaManagerWindow_Loaded;
  }

 private async void EtiquetaManagerWindow_Loaded(object sender, RoutedEventArgs e)
        {
   await _viewModel.LoadEtiquetasAsync();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
        Close();
     }
    }
}
