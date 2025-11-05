using System.Windows;
using DemoWpf.ViewModels;

namespace DemoWpf.Views
{
    /// <summary>
    /// Interaction logic for TareaEditWindow.xaml
    /// </summary>
    public partial class TareaEditWindow : Window
    {
        private readonly TareaEditViewModel _viewModel;

 public TareaEditWindow(TareaEditViewModel viewModel)
        {
  InitializeComponent();
 _viewModel = viewModel;
            DataContext = _viewModel;

            _viewModel.SaveCompleted += OnSaveCompleted;
  _viewModel.CancelRequested += OnCancelRequested;
 }

        public void LoadTarea(int tareaId)
     {
            _ = _viewModel.LoadTareaAsync(tareaId);
     }

        private void OnSaveCompleted(object? sender, EventArgs e)
        {
DialogResult = true;
Close();
        }

 private void OnCancelRequested(object? sender, EventArgs e)
        {
            DialogResult = false;
            Close();
     }
    }
}
