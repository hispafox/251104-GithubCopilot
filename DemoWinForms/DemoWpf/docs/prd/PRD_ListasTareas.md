# Product Requirements Document (PRD) - Sistema de Gestión de Listas de Tareas para Aplicación WPF

## 1. Visión General del Producto

### 1.1 Propósito

Implementar un sistema completo de gestión de listas de tareas en la aplicación WPF existente, permitiendo a los usuarios organizar múltiples tareas agrupadas en listas temáticas.

### 1.2 Objetivos

- Permitir la creación, edición y eliminación de listas de tareas
- Gestionar tareas individuales dentro de cada lista
- Proporcionar una interfaz intuitiva siguiendo los patrones MVVM de WPF
- Garantizar persistencia de datos

## 2. Alcance del Proyecto

### 2.1 Características Incluidas

- ✅ Gestión completa de listas de tareas (CRUD)
- ✅ Gestión completa de tareas individuales (CRUD)
- ✅ Relación jerárquica: Lista → Tareas
- ✅ Interfaz de usuario responsive y moderna
- ✅ Persistencia de datos

### 2.2 Características Excluidas (v1.0)

- ❌ Sincronización en la nube
- ❌ Compartir listas con otros usuarios
- ❌ Recordatorios y notificaciones
- ❌ Adjuntos o archivos en tareas
- ❌ Subtareas o jerarquías multinivel

## 3. Requisitos Funcionales

### 3.1 Gestión de Listas de Tareas

**RF-01: Crear Lista de Tareas**

- El usuario puede crear una nueva lista especificando:
    - Nombre (obligatorio, máx. 100 caracteres)
    - Descripción (opcional, máx. 500 caracteres)
    - Color/ícono identificativo (opcional)
- Validación: No permitir nombres duplicados o vacíos

**RF-02: Visualizar Listas**

- Mostrar todas las listas en un panel lateral o principal
- Indicar número de tareas por lista
- Mostrar contador de tareas completadas vs pendientes

**RF-03: Editar Lista**

- Permitir modificar nombre y descripción
- Mantener la integridad de las tareas asociadas

**RF-04: Eliminar Lista**

- Solicitar confirmación antes de eliminar
- Opciones:
    - Eliminar lista y todas sus tareas
    - Mover tareas a otra lista antes de eliminar

### 3.2 Gestión de Tareas

**RF-05: Crear Tarea**

- Crear tarea dentro de una lista específica con:
    - Título (obligatorio, máx. 200 caracteres)
    - Descripción (opcional, máx. 1000 caracteres)
    - Fecha de vencimiento (opcional)
    - Prioridad (Baja/Media/Alta)
    - Estado: Pendiente (por defecto)

**RF-06: Visualizar Tareas**

- Mostrar tareas de la lista seleccionada
- Ordenar por: fecha creación, prioridad, vencimiento, estado
- Filtrar por: estado, prioridad, fechas

**RF-07: Editar Tarea**

- Modificar todos los campos de la tarea
- Cambiar estado (Pendiente, En Progreso, Completada)
- Mover tarea a otra lista

**RF-08: Eliminar Tarea**

- Eliminar tarea con confirmación opcional
- Actualizar contadores de la lista

**RF-09: Marcar Tarea como Completada**

- Toggle rápido del estado completado/pendiente
- Registrar fecha de completado
- Actualizar contadores visuales

### 3.3 Navegación e Interacción

**RF-10: Navegación entre Listas**

- Seleccionar lista para ver sus tareas
- Breadcrumb o indicador de lista actual

**RF-11: Búsqueda**

- Buscar tareas por título o descripción
- Buscar en todas las listas o solo en la actual

**RF-12: Estadísticas**

- Vista resumen con:
    - Total de listas
    - Total de tareas
    - Tareas completadas hoy/semana
    - Tareas vencidas

## 4. Requisitos No Funcionales

### 4.1 Arquitectura

- **Patrón**: MVVM (Model-View-ViewModel)
- **Framework**: .NET 8 WPF
- **Capa de datos**: Separada en DemoWinForms.Core

### 4.2 Modelos de Datos

```csharp
// TaskList.cs
public class TaskList
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ColorCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public List<TaskItem> Tasks { get; set; }
}

// TaskItem.cs
public class TaskItem
{
    public Guid Id { get; set; }
    public Guid TaskListId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskPriority Priority { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public enum TaskPriority { Low, Medium, High }
public enum TaskStatus { Pending, InProgress, Completed }

```

### 4.3 Persistencia

- **Opción 1 (Recomendada)**: Entity Framework Core + SQLite
- **Opción 2**: Serialización JSON local
- **Opción 3**: SQL Server LocalDB

### 4.4 Rendimiento

