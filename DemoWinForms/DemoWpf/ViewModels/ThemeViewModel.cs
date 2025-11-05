using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace DemoWpf.ViewModels
{
    /// <summary>
    /// ViewModel para gestionar el tema de la aplicacion
  /// </summary>
    public partial class ThemeViewModel : ViewModelBase
    {
  [ObservableProperty]
        private bool _isDarkMode;

        public ThemeViewModel()
     {
       LoadTheme();
        }

        [RelayCommand]
  private void ToggleTheme()
        {
IsDarkMode = !IsDarkMode;
     ApplyTheme();
  SaveTheme();
        }

        private void LoadTheme()
 {
            var savedTheme = Properties.Settings.Default.IsDarkMode;
 IsDarkMode = savedTheme;
  ApplyTheme();
        }

  private void SaveTheme()
        {
Properties.Settings.Default.IsDarkMode = IsDarkMode;
         Properties.Settings.Default.Save();
        }

        private void ApplyTheme()
  {
  var app = Application.Current;
            var themeName = IsDarkMode ? "DarkTheme" : "LightTheme";

         var themeUri = new Uri($"Themes/{themeName}.xaml", UriKind.Relative);
   
            try
    {
        var themeDictionary = new ResourceDictionary { Source = themeUri };
 
       // Eliminar temas existentes
       var existingTheme = app.Resources.MergedDictionaries
       .FirstOrDefault(d => d.Source != null && 
       (d.Source.ToString().Contains("LightTheme") || 
    d.Source.ToString().Contains("DarkTheme")));

    if (existingTheme != null)
      {
         app.Resources.MergedDictionaries.Remove(existingTheme);
    }

             // Agregar nuevo tema
          app.Resources.MergedDictionaries.Add(themeDictionary);
          }
    catch (Exception)
     {
    // Si falla, usar tema por defecto
       }
        }
    }
}
