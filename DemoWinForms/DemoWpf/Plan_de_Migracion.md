## 🚀 **PLAN DE MIGRACIÓN PROGRESIVA ACTUALIZADO**

### **Fase 1: Refactorización Previa (3-4 días)**

**Objetivo:** Preparar el código existente para ser compartido

### 1.1 Crear Proyecto de Biblioteca Compartida

```bash
# En la raíz de la solución
dotnet new classlib -n DemoWinForms.Core -f net8.0
dotnet sln add DemoWinForms.Core/DemoWinForms.Core.csproj

```

### 1.2 Mover Capas Reutilizables

**Estructura final:**

```
Solución/
├── DemoWinForms.Core/          # 🆕 NUEVO PROYECTO
│   ├── Domain/
│   ├── Data/
│   ├── Business/
│   └── Common/
│
├── DemoWinForms/               # 🔄 MANTENER TEMPORALMENTE
│   ├── Presentation/
│   ├── Program.cs
│   └── appsettings.json
│
├── DemoWpf/                    # 🆕 NUEVA APP WPF
│   ├── Views/
│   ├── ViewModels/
│   ├── Converters/
│   ├── Resources/
│   └── App.xaml
│
└── DemoWinForms.Tests/         # 🆕 PRUEBAS COMPARTIDAS

```

### 1.3 Actualizar Referencias

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.11" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.1" />
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Logging" Version="8.0.1" />
    <PackageReference Include="Serilog" Version="4.1.0" />
    <PackageReference Include="FluentValidation" Version="11.9.0" />
  </ItemGroup>

</Project>

```

---

### **Fase 2: Configurar Proyecto WPF (2-3 días)**

### 2.1 Crear Proyecto WPF con Referencias

```bash
dotnet new wpf -n DemoWpf -f net8.0-windows
dotnet sln add DemoWpf/DemoWpf.csproj
dotnet add DemoWpf reference DemoWinForms.Core

```

### 2.2 Instalar Paquetes MVVM

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0-windows</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <UseWPF>true</UseWPF>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="CommunityToolkit.Mvvm" Version="8.3.2" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.1" />
    <PackageReference Include="Microsoft.Extensions.Hosting" Version="8.0.1" />
    <PackageReference Include="Serilog.Extensions.Hosting" Version="8.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\\DemoWinForms.Core\\DemoWinForms.Core.csproj" />
  </ItemGroup>

</Project>

```

### 2.3 Configurar App.xaml.cs con DI

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DemoWinForms.Data;
using DemoWinForms.Data.Repositories;
using DemoWinForms.Business.Services;
using DemoWpf.Views;
using DemoWpf.ViewModels;
using Serilog;
using System.Windows;
using System.IO;

namespace DemoWpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        // Configurar Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "app-.txt"),
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(services, context.Configuration);
            })
            .UseSerilog()
            .Build();
    }

    private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // 📁 Base de datos
        var dbPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "tareas.db");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        // 🗂️ Repositorios
        services.AddScoped<ITareaRepository, TareaRepository>();
        services.AddScoped<IEtiquetaRepository, EtiquetaRepository>();

        // 💼 Servicios de negocio
        services.AddScoped<ITareaService, TareaService>();

        // 🎨 ViewModels
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<TareasViewModel>();
        services.AddTransient<TareaDetalleViewModel>();

        // 🪟 Views
        services.AddSingleton<MainWindow>();
        services.AddTransient<TareasView>();
        services.AddTransient<TareaDetalleView>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        // Aplicar migraciones
        using (var scope = _host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync();
        }

        // Mostrar ventana principal
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        Log.CloseAndFlush();
        base.OnExit(e);
    }

    /// <summary>
    /// Proveedor de servicios global para acceso desde ViewModels
    /// </summary>
    public static IServiceProvider Services => ((App)Current)._host.Services;
}

