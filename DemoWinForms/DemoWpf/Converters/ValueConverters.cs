using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Collections;

namespace DemoWpf.Converters
{
    /// <summary>
 /// Convierte bool a Visibility
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
     public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
      if (value is bool boolValue)
     {
       return boolValue ? Visibility.Visible : Visibility.Collapsed;
}
 return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
{
throw new NotImplementedException();
        }
  }

    /// <summary>
    /// Convierte el modo de edición al título correspondiente
    /// </summary>
  public class EditModeTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
   {
     if (value is bool isEditMode)
     {
      return isEditMode ? "Editar Tarea" : "Nueva Tarea";
}
return "Nueva Tarea";
    }

   public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
     throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convierte el modo de edición al texto del botón
    /// </summary>
    public class EditModeButtonConverter : IValueConverter
    {
   public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
     {
       if (value is bool isEditMode)
     {
             return isEditMode ? "Actualizar" : "Agregar";
            }
            return "Agregar";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
         throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convierte una colección de etiquetas a su visibilidad
  /// </summary>
    public class EtiquetasVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection collection)
            {
        return collection.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
       return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
     {
   throw new NotImplementedException();
        }
    }
}
