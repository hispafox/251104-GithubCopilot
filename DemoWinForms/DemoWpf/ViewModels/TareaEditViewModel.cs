using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoWinForms.Business.Services;
using DemoWinForms.Domain.Entities;
using DemoWinForms.Domain.Enums;
using System.Windows;
using DemoWinForms.Data.Repositories;

namespace DemoWpf.ViewModels
{
    /// <summary>
    /// ViewModel para editar/crear tareas
    /// </summary>
    public partial class TareaEditViewModel : ViewModelBase
{
        private readonly ITareaService _tareaService;
        private readonly IEtiquetaRepository _etiquetaRepository;
    private int? _tareaId;

[ObservableProperty]
 private string _titulo = string.Empty;

        [ObservableProperty]
        private string _descripcion = string.Empty;

    [ObservableProperty]
      private DateTime _fechaVencimiento = DateTime.Today.AddDays(7);

        [ObservableProperty]
     private EstadoTarea _estado = EstadoTarea.Pendiente;

        [ObservableProperty]
        private PrioridadTarea _prioridad = PrioridadTarea.Media;

        [ObservableProperty]
        private bool _isLoading;

  [ObservableProperty]
        private bool _isEditMode;

 public ObservableCollection<EstadoTarea> Estados { get; }
  public ObservableCollection<PrioridadTarea> Prioridades { get; }
        public ObservableCollection<EtiquetaSeleccionable> EtiquetasDisponibles { get; }

        public TareaEditViewModel(ITareaService tareaService, IEtiquetaRepository etiquetaRepository)
  {
            _tareaService = tareaService;
        _etiquetaRepository = etiquetaRepository;

     Estados = new ObservableCollection<EstadoTarea>(Enum.GetValues<EstadoTarea>());
            Prioridades = new ObservableCollection<PrioridadTarea>(Enum.GetValues<PrioridadTarea>());
   EtiquetasDisponibles = new ObservableCollection<EtiquetaSeleccionable>();
 
 _ = LoadEtiquetasAsync();
      }

        private async Task LoadEtiquetasAsync()
        {
            try
    {
           var etiquetas = await _etiquetaRepository.GetAllAsync();
      EtiquetasDisponibles.Clear();
       foreach (var etiqueta in etiquetas)
          {
        EtiquetasDisponibles.Add(new EtiquetaSeleccionable(etiqueta));
  }
        }
   catch (Exception ex)
            {
                // Log error
       System.Diagnostics.Debug.WriteLine($"Error al cargar etiquetas: {ex.Message}");
            }
      }

        public event EventHandler? SaveCompleted;
      public event EventHandler? CancelRequested;

        public async Task LoadTareaAsync(int tareaId)
        {
            IsLoading = true;
            IsEditMode = true;
         _tareaId = tareaId;

        try
    {
             var result = await _tareaService.GetByIdAsync(tareaId);
     if (result.IsSuccess && result.Value != null)
       {
   var tarea = result.Value;
            Titulo = tarea.Titulo;
        Descripcion = tarea.Descripcion ?? string.Empty;
    FechaVencimiento = tarea.FechaVencimiento ?? DateTime.Today.AddDays(7);
              Estado = tarea.Estado;
      Prioridad = tarea.Prioridad;
     
          // Marcar etiquetas seleccionadas
      var etiquetasIds = tarea.TareaEtiquetas.Select(te => te.EtiquetaId).ToHashSet();
       foreach (var etiqueta in EtiquetasDisponibles)
        {
       etiqueta.IsSelected = etiquetasIds.Contains(etiqueta.Etiqueta.Id);
   }
     }
      }
    finally
   {
  IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task Save()
        {
       if (!ValidateInput())
                return;

    IsLoading = true;
     try
      {
      if (IsEditMode && _tareaId.HasValue)
   {
         // Actualizar tarea existente
        var getResult = await _tareaService.GetByIdAsync(_tareaId.Value);
        if (getResult.IsSuccess && getResult.Value != null)
  {
          var tarea = getResult.Value;
  tarea.Titulo = Titulo;
    tarea.Descripcion = Descripcion;
     tarea.FechaVencimiento = FechaVencimiento;
      tarea.Estado = Estado;
 tarea.Prioridad = Prioridad;
           
   // Actualizar etiquetas
           tarea.TareaEtiquetas.Clear();
           foreach (var etiqueta in EtiquetasDisponibles.Where(e => e.IsSelected))
  {
         tarea.TareaEtiquetas.Add(new TareaEtiqueta
  {
         TareaId = tarea.Id,
  EtiquetaId = etiqueta.Etiqueta.Id
       });
       }

          var updateResult = await _tareaService.ActualizarTareaAsync(tarea);
         if (updateResult.IsSuccess)
  {
            SaveCompleted?.Invoke(this, EventArgs.Empty);
                }
                 else
    {
         MessageBox.Show(
               updateResult.Error ?? "Error al actualizar la tarea",
      "Error",
  MessageBoxButton.OK,
       MessageBoxImage.Error);
      }
   }
        }
         else
            {
          // Crear nueva tarea
        var tarea = new Tarea
         {
        Titulo = Titulo,
            Descripcion = Descripcion,
          FechaVencimiento = FechaVencimiento,
  Estado = Estado,
    Prioridad = Prioridad,
    FechaCreacion = DateTime.Now,
 UsuarioId = 1 // Usuario por defecto
    };
        
          // Asignar etiquetas
          foreach (var etiqueta in EtiquetasDisponibles.Where(e => e.IsSelected))
{
     tarea.TareaEtiquetas.Add(new TareaEtiqueta
       {
  EtiquetaId = etiqueta.Etiqueta.Id
       });
  }

       var createResult = await _tareaService.CrearTareaAsync(tarea);
      if (createResult.IsSuccess)
             {
 SaveCompleted?.Invoke(this, EventArgs.Empty);
 }
      else
        {
 MessageBox.Show(
           createResult.Error ?? "Error al crear la tarea",
           "Error",
    MessageBoxButton.OK,
   MessageBoxImage.Error);
             }
      }
            }
            finally
        {
     IsLoading = false;
  }
        }

        [RelayCommand]
        private void Cancel()
  {
  CancelRequested?.Invoke(this, EventArgs.Empty);
        }

        private bool ValidateInput()
        {
     if (string.IsNullOrWhiteSpace(Titulo))
   {
   MessageBox.Show(
 "El título es obligatorio",
  "Validación",
    MessageBoxButton.OK,
    MessageBoxImage.Warning);
  return false;
      }

    if (Titulo.Length > 200)
   {
        MessageBox.Show(
        "El título no puede exceder los 200 caracteres",
     "Validación",
   MessageBoxButton.OK,
   MessageBoxImage.Warning);
         return false;
         }

  return true;
      }
    }

    /// <summary>
    /// Clase auxiliar para mostrar etiquetas con estado de selección
    /// </summary>
    public partial class EtiquetaSeleccionable : ObservableObject
    {
        public Etiqueta Etiqueta { get; }

        [ObservableProperty]
   private bool _isSelected;

        public EtiquetaSeleccionable(Etiqueta etiqueta)
        {
          Etiqueta = etiqueta;
     }
    }
}
