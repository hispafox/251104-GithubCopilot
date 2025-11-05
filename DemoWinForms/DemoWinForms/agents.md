# Gestor de Tareas - Windows Forms & WPF Applications

## ?? Descripción del Proyecto

Suite de aplicaciones de escritorio desarrolladas en **.NET 8** para la gestión de tareas personales y profesionales. El proyecto implementa una **arquitectura en capas compartida** con dos interfaces de usuario:

1. **DemoWinForms** - Aplicación Windows Forms (Legacy/Producción)
2. **DemoWpf** - Aplicación WPF con MVVM (Moderna/En desarrollo)

Ambas aplicaciones comparten las capas de dominio, datos y lógica de negocio a través del proyecto **DemoWinForms.Core**, implementando Entity Framework Core, inyección de dependencias y patrones modernos de desarrollo.

## ??? Arquitectura de la Solución

El proyecto sigue una **arquitectura en capas con núcleo compartido**:

### ?? Estructura de la Solución

```
251104-GithubCopilot/DemoWinForms/
??? DemoWinForms.Core/     # ?? NÚCLEO COMPARTIDO
?   ??? Domain/          # Capa de Dominio
?   ?   ??? Entities/     # Entidades del dominio
?   ?   ?   ??? Tarea.cs
?   ?   ?   ??? Usuario.cs
?   ?   ?   ??? Etiqueta.cs
?   ?   ?   ??? TareaEtiqueta.cs
?   ?   ??? Enums/       # Enumeraciones
?   ?       ??? EstadoTarea.cs
?   ?  ??? PrioridadTarea.cs
?   ?
?   ??? Data/           # Capa de Acceso a Datos
?   ?   ??? AppDbContext.cs
?   ?   ??? AppDbContextFactory.cs
?   ?   ??? Repositories/        # Patrón Repository
?   ?       ??? ITareaRepository.cs
?   ?       ??? TareaRepository.cs
?   ?   ??? IEtiquetaRepository.cs
?   ?       ??? EtiquetaRepository.cs
?   ?
?   ??? Business/       # Capa de Lógica de Negocio
?   ?   ??? Services/
?   ?   ?   ??? ITareaService.cs
?   ?   ?   ??? TareaService.cs
?   ?   ??? DTOs/  # Data Transfer Objects
?   ?       ??? FiltroTareasDto.cs
?   ?       ??? EstadisticasDto.cs
?   ?
?   ??? Common/   # Utilidades compartidas
?       ??? Result.cs     # Patrón Result
?       ??? Constants.cs
?
??? DemoWinForms/      # ?? APLICACIÓN WINDOWS FORMS
?   ??? Presentation/            # Capa de Presentación
?   ?   ??? Forms/
?   ?       ??? FormPrincipal.cs
?   ?    ??? FormPrincipal.Designer.cs
?   ?       ??? FormTarea.cs
?   ?       ??? FormTarea.Designer.cs
?   ??? Program.cs
?   ??? appsettings.json
?
??? DemoWpf/      # ?? APLICACIÓN WPF (MVVM)
?   ??? Views/    # Vistas XAML
?   ?   ??? MainWindow.xaml
?   ?   ??? TareaListWindow.xaml
?   ?   ??? TareaEditWindow.xaml
?   ?   ??? EtiquetaManagerWindow.xaml
?   ?
?   ??? ViewModels/    # ViewModels MVVM
?   ?   ??? ViewModelBase.cs
?   ?   ??? MainWindowViewModel.cs
?   ?   ??? TareaListViewModel.cs
?   ? ??? TareaEditViewModel.cs
?   ?   ??? ThemeViewModel.cs
?   ?   ??? EtiquetaManagerViewModel.cs
? ?
?   ??? Services/      # Servicios específicos de WPF
?   ? ??? IExportService.cs
?   ?   ??? ExportService.cs
?   ?
?   ??? Converters/  # Convertidores XAML
? ?   ??? BooleanToVisibilityConverter.cs
?   ?
?   ??? Themes/      # Temas visuales
?   ?   ??? LightTheme.xaml
?   ?   ??? DarkTheme.xaml
?   ?
?   ??? Resources/    # Recursos compartidos
?   ?   ??? Styles.xaml
?   ?
?   ??? App.xaml
?   ??? App.xaml.cs
?   ??? appsettings.json
?
??? Migrations/        # Migraciones de EF Core
    ??? 20251105072136_InitialCreate.cs
    ??? AppDbContextModelSnapshot.cs
```

