# Instrucciones del Proyecto - Gestor de Tareas (Windows Forms)

## Descripción General
Aplicación de escritorio para gestión de tareas desarrollada con .NET 8 y Windows Forms. Implementa una arquitectura limpia con separación de capas, inyección de dependencias, Entity Framework Core con SQLite, y logging con Serilog.

## Arquitectura del Proyecto

### Estructura de Capas

```
DemoWinForms/
??? Domain/   # Capa de Dominio
?   ??? Entities/      # Entidades del negocio
?   ?   ??? Tarea.cs
?   ?   ??? Usuario.cs
?   ?   ??? Etiqueta.cs
?   ?   ??? TareaEtiqueta.cs
?   ??? Enums/    # Enumeraciones
?       ??? EstadoTarea.cs
?       ??? PrioridadTarea.cs
??? Data/        # Capa de Acceso a Datos
?   ??? AppDbContext.cs     # Contexto de Entity Framework
?   ??? AppDbContextFactory.cs
?   ??? Repositories/       # Repositorios
?    ??? ITareaRepository.cs
?       ??? TareaRepository.cs
?       ??? IEtiquetaRepository.cs
?       ??? EtiquetaRepository.cs
??? Business/                # Capa de Lógica de Negocio
?   ??? Services/           # Servicios
?   ?   ??? ITareaService.cs
?   ?   ??? TareaService.cs
?   ??? DTOs/        # Objetos de Transferencia de Datos
?    ??? FiltroTareasDto.cs
?    ??? EstadisticasDto.cs
??? Presentation/            # Capa de Presentación
?   ??? Forms/        # Formularios Windows Forms
?       ??? FormPrincipal.cs
?       ??? FormPrincipal.Designer.cs
?       ??? FormTarea.cs
? ??? FormTarea.Designer.cs
??? Common/       # Utilidades Comunes
?   ??? Result.cs           # Clase Result para manejo de resultados
?   ??? Constants.cs      # Constantes de la aplicación
??? Migrations/            # Migraciones de Entity Framework
??? Program.cs          # Punto de entrada de la aplicación
??? appsettings.json        # Configuración de la aplicación
```

## Tecnologías y Paquetes Utilizados

### Framework Base
- **.NET 8.0** (Target: net8.0-windows)
- **Windows Forms** (.NET 8)
- **C# 12** (LangVersion)

### Paquetes NuGet

#### Entity Framework Core
- `Microsoft.EntityFrameworkCore.Sqlite` (8.0.11) - Base de datos SQLite
- `Microsoft.EntityFrameworkCore.Tools` (8.0.11) - Herramientas de migraciones

#### Inyección de Dependencias
- `Microsoft.Extensions.DependencyInjection` (8.0.1)
- `Microsoft.Extensions.Configuration` (8.0.0)
- `Microsoft.Extensions.Configuration.Json` (8.0.1)
- `Microsoft.Extensions.Logging` (8.0.1)

#### Logging
- `Serilog` (4.1.0)
- `Serilog.Sinks.File` (6.0.0)
- `Serilog.Extensions.Logging` (8.0.0)

#### Validación
- `FluentValidation` (11.9.0)

#### Exportación de Datos
- `EPPlus` (7.5.3) - Exportación a Excel

## Configuración del Proyecto

### Configuración Inicial

1. **Base de Datos SQLite**
   - La base de datos se crea automáticamente en el directorio del proyecto
   - Ubicación: `tareas.db` en la raíz del proyecto
   - Las migraciones se aplican automáticamente al iniciar la aplicación

2. **Archivo appsettings.json**
   - Configurado para copiarse al directorio de salida
   - Contiene configuraciones de la aplicación

3. **Logging**
 - Los logs se guardan en la carpeta `logs/`
   - Formato: `app-YYYYMMDD.txt` (un archivo por día)
   - Nivel mínimo: Information

### Configuración de Dependencias (Program.cs)

