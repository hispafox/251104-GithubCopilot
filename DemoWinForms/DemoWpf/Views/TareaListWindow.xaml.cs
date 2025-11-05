using System.Windows;
using DemoWpf.ViewModels;
using DemoWinForms.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace DemoWpf.Views
{
    /// <summary>
    /// Interaction logic for TareaListWindow.xaml
    /// </summary>
    public partial class TareaListWindow : Window
    {
        private readonly TareaListViewModel _viewModel;
   private readonly IServiceProvider _serviceProvider;

        public TareaListWindow(TareaListViewModel viewModel, IServiceProvider serviceProvider)
{
     InitializeComponent();
         _viewModel = viewModel;
       _serviceProvider = serviceProvider;
     DataContext = _viewModel;

  _viewModel.EditTareaRequested += OnEditTareaRequested;
     _viewModel.NewTareaRequested += OnNewTareaRequested;

      Loaded += TareaListWindow_Loaded;
 }

        private async void TareaListWindow_Loaded(object sender, RoutedEventArgs e)
   {
    await _viewModel.LoadTareasCommand.ExecuteAsync(null);
        }

   private void OnEditTareaRequested(object? sender, Tarea tarea)
        {
   var window = _serviceProvider.GetRequiredService<TareaEditWindow>();
  window.Owner = this;
      window.LoadTarea(tarea.Id);
      var result = window.ShowDialog();

     if (result == true)
  {
   _ = _viewModel.LoadTareasCommand.ExecuteAsync(null);
         }
        }

   private void OnNewTareaRequested(object? sender, EventArgs e)
        {
     var window = _serviceProvider.GetRequiredService<TareaEditWindow>();
window.Owner = this;
         var result = window.ShowDialog();

        if (result == true)
         {
  _ = _viewModel.LoadTareasCommand.ExecuteAsync(null);
            }
   }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
        }
    }
}
