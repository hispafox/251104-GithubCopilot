# Gestor de Tareas - Windows Forms Application

## ?? Descripción del Proyecto

Aplicación de escritorio desarrollada en **Windows Forms** con **.NET 8** para la gestión de tareas personales y profesionales. Implementa una arquitectura en capas con Entity Framework Core, inyección de dependencias, y patrones modernos de desarrollo.

## ??? Arquitectura del Proyecto

El proyecto sigue una **arquitectura en capas** claramente definida:

### ?? Estructura de Carpetas

```
DemoWinForms/
??? Domain/            # Capa de Dominio
?   ??? Entities/      # Entidades del dominio
?   ?   ??? Tarea.cs
?   ?   ??? Usuario.cs
?   ?   ??? Etiqueta.cs
?   ?   ??? TareaEtiqueta.cs
?   ??? Enums/                # Enumeraciones
?   ??? EstadoTarea.cs
?       ??? PrioridadTarea.cs
?
??? Data/    # Capa de Acceso a Datos
?   ??? AppDbContext.cs       # Contexto de Entity Framework
?   ??? AppDbContextFactory.cs
?   ??? Repositories/       # Patrón Repository
?       ??? ITareaRepository.cs
?  ??? TareaRepository.cs
?       ??? IEtiquetaRepository.cs
?       ??? EtiquetaRepository.cs
?
??? Business/       # Capa de Lógica de Negocio
?   ??? Services/             # Servicios de negocio
?   ?   ??? ITareaService.cs
?   ?   ??? TareaService.cs
?   ??? DTOs/     # Data Transfer Objects
?       ??? FiltroTareasDto.cs
?       ??? EstadisticasDto.cs
?
??? Presentation/      # Capa de Presentación
?   ??? Forms/        # Formularios WinForms
?       ??? FormPrincipal.cs
?       ??? FormPrincipal.Designer.cs
?       ??? FormTarea.cs
?       ??? FormTarea.Designer.cs
?
??? Common/  # Utilidades compartidas
?   ??? Result.cs   # Patrón Result para manejo de errores
?   ??? Constants.cs          # Constantes de la aplicación
?
??? Migrations/   # Migraciones de EF Core
?   ??? ...
?
??? Program.cs           # Punto de entrada de la aplicación
```

## ?? Tecnologías y Paquetes Utilizados

### Framework Base
- **.NET 8.0** (Windows)
- **Windows Forms** (UI Framework)
- **C# 12** con Nullable Reference Types habilitado

### Paquetes NuGet

#### ??? Base de Datos y ORM
- **Microsoft.EntityFrameworkCore.Sqlite** (v8.0.11)
  - Base de datos SQLite embebida
- **Microsoft.EntityFrameworkCore.Tools** (v8.0.11)
  - Herramientas para migraciones y scaffolding

#### ?? Inyección de Dependencias y Configuración
- **Microsoft.Extensions.DependencyInjection** (v8.0.1)
  - Contenedor de IoC
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

#### ? Validación
- **FluentValidation** (v11.9.0)
  - Validaciones fluidas y expresivas

#### ?? Exportación de Datos
- **EPPlus** (v7.5.3)
  - Generación y manipulación de archivos Excel

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

#### 3. **Etiqueta**
Sistema de etiquetado flexible para categorizar tareas.

#### 4. **TareaEtiqueta**
Tabla de unión para la relación many-to-many entre Tareas y Etiquetas.

### Enumeraciones

#### **EstadoTarea**
- `Pendiente`: Tarea no iniciada
- `EnProgreso`: Tarea en curso
- `Completada`: Tarea finalizada
- `Cancelada`: Tarea cancelada

#### **PrioridadTarea**
- `Baja`: Prioridad baja
- `Media`: Prioridad media (por defecto)
- `Alta`: Prioridad alta
- `Critica`: Prioridad crítica

## ?? Características de la Interfaz de Usuario

### FormPrincipal (Ventana Principal)

#### Funcionalidades:
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
   - ? Marcar como completada
   - ?? Refrescar listado

