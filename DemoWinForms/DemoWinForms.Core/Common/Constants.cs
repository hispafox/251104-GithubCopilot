namespace DemoWinForms.Common;

/// <summary>
/// Constantes de la aplicación
/// </summary>
public static class Constants
{
  public static class Categorias
    {
        public const string Trabajo = "Trabajo";
        public const string Personal = "Personal";
     public const string Estudio = "Estudio";
        public const string Otro = "Otro";

        public static readonly string[] Todas = { Trabajo, Personal, Estudio, Otro };
    }

    public static class Colores
    {
        public const string PrioridadCritica = "#DC3545";
        public const string PrioridadAlta = "#FFC107";
        public const string PrioridadMedia = "#17A2B8";
  public const string PrioridadBaja = "#28A745";
    }

  public static class Mensajes
    {
   public const string TareaCreadaExito = "Tarea creada exitosamente";
    public const string TareaActualizadaExito = "Tarea actualizada exitosamente";
        public const string TareaEliminadaExito = "Tarea eliminada exitosamente";
        public const string ErrorCrearTarea = "Error al crear la tarea";
        public const string ErrorActualizarTarea = "Error al actualizar la tarea";
        public const string ErrorEliminarTarea = "Error al eliminar la tarea";
        public const string ConfirmarEliminacion = "¿Está seguro de eliminar esta tarea?";
 }
}