```

---

### **Fase 3: Migrar FormPrincipal a WPF (1-2 semanas)**

### 3.1 Crear ViewModel Principal

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoWinForms.Domain.Entities;
using DemoWinForms.Domain.Enums;
using DemoWinForms.Business.Services;
using DemoWinForms.Business.DTOs;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Extensions.Logging;

namespace DemoWpf.ViewModels;

public partial class TareasViewModel : ObservableObject
{
    private readonly ITareaService _tareaService;
    private readonly ILogger<TareasViewModel> _logger;
    private System.Threading.Timer? _debounceTimer;

    [ObservableProperty]
    private ObservableCollection<Tarea> _tareas = new();

    [ObservableProperty]
    private Tarea? _tareaSeleccionada;

    // Filtros
    [ObservableProperty]
    private bool _filtroPendiente = true;

    [ObservableProperty]
    private bool _filtroEnProgreso = true;

    [ObservableProperty]
    private bool _filtroCompletada;

    [ObservableProperty]
    private bool _filtroCancelada;

    [ObservableProperty]
    private PrioridadTarea? _prioridadSeleccionada;

    [ObservableProperty]
    private string? _categoriaSeleccionada;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EditarTareaCommand))]
    [NotifyCanExecuteChangedFor(nameof(EliminarTareaCommand))]
    [NotifyCanExecuteChangedFor(nameof(CompletarTareaCommand))]
    private string _textoBusqueda = string.Empty;

    [ObservableProperty]
    private string _mensajeEstado = "Listo";

    public TareasViewModel(
        ITareaService tareaService,
        ILogger<TareasViewModel> logger)
    {
        _tareaService = tareaService;
        _logger = logger;
    }

    partial void OnTextoBusquedaChanged(string value)
    {
        // Debounce de 500ms
        _debounceTimer?.Dispose();
        _debounceTimer = new System.Threading.Timer(
            async _ => await CargarTareasAsync(),
            null,
            TimeSpan.FromMilliseconds(500),
            TimeSpan.FromMilliseconds(-1));
    }

    [RelayCommand]
    private async Task CargarTareasAsync()
    {
        try
        {
            var filtros = new FiltroTareasDto
            {
                Estados = ObtenerEstadosFiltrados(),
                Prioridad = PrioridadSeleccionada,
                Categoria = CategoriaSeleccionada,
                BusquedaTexto = TextoBusqueda
            };

            var resultado = await _tareaService.GetByFiltrosAsync(filtros);

            if (resultado.IsSuccess)
            {
                Tareas = new ObservableCollection<Tarea>(resultado.Value);
                MensajeEstado = $"{Tareas.Count} tarea(s) encontrada(s)";
                _logger.LogInformation("Tareas cargadas: {Count}", Tareas.Count);
            }
            else
            {
                MensajeEstado = $"Error: {resultado.Error}";
                _logger.LogWarning("Error al cargar tareas: {Error}", resultado.Error);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción al cargar tareas");
            MessageBox.Show($"Error inesperado: {ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private async Task NuevaTareaAsync()
    {
        // Abrir ventana de detalle
        var detalleView = App.Services.GetService<TareaDetalleView>();
        if (detalleView?.ShowDialog() == true)
        {
            await CargarTareasAsync();
        }
    }

    [RelayCommand(CanExecute = nameof(PuedeEditarTarea))]
    private async Task EditarTareaAsync()
    {
        if (TareaSeleccionada == null) return;

        var detalleView = App.Services.GetService<TareaDetalleView>();
        var viewModel = detalleView?.DataContext as TareaDetalleViewModel;

        if (viewModel != null && detalleView != null)
        {
            await viewModel.CargarTareaAsync(TareaSeleccionada.Id);
            if (detalleView.ShowDialog() == true)
            {
                await CargarTareasAsync();
            }
        }
    }

    [RelayCommand(CanExecute = nameof(PuedeEliminarTarea))]
    private async Task EliminarTareaAsync()
    {
        if (TareaSeleccionada == null) return;

        var resultado = MessageBox.Show(
            $"¿Está seguro que desea eliminar la tarea '{TareaSeleccionada.Titulo}'?",
            "Confirmar eliminación",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (resultado == MessageBoxResult.Yes)
        {
            var resultadoEliminacion = await _tareaService.EliminarTareaAsync(TareaSeleccionada.Id);

            if (resultadoEliminacion.IsSuccess)
            {
                MensajeEstado = "Tarea eliminada correctamente";
                await CargarTareasAsync();
            }
            else
            {
                MessageBox.Show(resultadoEliminacion.Error, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    [RelayCommand(CanExecute = nameof(PuedeCompletarTarea))]
    private async Task CompletarTareaAsync()
    {
        if (TareaSeleccionada == null) return;

        var resultado = await _tareaService.MarcarComoCompletadaAsync(TareaSeleccionada.Id);

        if (resultado.IsSuccess)
        {
            MensajeEstado = "Tarea completada";
            await CargarTareasAsync();
        }
        else
        {
            MessageBox.Show(resultado.Error, "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool PuedeEditarTarea() => TareaSeleccionada != null;
    private bool PuedeEliminarTarea() => TareaSeleccionada != null;
    private bool PuedeCompletarTarea() =>
        TareaSeleccionada != null &&
        TareaSeleccionada.Estado != EstadoTarea.Completada;

    private List<EstadoTarea> ObtenerEstadosFiltrados()
    {
        var estados = new List<EstadoTarea>();

        if (FiltroPendiente) estados.Add(EstadoTarea.Pendiente);
        if (FiltroEnProgreso) estados.Add(EstadoTarea.EnProgreso);
        if (FiltroCompletada) estados.Add(EstadoTarea.Completada);
        if (FiltroCancelada) estados.Add(EstadoTarea.Cancelada);

        return estados.Any() ? estados : Enum.GetValues<EstadoTarea>().ToList();
    }
}

```

