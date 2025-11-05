using DemoWinForms.Domain.Entities;
using System.Text;
using System.Text.Json;
using System.IO;

namespace DemoWpf.Services
{
    /// <summary>
    /// Servicio para exportar datos
    /// </summary>
    public interface IExportService
    {
        Task ExportToCsvAsync(IEnumerable<Tarea> tareas, string filePath);
        Task ExportToJsonAsync(IEnumerable<Tarea> tareas, string filePath);
 }

    public class ExportService : IExportService
    {
    public async Task ExportToCsvAsync(IEnumerable<Tarea> tareas, string filePath)
        {
       var sb = new StringBuilder();
          
            // Encabezados
            sb.AppendLine("Id,Titulo,Descripcion,Estado,Prioridad,FechaCreacion,FechaVencimiento,Categoria");

    // Datos
   foreach (var tarea in tareas)
  {
            sb.AppendLine($"{tarea.Id}," +
      $"\"{EscapeCsv(tarea.Titulo)}\"," +
      $"\"{EscapeCsv(tarea.Descripcion ?? "")}\"," +
          $"{tarea.Estado}," +
   $"{tarea.Prioridad}," +
          $"{tarea.FechaCreacion:yyyy-MM-dd}," +
  $"{tarea.FechaVencimiento:yyyy-MM-dd}," +
        $"\"{EscapeCsv(tarea.Categoria)}\"");
            }

          await File.WriteAllTextAsync(filePath, sb.ToString(), Encoding.UTF8);
      }

        public async Task ExportToJsonAsync(IEnumerable<Tarea> tareas, string filePath)
        {
    var options = new JsonSerializerOptions
   {
  WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

 var json = JsonSerializer.Serialize(tareas, options);
    await File.WriteAllTextAsync(filePath, json, Encoding.UTF8);
        }

   private string EscapeCsv(string? value)
        {
 if (string.IsNullOrEmpty(value))
     return string.Empty;

            return value.Replace("\"", "\"\"");
    }
    }
}