```csharp
// DbContext con SQLite
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// Repositorios
services.AddScoped<ITareaRepository, TareaRepository>();
services.AddScoped<IEtiquetaRepository, EtiquetaRepository>();

// Servicios
services.AddScoped<ITareaService, TareaService>();

// Logging
services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(dispose: true);
});

// Formularios
services.AddTransient<FormPrincipal>();
services.AddTransient<FormTarea>();
```

## Funcionalidades Principales

### FormPrincipal (Formulario Principal)

#### Características
- **Visualización de Tareas**: DataGridView con formato condicional
- **Filtros Múltiples**:
  - Por Estado (Pendiente, En Progreso, Completada, Cancelada)
  - Por Prioridad (Crítica, Alta, Media, Baja)
  - Por Categoría
  - Búsqueda por texto (con debounce de 500ms)
- **Operaciones CRUD**:
  - Crear nueva tarea
  - Editar tarea existente
  - Eliminar tarea
  - Marcar como completada
- **Formato Visual**:
  - Colores por prioridad:
    - Crítica: Fondo rojo claro (#FFE6E6)
    - Alta: Fondo naranja claro (#FFF4E6)
    - Media: Fondo azul claro (#E6F7FF)
    - Baja: Fondo verde claro (#E6FFE6)
  - Tareas completadas: Texto tachado y gris
  - Tareas vencidas: Texto rojo en negrita

#### Métodos Principales
- `CargarTareasAsync()`: Carga las tareas según filtros
- `ObtenerFiltros()`: Construye el DTO de filtros
- `AplicarFormatoFilas()`: Aplica formato visual a las filas
- `BtnNueva_Click()`: Crea nueva tarea
- `BtnEditar_Click()`: Edita tarea seleccionada
- `BtnEliminar_Click()`: Elimina tarea con confirmación
- `BtnCompletar_Click()`: Marca tarea como completada

### FormTarea (Formulario de Tarea)

#### Funcionalidad
- Formulario de creación/edición de tareas
- Validación de campos
- Asignación de propiedades:
  - Título (obligatorio)
  - Descripción
  - Estado
  - Prioridad
  - Categoría
  - Fecha de vencimiento
  - Usuario asignado
  - Etiquetas

### TareaService (Servicio de Lógica de Negocio)

#### Métodos Principales

```csharp
// Operaciones CRUD
Task<Result<Tarea>> GetByIdAsync(int id)
Task<Result<IEnumerable<Tarea>>> GetAllAsync()
Task<Result<IEnumerable<Tarea>>> GetByFiltrosAsync(FiltroTareasDto filtros)
Task<Result<Tarea>> CrearTareaAsync(Tarea tarea)
Task<Result> ActualizarTareaAsync(Tarea tarea)
Task<Result> EliminarTareaAsync(int id)

// Operaciones especiales
Task<Result> MarcarComoCompletadaAsync(int id)
Task<Result<EstadisticasDto>> GetEstadisticasAsync(int? usuarioId = null)
Task<Result<IEnumerable<Tarea>>> GetProximasVencerAsync(int dias = 3)
```

#### Validaciones
- Título: Obligatorio, máximo 200 caracteres
- Descripción: Opcional, máximo 2000 caracteres
- Fecha de vencimiento: No puede ser anterior a hoy
- Usuario: Debe estar asignado (UsuarioId > 0)

### Pattern Result

El proyecto utiliza un patrón Result para manejo consistente de errores:

```csharp
public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; }
    
    public static Result Success()
    public static Result Failure(string error)
}

public class Result<T> : Result
{
    public T Value { get; }
    
    public static Result<T> Success(T value)
    public static Result<T> Failure(string error)
}
```

**Ventajas**:
- Manejo explícito de éxito/error
- Evita excepciones para flujos esperados
- Mensajes de error descriptivos
- Fácil propagación de errores

## Modelo de Datos

### Entidades Principales

#### Tarea
```csharp
public class Tarea
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string? Descripcion { get; set; }
    public EstadoTarea Estado { get; set; }
    public PrioridadTarea Prioridad { get; set; }
    public string Categoria { get; set; }
    public DateTime FechaCreacion { get; set; }
 public DateTime? FechaVencimiento { get; set; }
    public DateTime? FechaCompletado { get; set; }
  public DateTime UltimaModificacion { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
    public ICollection<TareaEtiqueta> TareaEtiquetas { get; set; }
}
```

#### Enumeraciones
```csharp
public enum EstadoTarea
{
    Pendiente = 0,
    EnProgreso = 1,
    Completada = 2,
    Cancelada = 3
}

public enum PrioridadTarea
{
    Baja = 0,
    Media = 1,
    Alta = 2,
    Critica = 3
}
```

## Flujo de Trabajo

### Ciclo de Vida de una Tarea

1. **Creación**:
   - Usuario hace clic en "Nueva Tarea"
   - Se abre FormTarea en modo creación
   - Se completan los campos requeridos
   - TareaService valida los datos
   - Se guarda en la base de datos
   - Se registra en el log
   - Se actualiza el DataGridView

2. **Edición**:
   - Usuario selecciona una tarea y hace clic en "Editar"
   - Se abre FormTarea con datos cargados
   - Se modifican los campos deseados
   - TareaService valida los cambios
   - Se actualiza en la base de datos
   - Se actualiza el DataGridView

3. **Completar**:
   - Usuario selecciona una tarea y hace clic en "Completar"
   - Se actualiza el estado a Completada
   - Se registra FechaCompletado
   - Se actualiza en la base de datos
   - Se aplica formato tachado en la vista

4. **Eliminar**:
   - Usuario selecciona una tarea y hace clic en "Eliminar"
   - Se muestra confirmación
   - Si confirma, se elimina de la base de datos
   - Se actualiza el DataGridView

### Filtrado y Búsqueda

1. **Filtros por Estado**:
   - CheckBoxes para cada estado
   - Aplicación automática al cambiar
   - Si ninguno está marcado, muestra todos

2. **Filtros por Prioridad**:
   - RadioButtons para cada prioridad
   - Opción "Todas" disponible

3. **Búsqueda por Texto**:
   - Búsqueda en Título y Descripción
   - Debounce de 500ms para evitar búsquedas excesivas
   - Actualización automática mientras se escribe

## Patrones de Diseño Utilizados

### Repository Pattern
- Abstracción del acceso a datos
- Interfaces para testabilidad
- Separación de lógica de persistencia

### Service Layer Pattern
- Lógica de negocio centralizada
- Validaciones en la capa de servicio
- Coordinación entre repositorios

### Dependency Injection
- IoC Container de Microsoft.Extensions
- Registro de servicios en Program.cs
- Inyección por constructor

### Result Pattern
- Manejo funcional de errores
- Sin excepciones para casos esperados
- Mensajes descriptivos

## Mejores Prácticas Implementadas

### Código
- **Async/Await**: Todas las operaciones de datos son asíncronas
- **Nullable Reference Types**: Habilitado para mayor seguridad
- **Logging Estructurado**: Con Serilog para trazabilidad
- **Separación de Responsabilidades**: Arquitectura en capas clara
- **SOLID Principles**: 
  - Single Responsibility: Cada clase tiene un propósito único
  - Open/Closed: Extensible mediante interfaces
  - Liskov Substitution: Cumplimiento de contratos
  - Interface Segregation: Interfaces específicas
  - Dependency Inversion: Dependencia de abstracciones

### UI/UX
- **Feedback Visual**: Colores según prioridad y estado
- **Confirmaciones**: Para operaciones destructivas
- **Mensajes Informativos**: Para éxito y errores
- **Debounce**: En búsqueda para mejor rendimiento
- **Double Click**: Para edición rápida

### Datos
- **Migraciones Automáticas**: Al iniciar la aplicación
- **Transacciones**: Gestión automática por EF Core
- **Lazy Loading**: Según necesidad
- **Índices**: Configurados en el DbContext

## Comandos Útiles

### Entity Framework Core

```bash
# Crear nueva migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones
dotnet ef database update

# Revertir migración
dotnet ef database update MigracionAnterior

# Eliminar última migración
dotnet ef migrations remove

# Ver SQL de una migración
dotnet ef migrations script
```

### Compilación y Ejecución

```bash
# Compilar
dotnet build

# Ejecutar
dotnet run

# Limpiar
dotnet clean

# Publicar (release)
dotnet publish -c Release
```

## Extensibilidad

### Agregar Nuevo Formulario

1. Crear el formulario en `Presentation/Forms/`
2. Registrarlo en Program.cs:
   ```csharp
   services.AddTransient<NuevoFormulario>();
   ```
3. Obtenerlo desde ServiceProvider:
   ```csharp
   var form = Program.ServiceProvider.GetService<NuevoFormulario>();
   ```

### Agregar Nueva Entidad

1. Crear clase en `Domain/Entities/`
2. Agregar DbSet en `AppDbContext`
3. Crear repositorio en `Data/Repositories/`
4. Crear servicio en `Business/Services/`
5. Crear migración y aplicar

### Agregar Validaciones

1. Implementar validador con FluentValidation
2. Registrar en el contenedor de DI
3. Inyectar en el servicio correspondiente
4. Aplicar validaciones antes de operaciones

## Troubleshooting

### Base de Datos No Se Crea
- Verificar permisos de escritura en el directorio
- Revisar logs en carpeta `logs/`
- Verificar que las migraciones existen

### Errores de Inyección de Dependencias
- Verificar registro en Program.cs
- Verificar que las interfaces están implementadas
- Verificar ciclos de vida (Scoped, Transient, Singleton)

### Formularios No Se Abren
- Verificar que están registrados en el contenedor
- Verificar que el ServiceProvider está inicializado
- Revisar logs de Serilog

### Problemas de Rendimiento
- Verificar consultas N+1 (usar Include para cargas eager)
- Implementar paginación para grandes volúmenes
- Considerar caché para datos frecuentes

## Convenciones de Código

### Nomenclatura
- **Clases**: PascalCase
- **Métodos**: PascalCase
- **Propiedades**: PascalCase
- **Variables privadas**: _camelCase (con prefijo _)
- **Variables locales**: camelCase
- **Constantes**: PascalCase
- **Interfaces**: IPascalCase (prefijo I)

### Organización
- **Usings**: Ordenados automáticamente
- **Regiones**: Evitar, preferir archivos pequeños
- **Comentarios**: XML comments para APIs públicas
- **Async**: Sufijo Async en métodos asíncronos

## Seguridad

### Consideraciones
- **SQL Injection**: Protegido por EF Core (consultas parametrizadas)
- **Validación de Entrada**: En capa de servicio
- **Manejo de Errores**: Sin exposición de detalles internos
- **Logs**: Sin información sensible

### Recomendaciones para Producción
- Implementar autenticación de usuarios
- Agregar autorización basada en roles
- Cifrar datos sensibles
- Implementar auditoría de cambios
- Validar en cliente y servidor

## Recursos Adicionales

### Documentación Oficial
- [.NET 8 Documentation](https://docs.microsoft.com/dotnet/core/whats-new/dotnet-8)
- [Windows Forms .NET](https://docs.microsoft.com/dotnet/desktop/winforms/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Serilog Documentation](https://serilog.net/)

### Herramientas Recomendadas
- **Visual Studio 2022** (Community o superior)
- **DB Browser for SQLite** (para inspeccionar la base de datos)
- **Postman** (si se agrega API)

## Licencia y Créditos

© 2024 - Gestor de Tareas v1.0
Desarrollado con .NET 8 y Windows Forms

---

**Última actualización**: 2024
**Versión del documento**: 1.0
