using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoWinForms.Business.Services;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace DemoWpf.ViewModels;

public partial class TaskStatisticsViewModel : ViewModelBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger<TaskStatisticsViewModel> _logger;

    [ObservableProperty]
    private int _totalLists;

    [ObservableProperty]
    private int _totalTasks;

    [ObservableProperty]
    private int _completedTasks;

    [ObservableProperty]
  private int _pendingTasks;

    [ObservableProperty]
    private int _inProgressTasks;

    [ObservableProperty]
    private int _overdueTasks;

    [ObservableProperty]
    private double _completionPercentage;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _statusMessage = "Listo";

    [ObservableProperty]
    private ObservableCollection<PriorityStatistic> _priorityStats = new();

    public string CompletionColor => CompletionPercentage >= 80 ? "#5CB85C" : 
                CompletionPercentage >= 50 ? "#F0AD4E" : 
  CompletionPercentage >= 25 ? "#5BC0DE" : "#D9534F";

 public string ProgressMessage => TotalTasks == 0 ? "No hay tareas" :
    CompletionPercentage == 100 ? "Todas las tareas completadas" :
          CompletionPercentage >= 75 ? "Excelente progreso" :
        CompletionPercentage >= 50 ? "Buen avance" :
            CompletionPercentage >= 25 ? "Sigue asi" : "Empecemos";

    public TaskStatisticsViewModel(ITaskService taskService, ILogger<TaskStatisticsViewModel> logger)
    {
        _taskService = taskService;
      _logger = logger;
    }

    [RelayCommand]
    public async Task LoadStatisticsAsync()
    {
        try
      {
 IsLoading = true;
      StatusMessage = "Cargando estadisticas...";

        var result = await _taskService.GetStatisticsAsync();
  if (result.IsSuccess)
            {
                var stats = result.Value!;
                TotalLists = stats.GetValueOrDefault("TotalLists", 0);
         TotalTasks = stats.GetValueOrDefault("TotalTasks", 0);
    CompletedTasks = stats.GetValueOrDefault("CompletedTasks", 0);
         PendingTasks = stats.GetValueOrDefault("PendingTasks", 0);
        InProgressTasks = stats.GetValueOrDefault("InProgressTasks", 0);
  OverdueTasks = stats.GetValueOrDefault("OverdueTasks", 0);

    CompletionPercentage = TotalTasks > 0 ? (CompletedTasks * 100.0 / TotalTasks) : 0;

    await LoadPriorityStatisticsAsync();

     StatusMessage = "Estadisticas actualizadas";
                OnPropertyChanged(nameof(CompletionColor));
    OnPropertyChanged(nameof(ProgressMessage));
            }
   else
     {
      StatusMessage = $"Error: {result.Error}";
       }
        }
   catch (Exception ex)
   {
        _logger.LogError(ex, "Error al cargar estadisticas");
   StatusMessage = "Error al cargar estadisticas";
        }
        finally
        {
     IsLoading = false;
        }
    }

    private async Task LoadPriorityStatisticsAsync()
    {
        try
        {
var listsResult = await _taskService.GetAllTaskListsAsync();
      if (listsResult.IsSuccess)
      {
       var allTasks = listsResult.Value!.SelectMany(l => l.Tasks).ToList();
              var highPriority = allTasks.Count(t => t.Priority == DemoWinForms.Domain.Enums.TaskListPriority.High);
         var mediumPriority = allTasks.Count(t => t.Priority == DemoWinForms.Domain.Enums.TaskListPriority.Medium);
                var lowPriority = allTasks.Count(t => t.Priority == DemoWinForms.Domain.Enums.TaskListPriority.Low);

     PriorityStats.Clear();
     PriorityStats.Add(new PriorityStatistic("Alta", highPriority, "#D9534F"));
 PriorityStats.Add(new PriorityStatistic("Media", mediumPriority, "#F0AD4E"));
         PriorityStats.Add(new PriorityStatistic("Baja", lowPriority, "#5CB85C"));
  }
     }
    catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar estadisticas por prioridad");
        }
    }
}

public class PriorityStatistic
{
    public string Name { get; set; }
    public int Count { get; set; }
    public string Color { get; set; }

    public PriorityStatistic(string name, int count, string color)
    {
        Name = name;
     Count = count;
 Color = color;
    }
}