### 3.2 Crear Vista WPF

```xml
<UserControl x:Class="DemoWpf.Views.TareasView"
             xmlns="<http://schemas.microsoft.com/winfx/2006/xaml/presentation>"
             xmlns:x="<http://schemas.microsoft.com/winfx/2006/xaml>"
             xmlns:d="<http://schemas.microsoft.com/expression/blend/2008>"
             xmlns:mc="<http://schemas.openxmlformats.org/markup-compatibility/2006>"
             xmlns:vm="clr-namespace:DemoWpf.ViewModels"
             d:DataContext="{d:DesignInstance Type=vm:TareasViewModel}"
             mc:Ignorable="d"
             d:DesignHeight="600" d:DesignWidth="1000">
    <UserControl.Resources>
        <!-- Estilos para las filas según prioridad -->
        <Style x:Key="TareaRowStyle" TargetType="DataGridRow">
            <Style.Triggers>
                <!-- Prioridad Crítica -->
                <DataTrigger Binding="{Binding Prioridad}" Value="3">
                    <Setter Property="Background" Value="#FFE6E6"/>
                </DataTrigger>
                <!-- Prioridad Alta -->
                <DataTrigger Binding="{Binding Prioridad}" Value="2">
                    <Setter Property="Background" Value="#FFF4E6"/>
                </DataTrigger>
                <!-- Prioridad Media -->
                <DataTrigger Binding="{Binding Prioridad}" Value="1">
                    <Setter Property="Background" Value="#E6F7FF"/>
                </DataTrigger>
                <!-- Prioridad Baja -->
                <DataTrigger Binding="{Binding Prioridad}" Value="0">
                    <Setter Property="Background" Value="#E6FFE6"/>
                </DataTrigger>
                <!-- Completada -->
                <DataTrigger Binding="{Binding Estado}" Value="2">
                    <Setter Property="Foreground" Value="Gray"/>
                    <Setter Property="FontStyle" Value="Italic"/>
                </DataTrigger>
            </Style.Triggers>
        </Style>
    </UserControl.Resources>

    <Grid Margin="10">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>

        <!-- Barra de herramientas -->
        <StackPanel Grid.Row="0" Orientation="Horizontal" Margin="0,0,0,10">
            <Button Content="➕ Nueva"
                    Command="{Binding NuevaTareaCommand}"
                    Padding="15,8"
                    Margin="0,0,5,0"
                    Background="#4CAF50"
                    Foreground="White"
                    BorderThickness="0"
                    Cursor="Hand"/>
            <Button Content="✏️ Editar"
                    Command="{Binding EditarTareaCommand}"
                    Padding="15,8"
                    Margin="0,0,5,0"
                    Background="#2196F3"
                    Foreground="White"
                    BorderThickness="0"/>
            <Button Content="🗑️ Eliminar"
                    Command="{Binding EliminarTareaCommand}"
                    Padding="15,8"
                    Margin="0,0,5,0"
                    Background="#F44336"
                    Foreground="White"
                    BorderThickness="0"/>
            <Button Content="✔️ Completar"
                    Command="{Binding CompletarTareaCommand}"
                    Padding="15,8"
                    Margin="0,0,5,0"
                    Background="#FF9800"
                    Foreground="White"
                    BorderThickness="0"/>
            <Button Content="🔄 Refrescar"
                    Command="{Binding CargarTareasCommand}"
                    Padding="15,8"
                    Background="#9E9E9E"
                    Foreground="White"
                    BorderThickness="0"/>
        </StackPanel>

        <!-- Panel de filtros -->
        <Expander Grid.Row="1" Header="🔍 Filtros" IsExpanded="True" Margin="0,0,0,10">
            <Grid Margin="10">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="*"/>
                    <ColumnDefinition Width="*"/>
                    <ColumnDefinition Width="*"/>
                </Grid.ColumnDefinitions>

                <!-- Filtro por Estado -->
                <GroupBox Grid.Column="0" Header="Estado" Margin="0,0,5,0">
                    <StackPanel>
                        <CheckBox Content="Pendiente"
                                  IsChecked="{Binding FiltroPendiente}"
                                  Margin="5"/>
                        <CheckBox Content="En Progreso"
                                  IsChecked="{Binding FiltroEnProgreso}"
                                  Margin="5"/>
                        <CheckBox Content="Completada"
                                  IsChecked="{Binding FiltroCompletada}"
                                  Margin="5"/>
                        <CheckBox Content="Cancelada"
                                  IsChecked="{Binding FiltroCancelada}"
                                  Margin="5"/>
                    </StackPanel>
                </GroupBox>

                <!-- Filtro por Prioridad -->
                <GroupBox Grid.Column="1" Header="Prioridad" Margin="5,0">
                    <StackPanel>
                        <RadioButton Content="Todas"
                                     IsChecked="{Binding PrioridadSeleccionada,
                                                 Converter={StaticResource NullToBooleanConverter}}"
                                     Margin="5"/>
                        <RadioButton Content="Baja" Margin="5"/>
                        <RadioButton Content="Media" Margin="5"/>
                        <RadioButton Content="Alta" Margin="5"/>
                        <RadioButton Content="Crítica" Margin="5"/>
                    </StackPanel>
                </GroupBox>

                <!-- Búsqueda -->
                <GroupBox Grid.Column="2" Header="Búsqueda" Margin="5,0,0,0">
                    <TextBox Text="{Binding TextoBusqueda, UpdateSourceTrigger=PropertyChanged}"
                             VerticalAlignment="Center"
                             Margin="5"/>
                </GroupBox>
            </Grid>
        </Expander>

        <!-- DataGrid de tareas -->
        <DataGrid Grid.Row="2"
                  ItemsSource="{Binding Tareas}"
                  SelectedItem="{Binding TareaSeleccionada}"
                  RowStyle="{StaticResource TareaRowStyle}"
                  AutoGenerateColumns="False"
                  IsReadOnly="True"
                  SelectionMode="Single"
                  GridLinesVisibility="Horizontal"
                  AlternatingRowBackground="#F5F5F5">
            <DataGrid.Columns>
                <DataGridTextColumn Header="ID" Binding="{Binding Id}" Width="50"/>
                <DataGridTextColumn Header="Título" Binding="{Binding Titulo}" Width="*"/>
                <DataGridTextColumn Header="Estado" Binding="{Binding Estado}" Width="100"/>
                <DataGridTextColumn Header="Prioridad" Binding="{Binding Prioridad}" Width="100"/>
                <DataGridTextColumn Header="Categoría" Binding="{Binding Categoria}" Width="120"/>
                <DataGridTextColumn Header="Vencimiento"
                                    Binding="{Binding FechaVencimiento, StringFormat=dd/MM/yyyy}"
                                    Width="110"/>
                <DataGridTextColumn Header="Usuario" Binding="{Binding Usuario.Nombre}" Width="150"/>
            </DataGrid.Columns>
        </DataGrid>

        <!-- Barra de estado -->
        <StatusBar Grid.Row="3" Margin="0,5,0,0">
            <StatusBarItem>
                <TextBlock Text="{Binding MensajeEstado}"/>
            </StatusBarItem>
        </StatusBar>
    </Grid>
</UserControl>

```