- Carga inicial < 2 segundos
- Respuesta UI < 100ms para operaciones CRUD
- Soporte para al menos 100 listas y 1000 tareas totales

### 4.5 Usabilidad

- Interfaz moderna siguiendo Material Design o Fluent Design
- Soporte para temas claro/oscuro
- Atajos de teclado para acciones comunes
- Drag & drop para mover tareas entre listas

## 5. Interfaz de Usuario

### 5.1 Layout Principal

```
┌─────────────────────────────────────────────────┐
│  [Menú]  Gestión de Tareas          [⚙️] [👤]  │
├──────────────┬──────────────────────────────────┤
│              │                                  │
│  📋 Listas   │  📝 Tareas - [Nombre Lista]     │
│              │  ┌──────────────────────────┐   │
│ ⊕ Nueva      │  │ ☐ Tarea 1          🔴   │   │
│              │  │ ☐ Tarea 2          🟡   │   │
│ • Lista 1    │  │ ☑ Tarea 3          🟢   │   │
│   (5/10)     │  └──────────────────────────┘   │
│              │                                  │
│ • Lista 2    │  [+ Nueva Tarea]                │
│   (2/3)      │                                  │
│              │  Filtros: [Todas ▼] [📅] [⭐]   │
└──────────────┴──────────────────────────────────┘

```

### 5.2 Ventanas/Diálogos

- **MainWindow**: Vista principal con listas y tareas
- **TaskListDialog**: Crear/editar lista
- **TaskItemDialog**: Crear/editar tarea
- **ConfirmationDialog**: Confirmaciones de eliminación
- **StatisticsWindow**: Vista de estadísticas (opcional)

## 6. Estructura de Proyecto

### 6.1 DemoWinForms.Core (Capa de Dominio)

```

/Models
  - TaskList.cs
  - TaskItem.cs
  - Enums.cs (TaskPriority, TaskStatus)
/Interfaces
  - ITaskRepository.cs
/Services
  - TaskService.cs
/Data
  - TaskDbContext.cs (si usa EF Core)

```

### 6.2 DemoWpf (Capa de Presentación)

```
/Views
  - MainWindow.xaml
  - TaskListDialog.xaml
  - TaskItemDialog.xaml
/ViewModels
  - MainViewModel.cs
  - TaskListViewModel.cs
  - TaskItemViewModel.cs
/Converters
  - PriorityToColorConverter.cs
  - StatusToIconConverter.cs
/Resources
  - Styles.xaml
  - Icons.xaml

```

## 7. Criterios de Aceptación

### Sprint 1 - Funcionalidad Básica

- ✅ Crear/listar/eliminar listas de tareas
- ✅ Crear/listar/eliminar tareas
- ✅ Marcar tareas como completadas
- ✅ Persistencia básica de datos

### Sprint 2 - Funcionalidad Avanzada

- ✅ Edición completa de listas y tareas
- ✅ Filtros y ordenamiento
- ✅ Prioridades y fechas de vencimiento
- ✅ Mover tareas entre listas

### Sprint 3 - UI/UX

- ✅ Diseño visual completo
- ✅ Validaciones y mensajes de error
- ✅ Drag & drop
- ✅ Atajos de teclado

## 8. Dependencias Técnicas

### 8.1 NuGet Packages

```xml
<!-- Para EF Core + SQLite -->
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.*" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.*" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.*" />

<!-- Para MVVM -->
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.*" />

<!-- Para UI moderna (opcional) -->
<PackageReference Include="MaterialDesignThemes" Version="4.9.*" />

```

## 9. Riesgos y Mitigaciones

| Riesgo | Impacto | Mitigación |
| --- | --- | --- |
| Pérdida de datos | Alto | Implementar auto-guardado y backups |
| Rendimiento con muchas tareas | Medio | Paginación y virtualización de listas |
| Complejidad del drag & drop | Medio | Usar librerías probadas o implementar en v2 |

## 10. Roadmap

- **v1.0** (2-3 semanas): Funcionalidad básica + persistencia
- **v1.1** (1 semana): Filtros, búsqueda, estadísticas
- **v1.2** (1 semana): UI mejorada, drag & drop
- **v2.0** (futuro): Sincronización cloud, colaboración

---

**Notas de Implementación:**

- Comenzar con Entity Framework Core + SQLite para persistencia
- Usar CommunityToolkit.Mvvm para simplificar ViewModels
- Implementar validaciones en el ViewModel
- Usar INotifyPropertyChanged y ObservableCollection para binding reactivo

¿Te gustaría que proceda con la implementación de alguna parte específica del sistema? Puedo empezar con:

1. Los modelos de datos en DemoWinForms.Core
2. El DbContext y repositorio
3. Los ViewModels con MVVM
4. Las vistas XAML