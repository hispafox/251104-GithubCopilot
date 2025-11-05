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
    /// Convierte bool inverso a Visibility
    /// </summary>
    public class InverseBoolToVisibilityConverter : IValueConverter
  {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
     if (value is bool boolValue)
  {
    return boolValue ? Visibility.Collapsed : Visibility.Visible;
}
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
        throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convierte string vacío a Visibility.Collapsed
    /// </summary>
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
     if (value is string str)
      {
   return string.IsNullOrWhiteSpace(str) ? Visibility.Collapsed : Visibility.Visible;
 }
            return Visibility.Collapsed;
 }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convierte null a Visibility.Visible (para mostrar mensaje cuando no hay selección)
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Visible : Visibility.Collapsed;
   }

     public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Invierte un valor booleano
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
        if (value is bool boolValue)
            {
     return !boolValue;
  }
            return true;
}

     public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
     return !boolValue;
     }
          return false;
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

    /// <summary>
    /// Convierte bool a ancho del menú (abierto/cerrado)
  /// </summary>
    public class MenuWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
   if (value is bool isOpen)
            {
       return isOpen ? 250.0 : 70.0;
        }
   return 250.0;
  }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
 {
            throw new NotImplementedException();
        }
    }
}