## ?? Tecnologías y Paquetes Utilizados

### Framework Base
- **.NET 8.0** (Windows)
- **Windows Forms** (UI Framework Legacy)
- **WPF** (Windows Presentation Foundation - Moderna)
- **C# 12** con Nullable Reference Types habilitado

### Paquetes NuGet

#### ??? Base de Datos y ORM
- **Microsoft.EntityFrameworkCore.Sqlite** (v8.0.11)
  - Base de datos SQLite embebida compartida
- **Microsoft.EntityFrameworkCore.Tools** (v8.0.11)
  - Herramientas para migraciones y scaffolding

#### ?? Inyección de Dependencias y Configuración
- **Microsoft.Extensions.DependencyInjection** (v8.0.1)
  - Contenedor de IoC
- **Microsoft.Extensions.Hosting** (v8.0.1)
- Hosting genérico para WPF
- **Microsoft.Extensions.Configuration** (v8.0.0)
  - Sistema de configuración
- **Microsoft.Extensions.Configuration.Json** (v8.0.1)
  - Soporte para archivos JSON de configuración
- **Microsoft.Extensions.Logging** (v8.0.1)
  - Infraestructura de logging

#### ?? Logging
- **Serilog** (v4.1.0)
  - Framework de logging estructurado
- **Serilog.Sinks.File** (v6.0.0)
  - Escritura de logs a archivos
- **Serilog.Extensions.Logging** (v8.0.0)
  - Integración con Microsoft.Extensions.Logging
- **Serilog.Extensions.Hosting** (v8.0.0)
  - Integración con Generic Host (WPF)

#### ? Validación
- **FluentValidation** (v11.9.0)
  - Validaciones fluidas y expresivas

#### ?? Exportación de Datos (WPF)
- **EPPlus** (v7.5.3)
  - Generación y manipulación de archivos Excel

#### ?? MVVM Toolkit (WPF)
- **CommunityToolkit.Mvvm** (v8.3.2)
  - Herramientas modernas para MVVM (Source Generators)
  - ObservableProperty, RelayCommand, etc.

## ?? Modelo de Datos

### Entidades Principales

#### 1. **Tarea**
Entidad principal del sistema que representa una tarea individual.

**Propiedades:**
- `Id` (int): Identificador único
- `Titulo` (string): Título de la tarea (máx. 200 caracteres, requerido)
- `Descripcion` (string?): Descripción detallada (máx. 2000 caracteres)
- `FechaCreacion` (DateTime): Fecha de creación automática
- `FechaVencimiento` (DateTime?): Fecha límite opcional
- `FechaCompletado` (DateTime?): Fecha de completado
- `Prioridad` (PrioridadTarea): Nivel de prioridad (enum)
- `Estado` (EstadoTarea): Estado actual (enum)
- `Categoria` (string): Categoría de la tarea
- `UsuarioId` (int): Relación con Usuario
- `TareaPadreId` (int?): Para subtareas (auto-referencial)
- `UltimaModificacion` (DateTime): Auditoría
- `EliminadoLogico` (bool): Soft delete

**Relaciones:**
- Pertenece a un `Usuario`
- Puede tener múltiples `Subtareas`
- Relación many-to-many con `Etiqueta` a través de `TareaEtiqueta`

#### 2. **Usuario**
Representa el usuario que crea y gestiona tareas.

**Propiedades:**
- `Id` (int)
- `Nombre` (string, máx. 200 caracteres)
- `Edad` (int)
- `Pais` (string, máx. 100 caracteres)
- `Email` (string, máx. 255 caracteres, único)
- `Telefono` (string, máx. 20 caracteres)

#### 3. **Etiqueta**
Sistema de etiquetado flexible para categorizar tareas.

**Propiedades:**
- `Id` (int)
- `Nombre` (string, máx. 50 caracteres, único)
- `ColorHex` (string, 7 caracteres, ej: "#FF5733")

#### 4. **TareaEtiqueta**
Tabla de unión para la relación many-to-many entre Tareas y Etiquetas.

### Enumeraciones

#### **EstadoTarea**
- `Pendiente` (0): Tarea no iniciada
- `EnProgreso` (1): Tarea en curso
- `Completada` (2): Tarea finalizada
- `Cancelada` (3): Tarea cancelada

#### **PrioridadTarea**
- `Baja` (0): Prioridad baja
- `Media` (1): Prioridad media (por defecto)
- `Alta` (2): Prioridad alta
- `Critica` (3): Prioridad crítica

## ?? DemoWinForms - Aplicación Windows Forms

