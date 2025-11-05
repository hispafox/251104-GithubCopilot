namespace DemoWinForms.Business.DTOs;

/// <summary>
/// DTO para estadísticas de tareas
/// </summary>
public class EstadisticasDto
{
    public int TotalTareas { get; set; }
    public int TareasPendientes { get; set; }
    public int TareasEnProgreso { get; set; }
    public int TareasCompletadas { get; set; }
    public int TareasCanceladas { get; set; }
    public int TareasVencidas { get; set; }
    public Dictionary<string, int> TareasPorCategoria { get; set; } = new();
    public Dictionary<string, int> TareasPorPrioridad { get; set; } = new();
}
