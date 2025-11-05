# DemoWinForms.Core

## ?? Biblioteca Compartida para Gestor de Tareas

Este proyecto contiene toda la lógica de negocio, acceso a datos y entidades del dominio que pueden ser compartidas entre diferentes interfaces de usuario (Windows Forms, WPF, etc.).

## ??? Estructura

```
DemoWinForms.Core/
??? Domain/        # Capa de Dominio
?   ??? Entities/          # Entidades del negocio
?   ?   ??? Tarea.cs
?   ?   ??? Usuario.cs
?   ?   ??? Etiqueta.cs
?   ?   ??? TareaEtiqueta.cs
?   ??? Enums/             # Enumeraciones
?       ??? EstadoTarea.cs
?       ??? PrioridadTarea.cs
?
??? Data/       # Capa de Acceso a Datos
?   ??? AppDbContext.cs
?   ??? AppDbContextFactory.cs
?   ??? Repositories/
?     ??? ITareaRepository.cs
?     ??? TareaRepository.cs
?       ??? IEtiquetaRepository.cs
?     ??? EtiquetaRepository.cs
?
??? Business/        # Capa de Lógica de Negocio
?   ??? Services/
?   ?   ??? ITareaService.cs
?   ?   ??? TareaService.cs
?   ??? DTOs/
?       ??? FiltroTareasDto.cs
?     ??? EstadisticasDto.cs
?
??? Common/       # Utilidades Comunes
    ??? Result.cs
    ??? Constants.cs
```

## ?? Paquetes NuGet

- **Microsoft.EntityFrameworkCore.Sqlite** (8.0.11) - Base de datos SQLite
- **Microsoft.EntityFrameworkCore.Tools** (8.0.11) - Herramientas de migraciones
- **Microsoft.EntityFrameworkCore.Design** (8.0.11) - Diseño de migraciones
- **Microsoft.Extensions.DependencyInjection** (8.0.1) - Inyección de dependencias
- **Microsoft.Extensions.Configuration** (8.0.0) - Configuración
- **Microsoft.Extensions.Logging** (8.0.1) - Logging
- **Serilog** (4.1.0) - Logging estructurado
- **FluentValidation** (11.9.0) - Validaciones

## ?? Uso en Proyectos

### Windows Forms (DemoWinForms)
```xml
<ItemGroup>
  <ProjectReference Include="..\DemoWinForms.Core\DemoWinForms.Core.csproj" />
</ItemGroup>
```

### WPF (DemoWpf)
```xml
<ItemGroup>
  <ProjectReference Include="..\DemoWinForms.Core\DemoWinForms.Core.csproj" />
</ItemGroup>
```

## ??? Migraciones de Entity Framework

### Crear nueva migración
```bash
cd DemoWinForms.Core
dotnet ef migrations add NombreMigracion
```

### Aplicar migraciones
Las migraciones se aplican automáticamente al iniciar la aplicación.

## ? Características

- ? **Arquitectura en Capas**: Separación clara de responsabilidades
- ? **Repository Pattern**: Abstracción del acceso a datos
- ? **Service Layer**: Lógica de negocio centralizada
- ? **Result Pattern**: Manejo funcional de errores
- ? **Async/Await**: Operaciones asíncronas
- ? **Logging**: Con Serilog para trazabilidad
- ? **Validaciones**: Con FluentValidation
- ? **Inyección de Dependencias**: Para testabilidad

## ?? Notas

Este proyecto es **independiente de la UI** y puede ser usado por cualquier tipo de aplicación .NET 8 que necesite gestionar tareas.