### Características de la Interfaz de Usuario

#### FormPrincipal (Ventana Principal)

**Funcionalidades:**
1. **Visualización de Tareas**
   - DataGridView con todas las tareas
   - Colores por prioridad:
     - ?? Crítica: Fondo rojo claro (#FFE6E6)
     - ?? Alta: Fondo naranja claro (#FFF4E6)
     - ?? Media: Fondo azul claro (#E6F7FF)
     - ?? Baja: Fondo verde claro (#E6FFE6)
   - Tareas completadas: texto tachado y gris
   - Tareas vencidas: texto rojo y negrita

2. **Sistema de Filtros**
   - Por estado (checkboxes): Pendiente, En Progreso, Completada, Cancelada
   - Por prioridad (radio buttons): Todas, Baja, Media, Alta, Crítica
   - Por categoría (combo box)
   - Búsqueda por texto con debounce (500ms)

3. **Operaciones CRUD**
   - ? Crear nueva tarea
   - ?? Editar tarea seleccionada (doble clic o botón)
   - ??? Eliminar tarea con confirmación
   - ?? Marcar como completada
   - ?? Refrescar listado

4. **Barra de Estado**
   - Contador de tareas
   - Mensaje de estado de operaciones

#### FormTarea (Formulario de Edición)
- Formulario modal para crear/editar tareas
- Campos para todos los atributos de la tarea
- Validaciones integradas

## ?? DemoWpf - Aplicación WPF con MVVM

### Patrón MVVM Implementado

#### ViewModels Principales

##### 1. **MainWindowViewModel**
Gestiona la ventana principal y estadísticas.

**Propiedades:**
- `Estadisticas` (EstadisticasDto)
- `IsLoading` (bool)

**Comandos:**
- `LoadEstadisticasCommand` (IAsyncRelayCommand)
- `OpenTareasCommand` (IRelayCommand)
- `OpenNewTareaCommand` (IRelayCommand)

**Eventos:**
- `OpenTareasRequested`
- `OpenNewTareaRequested`

##### 2. **TareaListViewModel**
Lista y filtrado de tareas.

**Propiedades:**
- `Tareas` (ObservableCollection<Tarea>)
- `TareaSeleccionada` (Tarea?)
- `FiltroPendiente`, `FiltroEnProgreso`, `FiltroCompletada`, `FiltroCancelada` (bool)
- `PrioridadSeleccionada` (PrioridadTarea?)
- `CategoriaSeleccionada` (string?)
- `TextoBusqueda` (string)
- `MensajeEstado` (string)

**Comandos:**
- `LoadTareasCommand`
- `NewTareaCommand`
- `EditTareaCommand` (CanExecute según selección)
- `DeleteTareaCommand` (CanExecute según selección)
- `CompleteTareaCommand` (CanExecute según estado)
- `ExportCsvCommand`
- `ExportJsonCommand`

**Eventos:**
- `EditTareaRequested`
- `NewTareaRequested`

##### 3. **TareaEditViewModel**
Creación y edición de tareas.

**Propiedades:**
- `Tarea` (Tarea)
- Propiedades individuales editables
- `IsEditMode` (bool)
- `ValidationErrors` (ObservableCollection<string>)

**Comandos:**
- `SaveCommand`
- `CancelCommand`

##### 4. **ThemeViewModel**
Gestión de temas claro/oscuro.

**Propiedades:**
- `IsDarkMode` (bool)

**Comandos:**
- `ToggleThemeCommand`

**Características:**
- Persistencia en `Settings.settings`
- Aplicación dinámica de ResourceDictionary
- Soporte para `LightTheme.xaml` y `DarkTheme.xaml`

##### 5. **EtiquetaManagerViewModel**
Gestión CRUD de etiquetas.

**Propiedades:**
- `Etiquetas` (ObservableCollection<Etiqueta>)
- `EtiquetaSeleccionada` (Etiqueta?)
- `NuevaEtiquetaNombre`, `NuevaEtiquetaColor` (string)

**Comandos:**
- `LoadEtiquetasCommand`
- `AddEtiquetaCommand`
- `EditEtiquetaCommand`
- `DeleteEtiquetaCommand`

### Características Avanzadas WPF

#### 1. **Servicios Específicos**

##### IExportService / ExportService
Exportación de datos a diferentes formatos:
- **CSV**: Exportación con escape de caracteres especiales
- **JSON**: Serialización con JsonSerializer
- Encoding UTF-8 con BOM

#### 2. **Temas Visuales**
- **LightTheme.xaml**: Tema claro con paleta de colores suaves
- **DarkTheme.xaml**: Tema oscuro con contraste optimizado
- Cambio dinámico sin reiniciar la aplicación

#### 3. **Converters XAML**
- `BooleanToVisibilityConverter`: Conversión Bool ? Visibility
- Soporte para inversión (`ConverterParameter="Inverse"`)

#### 4. **Data Binding Avanzado**
- `UpdateSourceTrigger=PropertyChanged` para búsqueda en tiempo real
- `INotifyPropertyChanged` automático con `[ObservableProperty]`
- Command binding con `CanExecute` dinámico

#### 5. **Source Generators**
Uso de **CommunityToolkit.Mvvm** para generar código:
- `[ObservableProperty]` ? Genera propiedades con INotifyPropertyChanged
- `[RelayCommand]` ? Genera comandos ICommand
- `[NotifyCanExecuteChangedFor]` ? Actualiza CanExecute automáticamente

## ?? Patrones y Principios Aplicados

### Patrones de Diseño

1. **Repository Pattern**
   - Abstracción del acceso a datos
   - Interfaces: `ITareaRepository`, `IEtiquetaRepository`
   - Implementaciones con Entity Framework Core

2. **Service Layer Pattern**
   - Lógica de negocio centralizada en `TareaService`
   - Separación clara entre UI y lógica de negocio

3. **Dependency Injection (DI)**
   - Configurado en `Program.cs` (WinForms) y `App.xaml.cs` (WPF)
   - Registro de servicios, repositorios y contextos
   - Formularios/ViewModels/Views registrados como Transient/Scoped/Singleton

4. **Result Pattern**
   - Manejo de errores sin excepciones
   - Clase `Result<T>` en la capa Common
   - Proporciona `IsSuccess`, `Value`, `Error`

5. **MVVM Pattern** (WPF)
   - Separación completa View-ViewModel-Model
   - Data binding bidireccional
   - Commands para lógica de interacción
   - ViewModelBase con INotifyPropertyChanged

6. **DTO Pattern**
   - `FiltroTareasDto`: Encapsula criterios de filtrado
   - `EstadisticasDto`: Datos agregados de tareas
   - Separación entre entidades de dominio y modelos de transferencia

### Principios SOLID

- **Single Responsibility**: Cada clase tiene una única responsabilidad
- **Open/Closed**: Extensible mediante interfaces
- **Liskov Substitution**: Interfaces pueden ser sustituidas
- **Interface Segregation**: Interfaces específicas y cohesivas
- **Dependency Inversion**: Dependencias hacia abstracciones

## ??? Base de Datos

### Proveedor
- **SQLite** (archivo local `tareas.db`)
- Ubicación: Directorio del proyecto (compartida entre aplicaciones)
- Mismo esquema para WinForms y WPF

### Estrategia de Migraciones
- **Code-First** con Entity Framework Core
- Migraciones automáticas al iniciar la aplicación
- Comando: `context.Database.MigrateAsync()` en ambas apps

### Configuración
```csharp
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));
```

### Datos Iniciales (Seed)
- Usuario demo: `demo@tareas.com`
- Tarea de ejemplo: "Bienvenido al Gestor de Tareas"
- 5 etiquetas predefinidas: Urgente, Proyecto, Personal, Trabajo, Importante

## ?? Logging

### Configuración de Serilog
- **Nivel mínimo**: Information
- **Sink**: Archivo en carpeta `logs/`
- **Rotación**: Diaria (`app-{fecha}.txt`)
- **Eventos registrados**:
  - Inicio y cierre de aplicación
  - Operaciones CRUD
  - Errores y excepciones
  - Inicialización de base de datos

### Ejemplo de uso:
```csharp
_logger.LogInformation("Formulario principal cargado correctamente");
_logger.LogError(ex, "Error al cargar tareas");
```

## ?? Configuración de las Aplicaciones

### appsettings.json
Archivo de configuración compartido para:
- Cadenas de conexión
- Configuración de logging
- Parámetros personalizados

### Características de Program.cs (WinForms)

1. **Configuración de Servicios**
   ```csharp
   ServiceProvider = services.BuildServiceProvider();
   ```

2. **Inicialización de Base de Datos**
   - Aplicación automática de migraciones
   - Verificación de integridad

3. **Manejo de Errores Global**
   - Try-catch en el punto de entrada
   - Logging de excepciones fatales
   - MessageBox para errores críticos

### Características de App.xaml.cs (WPF)

1. **Generic Host Pattern**
```csharp
   _host = Host.CreateDefaultBuilder()
       .ConfigureServices(...)
       .UseSerilog()
   .Build();
   ```

2. **Lifecycle Hooks**
   - `OnStartup`: Aplicar migraciones, mostrar ventana
   - `OnExit`: Liberar recursos, flush logs

3. **Service Provider Global**
   ```csharp
   public static IServiceProvider Services => ((App)Current)._host.Services;
   ```

## ?? Flujo de Ejecución

### Windows Forms (DemoWinForms)

1. **Inicio de Aplicación**
   - Configuración de Serilog
   - Carga de `appsettings.json`
   - Registro de servicios en DI Container
   - Aplicación de migraciones
   - Inicialización de FormPrincipal

2. **Carga del Formulario Principal**
   - Inicialización de controles
   - Carga asíncrona de tareas con filtros
   - Aplicación de formato visual

3. **Operaciones de Usuario**
   - Filtrado en tiempo real
   - CRUD de tareas
   - Actualización automática de la interfaz

4. **Cierre de Aplicación**
   - Flush de logs
   - Liberación de recursos

### WPF (DemoWpf)

1. **Inicio de Aplicación**
   - Configuración de Generic Host
   - Configuración de Serilog
   - Registro de servicios, ViewModels y Views
   - Aplicación de migraciones
   - Carga de tema guardado

2. **MainWindow**
   - Carga de estadísticas
   - Navegación a ventanas secundarias
   - Gestión de eventos de ViewModels

3. **TareaListWindow**
   - Binding de TareaListViewModel
   - Carga asíncrona de tareas
   - Filtrado reactivo con debounce
   - Exportación CSV/JSON

4. **TareaEditWindow**
   - Modo creación/edición
   - Validación en tiempo real
   - Guardado con Result Pattern

## ?? Características Avanzadas

### 1. Búsqueda con Debounce
- Evita búsquedas excesivas mientras el usuario escribe
- Timer de 500ms antes de ejecutar la búsqueda
- Implementado en ambas aplicaciones

### 2. Formateo Visual Dinámico
- **WinForms**: DataGridView con estilos condicionales
- **WPF**: DataTriggers en DataGrid.RowStyle
- Colores según prioridad, estilo tachado para completadas

### 3. Filtros Múltiples Combinados
- Aplicación simultánea de múltiples criterios
- Estado + Prioridad + Categoría + Texto
- Consultas optimizadas con LINQ

### 4. Validaciones
- FluentValidation para reglas de negocio (Core)
- Data Annotations en entidades
- Validación en ViewModel (WPF)

### 5. Soft Delete
- Eliminación lógica mediante `EliminadoLogico`
- Permite recuperación de datos
- Filtrado automático en consultas

### 6. Exportación de Datos (WPF)
- CSV con escape de caracteres especiales
- JSON con formato legible (WriteIndented)
- SaveFileDialog para selección de destino

### 7. Temas Dinámicos (WPF)
- Cambio entre claro/oscuro sin reiniciar
- Persistencia de preferencia del usuario
- Aplicación global con ResourceDictionary

## ?? DTOs Utilizados

### FiltroTareasDto
```csharp
- Estados?: List<EstadoTarea>
- Prioridad?: PrioridadTarea?
- Categoria?: string
- BusquedaTexto?: string
- OrdenarPor?: string
- TamañoPagina?: int
```

### EstadisticasDto
```csharp
- TotalTareas: int
- TareasPendientes: int
- TareasEnProgreso: int
- TareasCompletadas: int
- TareasCanceladas: int
- ProximasVencer: int
- TareasPorPrioridad: Dictionary<PrioridadTarea, int>
- TareasPorCategoria: Dictionary<string, int>
```

## ? Mejores Prácticas Implementadas

1. **Separación de Responsabilidades**
   - UI separada de la lógica de negocio
   - Lógica de negocio separada del acceso a datos
   - Core compartido entre múltiples interfaces

2. **Programación Asíncrona**
   - Operaciones async/await para no bloquear la UI
   - Mejor experiencia de usuario
   - Task-based asynchronous pattern

3. **Manejo de Errores Robusto**
   - Pattern Result para operaciones
   - Try-catch en operaciones críticas
   - Logging de excepciones

4. **Inyección de Dependencias**
   - Testabilidad mejorada
   - Acoplamiento reducido
   - Facilita el mantenimiento

5. **Convenciones de Código**
   - Nullable Reference Types habilitado
   - ImplicitUsings para simplificar código
   - Documentación XML en clases principales
   - File-scoped namespaces (C# 10+)

6. **Source Generators** (WPF)
   - Reduce boilerplate code
   - Mejor rendimiento en compilación
   - Menos errores humanos

## ??? Comandos Útiles

### Entity Framework Core
```bash
# Crear nueva migración
dotnet ef migrations add NombreMigracion --project DemoWinForms.Core

# Aplicar migraciones
dotnet ef database update --project DemoWinForms

# Revertir migración
dotnet ef database update MigracionAnterior --project DemoWinForms

# Eliminar última migración
dotnet ef migrations remove --project DemoWinForms.Core
```

### Compilación y Ejecución
```bash
# Compilar solución completa
dotnet build

# Ejecutar WinForms
dotnet run --project DemoWinForms

# Ejecutar WPF
dotnet run --project DemoWpf

# Publicar WinForms
dotnet publish DemoWinForms -c Release -o ./publish/winforms

# Publicar WPF
dotnet publish DemoWpf -c Release -o ./publish/wpf
```

## ?? Conceptos de Aprendizaje

Este proyecto es ideal para aprender:

- ? Windows Forms moderno con .NET 8
- ? WPF con MVVM y Community Toolkit
- ? Entity Framework Core con SQLite
- ? Arquitectura en capas compartida
- ? Dependency Injection en aplicaciones desktop
- ? Patrones Repository y Service Layer
- ? Logging con Serilog
- ? Programación asíncrona en WinForms y WPF
- ? Validaciones con FluentValidation
- ? LINQ y expresiones lambda
- ? Manejo de estado y filtros complejos
- ? Source Generators con CommunityToolkit.Mvvm
- ? Data Binding y Commands en WPF
- ? Temas dinámicos y estilos XAML
- ? Exportación de datos (CSV, JSON)

## ?? Posibles Extensiones Futuras

1. **Exportación a Excel** (EPPlus ya está incluido en WPF)
2. **Notificaciones de tareas vencidas** (Windows ToastNotifications)
3. **Sistema de recordatorios** con timers
4. **Gráficos y estadísticas visuales** (LiveCharts o OxyPlot)
5. **Sincronización en la nube** (Azure Storage o SQL Azure)
6. **Trabajo con múltiples usuarios** (autenticación y autorización)
7. **Sistema de adjuntos de archivos** (blob storage)
8. **Integración con calendario** (Outlook, Google Calendar)
9. **Temas personalizados** (editor de colores)
10. **Reportes personalizados** con Crystal Reports o FastReport
11. **Aplicación móvil** con .NET MAUI compartiendo el Core
12. **API REST** con ASP.NET Core Web API

## ?? Comparación WinForms vs WPF

| Característica | WinForms | WPF |
|----------------|----------|-----|
| Patrón de diseño | Code-behind | MVVM |
| Data Binding | Manual (eventos) | Bidireccional automático |
| Estilos | Limitado | Totalmente personalizable |
| Temas | No nativo | ResourceDictionary |
| Animaciones | No soportado | Sí, con Storyboards |
| Rendimiento | Bueno | Excelente (aceleración GPU) |
| Curva de aprendizaje | Baja | Media-Alta |
| Legacy | Sí | Tecnología actual |
| Comunidad | Estable | Activa y creciente |

## ?? Estado del Proyecto

- **DemoWinForms**: ? Funcional y completo
- **DemoWinForms.Core**: ? Estable y compartido
- **DemoWpf**: ? Funcional con todas las características principales
- **Migraciones**: ? Aplicadas y funcionando
- **Documentación**: ? Actualizada

### Funcionalidades Implementadas en WPF

- ? Ventana principal con estadísticas
- ? Lista de tareas con filtros avanzados
- ? Creación y edición de tareas
- ? Gestión de etiquetas
- ? Temas claro/oscuro
- ? Exportación CSV/JSON
- ? Logging con Serilog
- ? Validaciones en ViewModel
- ? Comandos con CanExecute
- ? Búsqueda con debounce

## ?? Información de Contacto

Aplicación desarrollada como proyecto de demostración de buenas prácticas en Windows Forms y WPF con .NET 8, implementando arquitectura compartida y patrones modernos.

---

**Versión:** 2.0  
**Framework:** .NET 8.0 (Windows)  
**Última actualización:** Noviembre 2025  
**Repositorio:** https://github.com/hispafox/251104-GithubCopilot  
**Licencia:** © 2024-2025
