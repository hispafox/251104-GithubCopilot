using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoWinForms.Business.Services;
using DemoWinForms.Business.DTOs;

namespace DemoWpf.ViewModels
{
    /// <summary>
    /// ViewModel para la ventana principal
    /// </summary>
    public partial class MainWindowViewModel : ViewModelBase
 {
  private readonly ITareaService _tareaService;

     [ObservableProperty]
    private EstadisticasDto? _estadisticas;

    [ObservableProperty]
        private bool _isLoading;

        public MainWindowViewModel(ITareaService tareaService)
        {
            _tareaService = tareaService;
   LoadEstadisticasCommand = new AsyncRelayCommand(LoadEstadisticasAsync);
        }

public IAsyncRelayCommand LoadEstadisticasCommand { get; }

    public event EventHandler? OpenTareasRequested;
      public event EventHandler? OpenNewTareaRequested;

        private async Task LoadEstadisticasAsync()
        {
       IsLoading = true;
   try
      {
    var result = await _tareaService.GetEstadisticasAsync();
 if (result.IsSuccess && result.Value != null)
      {
    Estadisticas = result.Value;
  }
   }
      finally
  {
    IsLoading = false;
   }
        }

  [RelayCommand]
    private void OpenTareas()
    {
            OpenTareasRequested?.Invoke(this, EventArgs.Empty);
}

     [RelayCommand]
 private void OpenNewTarea()
        {
   OpenNewTareaRequested?.Invoke(this, EventArgs.Empty);
 }
    }
}
