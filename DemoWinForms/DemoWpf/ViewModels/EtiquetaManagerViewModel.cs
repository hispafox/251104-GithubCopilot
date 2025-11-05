using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoWinForms.Business.Services;
using DemoWinForms.Domain.Entities;
using DemoWinForms.Data.Repositories;
using System.Windows;

namespace DemoWpf.ViewModels
{
    /// <summary>
    /// ViewModel para gestionar etiquetas
  /// </summary>
    public partial class EtiquetaManagerViewModel : ViewModelBase
    {
        private readonly IEtiquetaRepository _etiquetaRepository;

        [ObservableProperty]
        private ObservableCollection<Etiqueta> _etiquetas = new();

  [ObservableProperty]
        private Etiqueta? _selectedEtiqueta;

        [ObservableProperty]
 private bool _isLoading;

        [ObservableProperty]
        private string _nuevoNombre = string.Empty;

      [ObservableProperty]
        private string _nuevoColor = "#808080";

  [ObservableProperty]
private bool _isEditing;

        private int? _editingEtiquetaId;

        public EtiquetaManagerViewModel(IEtiquetaRepository etiquetaRepository)
        {
   _etiquetaRepository = etiquetaRepository;
        }

        public async Task LoadEtiquetasAsync()
      {
        IsLoading = true;
            try
     {
        var etiquetas = await _etiquetaRepository.GetAllAsync();
      Etiquetas.Clear();
       foreach (var etiqueta in etiquetas)
    {
       Etiquetas.Add(etiqueta);
     }
     }
            finally
      {
              IsLoading = false;
            }
 }

 [RelayCommand]
        private void StartEdit(Etiqueta? etiqueta)
        {
            if (etiqueta == null) return;

            IsEditing = true;
       _editingEtiquetaId = etiqueta.Id;
     NuevoNombre = etiqueta.Nombre;
NuevoColor = etiqueta.ColorHex ?? "#808080";
        }

      [RelayCommand]
        private void CancelEdit()
     {
        IsEditing = false;
  _editingEtiquetaId = null;
            NuevoNombre = string.Empty;
            NuevoColor = "#808080";
        }

     [RelayCommand]
        private async Task SaveEtiqueta()
    {
            if (string.IsNullOrWhiteSpace(NuevoNombre))
   {
     MessageBox.Show("El nombre es obligatorio", "Validacion", 
      MessageBoxButton.OK, MessageBoxImage.Warning);
          return;
     }

            IsLoading = true;
  try
            {
       if (IsEditing && _editingEtiquetaId.HasValue)
      {
          var etiqueta = await _etiquetaRepository.GetByIdAsync(_editingEtiquetaId.Value);
    if (etiqueta != null)
  {
                etiqueta.Nombre = NuevoNombre;
       etiqueta.ColorHex = NuevoColor;
        await _etiquetaRepository.UpdateAsync(etiqueta);
                }
      }
  else
          {
    var nuevaEtiqueta = new Etiqueta
         {
     Nombre = NuevoNombre,
             ColorHex = NuevoColor
     };
        await _etiquetaRepository.AddAsync(nuevaEtiqueta);
     }

         await LoadEtiquetasAsync();
                CancelEdit();
            }
      catch (Exception ex)
  {
           MessageBox.Show($"Error al guardar: {ex.Message}", "Error", 
    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
  {
       IsLoading = false;
      }
        }

        [RelayCommand]
        private async Task DeleteEtiqueta(Etiqueta? etiqueta)
        {
            if (etiqueta == null) return;

       var result = MessageBox.Show(
  $"¿Eliminar la etiqueta '{etiqueta.Nombre}'?",
    "Confirmar eliminacion",
        MessageBoxButton.YesNo,
    MessageBoxImage.Question);

      if (result == MessageBoxResult.Yes)
 {
             IsLoading = true;
         try
     {
await _etiquetaRepository.DeleteAsync(etiqueta.Id);
    await LoadEtiquetasAsync();
          }
 finally
          {
        IsLoading = false;
   }
  }
        }
    }
}