4. **Barra de Estado**
   - Contador de tareas
   - Mensaje de estado de operaciones

### FormTarea (Formulario de Edición)
- Formulario modal para crear/editar tareas
- Campos para todos los atributos de la tarea
- Validaciones integradas

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
   - Configurado en `Program.cs`
   - Registro de servicios, repositorios y contextos
   - Formularios registrados como Transient

4. **Result Pattern**
   - Manejo de errores sin excepciones
   - Clase `Result<T>` en la capa Common
   - Proporciona `IsSuccess`, `Value`, `Error`

5. **DTO Pattern**
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
- Ubicación: Directorio del proyecto

### Estrategia de Migraciones
- **Code-First** con Entity Framework Core
- Migraciones automáticas al iniciar la aplicación
- Comando: `context.Database.Migrate()` en `Program.cs`

### Configuración
```csharp
services.AddDbContext<AppDbContext>(options =>
 options.UseSqlite($"Data Source={dbPath}"));
```

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

## ?? Configuración de la Aplicación

### appsettings.json
Archivo de configuración para parámetros de la aplicación:
- Cadenas de conexión
- Configuración de logging
- Parámetros personalizados

### Características de Program.cs

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

## ?? Flujo de Ejecución

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

## ?? Características Avanzadas

### 1. Búsqueda con Debounce
- Evita búsquedas excesivas mientras el usuario escribe
- Timer de 500ms antes de ejecutar la búsqueda

### 2. Formateo Visual Dinámico
- Colores según prioridad
- Estilo tachado para completadas
- Resaltado de tareas vencidas

### 3. Filtros Múltiples Combinados
- Aplicación simultánea de múltiples criterios
- Estado + Prioridad + Categoría + Texto

### 4. Validaciones
- FluentValidation para reglas de negocio
- Validaciones de Data Annotations en entidades

### 5. Soft Delete
- Eliminación lógica mediante `EliminadoLogico`
- Permite recuperación de datos

## ?? DTOs Utilizados

### FiltroTareasDto
```csharp
- Estado?: EstadoTarea?
- Prioridad?: PrioridadTarea?
- Categoria?: string
- BusquedaTexto?: string
- OrdenarPor?: string
- TamañoPagina?: int
```

### EstadisticasDto
Proporciona métricas y estadísticas sobre las tareas.

## ?? Mejores Prácticas Implementadas

1. **Separación de Responsabilidades**
   - UI separada de la lógica de negocio
   - Lógica de negocio separada del acceso a datos

2. **Programación Asíncrona**
   - Operaciones async/await para no bloquear la UI
   - Mejor experiencia de usuario

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

## ?? Comandos Útiles

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
```

### Compilación y Ejecución
```bash
# Compilar
dotnet build

# Ejecutar
dotnet run

# Publicar
dotnet publish -c Release
```

## ?? Conceptos de Aprendizaje

Este proyecto es ideal para aprender:

- ? Windows Forms moderno con .NET 8
- ? Entity Framework Core con SQLite
- ? Arquitectura en capas
- ? Dependency Injection en aplicaciones desktop
- ? Patrones Repository y Service Layer
- ? Logging con Serilog
- ? Programación asíncrona en WinForms
- ? Validaciones con FluentValidation
- ? LINQ y expresiones lambda
- ? Manejo de estado y filtros complejos

## ?? Posibles Extensiones Futuras

1. **Exportación a Excel** (EPPlus ya está incluido)
2. **Notificaciones de tareas vencidas**
3. **Sistema de recordatorios**
4. **Gráficos y estadísticas visuales**
5. **Sincronización en la nube**
6. **Trabajo con múltiples usuarios**
7. **Sistema de adjuntos de archivos**
8. **Integración con calendario**
9. **Temas y personalización de UI**
10. **Reportes personalizados**

## ?? Información de Contacto

Aplicación desarrollada como proyecto de demostración de buenas prácticas en Windows Forms con .NET 8.

---

**Versión:** 1.0  
**Framework:** .NET 8.0 (Windows)  
**Fecha de creación:** 2024  
**Licencia:** © 2024
