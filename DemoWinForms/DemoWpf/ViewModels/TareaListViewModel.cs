using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoWinForms.Business.Services;
using DemoWinForms.Domain.Entities;
using DemoWinForms.Domain.Enums;
using DemoWinForms.Business.DTOs;
using System.Windows;
using DemoWpf.Services;
using Microsoft.Win32;

namespace DemoWpf.ViewModels
{
    /// <summary>
    /// ViewModel para la lista de tareas
    /// </summary>
public partial class TareaListViewModel : ViewModelBase
    {
     private readonly ITareaService _tareaService;
        private readonly IExportService _exportService;

  [ObservableProperty]
     private ObservableCollection<Tarea> _tareas = new();

  [ObservableProperty]
      private Tarea? _selectedTarea;

    [ObservableProperty]
      private bool _isLoading;

[ObservableProperty]
  private string _searchText = string.Empty;

  [ObservableProperty]
 private EstadoTarea? _filtroEstado;

    [ObservableProperty]
        private PrioridadTarea? _filtroPrioridad;

public ObservableCollection<EstadoTarea> Estados { get; }
  public ObservableCollection<PrioridadTarea> Prioridades { get; }

   public TareaListViewModel(ITareaService tareaService, IExportService exportService)
      {
     _tareaService = tareaService;
      _exportService = exportService;
    
     Estados = new ObservableCollection<EstadoTarea>(Enum.GetValues<EstadoTarea>());
  Prioridades = new ObservableCollection<PrioridadTarea>(Enum.GetValues<PrioridadTarea>());

  LoadTareasCommand = new AsyncRelayCommand(LoadTareasAsync);
  }

public IAsyncRelayCommand LoadTareasCommand { get; }

 public event EventHandler<Tarea>? EditTareaRequested;
     public event EventHandler? NewTareaRequested;

  private async Task LoadTareasAsync()
        {
   IsLoading = true;
 try
  {
   var filtro = new FiltroTareasDto
 {
    BusquedaTexto = string.IsNullOrWhiteSpace(SearchText) ? null : SearchText,
  Estado = FiltroEstado,
 Prioridad = FiltroPrioridad
   };

  var result = await _tareaService.GetByFiltrosAsync(filtro);
    if (result.IsSuccess && result.Value != null)
   {
   Tareas.Clear();
      foreach (var tarea in result.Value)
         {
      Tareas.Add(tarea);
  }
  }
    }
 finally
      {
     IsLoading = false;
          }
    }

   [RelayCommand]
private void NewTarea()
   {
      NewTareaRequested?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand]
    private void EditTarea(Tarea? tarea)
 {
 if (tarea != null)
     {
      EditTareaRequested?.Invoke(this, tarea);
   }
 }

    [RelayCommand]
   private async Task DeleteTarea(Tarea? tarea)
 {
if (tarea == null) return;

      var result = MessageBox.Show(
     $"¿Está seguro de eliminar la tarea '{tarea.Titulo}'?",
     "Confirmar eliminacion",
  MessageBoxButton.YesNo,
      MessageBoxImage.Question);

if (result == MessageBoxResult.Yes)
 {
 IsLoading = true;
       try
  {
       var deleteResult = await _tareaService.EliminarTareaAsync(tarea.Id);
      if (deleteResult.IsSuccess)
        {
    await LoadTareasAsync();
      }
 else
    {
MessageBox.Show(deleteResult.Error ?? "Error al eliminar la tarea", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
   }
 }
       finally
    {
 IsLoading = false;
       }
        }
  }

  [RelayCommand]
      private async Task Search()
  {
    await LoadTareasAsync();
      }

    [RelayCommand]
private async Task ClearFilters()
        {
   SearchText = string.Empty;
      FiltroEstado = null;
      FiltroPrioridad = null;
     await LoadTareasAsync();
        }

        [RelayCommand]
        private async Task ExportToCsv()
  {
       try
 {
     var dialog = new SaveFileDialog
   {
    Filter = "CSV files (*.csv)|*.csv",
  DefaultExt = ".csv",
     FileName = $"tareas_{DateTime.Now:yyyyMMdd}.csv"
 };

    if (dialog.ShowDialog() == true)
       {
 IsLoading = true;
      await _exportService.ExportToCsvAsync(Tareas, dialog.FileName);
        MessageBox.Show("Exportacion completada", "Exito", 
  MessageBoxButton.OK, MessageBoxImage.Information);
   }
            }
          catch (Exception ex)
{
    MessageBox.Show($"Error al exportar: {ex.Message}", "Error", 
       MessageBoxButton.OK, MessageBoxImage.Error);
 }
        finally
   {
        IsLoading = false;
      }
        }

        [RelayCommand]
   private async Task ExportToJson()
 {
  try
  {
     var dialog = new SaveFileDialog
 {
   Filter = "JSON files (*.json)|*.json",
          DefaultExt = ".json",
        FileName = $"tareas_{DateTime.Now:yyyyMMdd}.json"
     };

       if (dialog.ShowDialog() == true)
    {
        IsLoading = true;
    await _exportService.ExportToJsonAsync(Tareas, dialog.FileName);
  MessageBox.Show("Exportacion completada", "Exito", 
     MessageBoxButton.OK, MessageBoxImage.Information);
   }
   }
        catch (Exception ex)
   {
  MessageBox.Show($"Error al exportar: {ex.Message}", "Error", 
       MessageBoxButton.OK, MessageBoxImage.Error);
   }
        finally
            {
        IsLoading = false;
     }
        }
    }
}
