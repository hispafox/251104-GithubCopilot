using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoWinForms.Business.Services;
using DemoWinForms.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Windows;

namespace DemoWpf.ViewModels;

public partial class MoveTaskDialogViewModel : ViewModelBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger<MoveTaskDialogViewModel> _logger;
  private readonly Guid _taskId;
    private readonly Guid _currentListId;

 [ObservableProperty]
    private ObservableCollection<TaskListOption> _availableLists = new();

    [ObservableProperty]
    private TaskListOption? _selectedList;

    [ObservableProperty]
 private string _validationMessage = string.Empty;

    [ObservableProperty]
    private bool _isMoving;

    public string TaskTitle { get; }
    public bool DialogResult { get; private set; }

    public MoveTaskDialogViewModel(ITaskService taskService, ILogger<MoveTaskDialogViewModel> logger,
  Guid taskId, Guid currentListId, string taskTitle)
    {
     _taskService = taskService;
        _logger = logger;
        _taskId = taskId;
   _currentListId = currentListId;
    TaskTitle = taskTitle;
    }

    [RelayCommand]
    public async Task LoadListsAsync()
    {
   try
   {
   var result = await _taskService.GetAllTaskListsAsync();
   if (result.IsSuccess)
   {
      AvailableLists.Clear();
      foreach (var list in result.Value!.Where(l => l.Id != _currentListId))
  {
     AvailableLists.Add(new TaskListOption(list));
   }
             if (!AvailableLists.Any())
      {
      ValidationMessage = "No hay otras listas disponibles";
      }
  }
    else
  {
ValidationMessage = result.Error;
 }
    }
 catch (Exception ex)
        {
_logger.LogError(ex, "Error al cargar listas");
 ValidationMessage = "Error al cargar listas";
}
    }

 [RelayCommand(CanExecute = nameof(CanMove))]
    private async Task MoveAsync(Window? window)
    {
        if (SelectedList == null) return;

   try
        {
      IsMoving = true;
   ValidationMessage = string.Empty;

    var result = await _taskService.MoveTaskToListAsync(_taskId, SelectedList.Id);
   if (result.IsSuccess)
   {
      _logger.LogInformation("Tarea movida a lista");
         DialogResult = true;
   window?.Close();
     }
    else
   {
        ValidationMessage = result.Error;
     }
        }
    catch (Exception ex)
{
            _logger.LogError(ex, "Error al mover tarea");
 ValidationMessage = $"Error inesperado: {ex.Message}";
  }
        finally
  {
   IsMoving = false;
   }
    }

 [RelayCommand]
    private void Cancel(Window? window)
    {
 DialogResult = false;
   window?.Close();
    }

    private bool CanMove() => SelectedList != null && !IsMoving;
}

public class TaskListOption
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ColorCode { get; set; }
    public int TaskCount { get; set; }

    public TaskListOption(TaskList taskList)
 {
  Id = taskList.Id;
    Name = taskList.Name;
   Description = taskList.Description;
  ColorCode = taskList.ColorCode ?? "#4A90E2";
  TaskCount = taskList.TotalTasks;
    }

    public string DisplayText => $"{Name} ({TaskCount} tareas)";
}