---

## 📊 **VENTAJAS ESPECÍFICAS DE ESTA ESTRATEGIA**

### ✅ **1. Reutilización Máxima (80-90% del código)**

- ✅ `Domain/` → **100% reutilizable**
- ✅ `Data/` → **95% reutilizable** (solo ajustes menores en DI)
- ✅ `Business/` → **100% reutilizable**
- ✅ `Common/` → **100% reutilizable**
- ❌ `Presentation/` → **0% reutilizable** (migrar a XAML/MVVM)

### ✅ **2. Aprendizaje Gradual**

- Semana 1-2: Refactorización (conocimientos existentes)
- Semana 3-4: MVVM básico (nueva sintaxis)
- Semana 5-6: Features avanzadas WPF
- Sin "shock" de migración completa

### ✅ **3. Testing Continuo**

- Cada módulo se prueba antes de continuar
- Windows Forms sigue funcionando como "red de seguridad"
- Rollback posible en cualquier momento

### ✅ **4. Paralelo Temporal Corto**

- Solo 2-3 semanas con ambas apps
- No requiere mantenimiento dual prolongado

---

## ⏱️ **CRONOGRAMA DETALLADO**

| Semana | Fase | Entregable | Riesgo |
| --- | --- | --- | --- |
| 1 | Refactorización | Proyecto `.Core` funcional | Bajo |
| 2 | Setup WPF | App WPF vacía con DI | Bajo |
| 3-4 | Vista principal | TareasView completa | Medio |
| 5 | Detalle tarea | FormTarea → TareaDetalleView | Medio |
| 6 | Funciones extra | Estadísticas, exportación | Bajo |
| 7 | Pulido | Estilos, optimización | Bajo |

---

## ✅ **CONCLUSIÓN Y RECOMENDACIÓN FINAL**

**Elige la Estrategia 1** porque:

1. ✅ Tu arquitectura actual es **excelente** y está lista para ser compartida
2. ✅ El 85% del código es **reutilizable sin cambios**
3. ✅ Riesgo **bajo-medio** vs. alto (Big Bang) o costos excesivos (Paralelo)
4. ✅ Tiempo estimado: **6-7 semanas** vs. 10-12 semanas (Paralelo) o alto riesgo (Big Bang)
5. ✅ Permite **aprender WPF/MVVM progresivamente**