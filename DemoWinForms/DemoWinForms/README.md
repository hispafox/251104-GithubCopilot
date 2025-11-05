# ?? Gestor de Tareas - WinForms .NET 8

Sistema completo de gestión de tareas implementado con WinForms, Entity Framework Core y SQLite.

## ?? Características Implementadas

### ? Funcionalidades Core
- ? **CRUD Completo de Tareas** (Crear, Leer, Actualizar, Eliminar)
- ? **Sistema de Filtros Avanzado**
  - Filtro por Estado (Pendiente, En Progreso, Completada, Cancelada)
  - Filtro por Prioridad (Baja, Media, Alta, Crítica)
  - Filtro por Categoría
  - Búsqueda por texto en título y descripción
- ? **Gestión de Prioridades** con indicadores visuales por colores
- ? **Categorías Personalizables**
- ? **Fechas de Vencimiento**
- ? **Marcar tareas como completadas**
- ? **Eliminación lógica** (soft delete)
- ? **Interfaz intuitiva** con DataGridView

### ??? Arquitectura
- **Arquitectura en Capas**:
  - `Presentation`: Formularios WinForms
  - `Business`: Servicios y lógica de negocio
  - `Data`: Repositorios y DbContext
  - `Domain`: Entidades y enums
  - `Common`: Utilidades compartidas

- **Patrones de Diseño**:
  - Repository Pattern
  - Dependency Injection
  - Result Pattern para manejo de errores

### ??? Tecnologías
- **.NET 8** (Windows Forms)
- **Entity Framework Core 8.0.11** con SQLite
- **Serilog** para logging
- **FluentValidation** para validaciones
- **Dependency Injection** nativo de Microsoft

## ?? Instalación y Configuración

### Prerrequisitos
- Visual Studio 2022 o superior
- .NET 8 SDK
- Windows 10/11

### Pasos de Instalación

1. **Clonar el repositorio**
```bash
git clone https://github.com/hispafox/251104-GithubCopilot
cd 251104-GithubCopilot/DemoWinForms
```

2. **Restaurar paquetes NuGet**
```bash
dotnet restore
```

3. **Aplicar migraciones (crear la base de datos)**
```bash
dotnet ef migrations add InitialCreate --project DemoWinForms
dotnet ef database update --project DemoWinForms
```

4. **Compilar el proyecto**
```bash
dotnet build
```

5. **Ejecutar la aplicación**
```bash
dotnet run --project DemoWinForms
```

O simplemente presionar **F5** en Visual Studio.

## ?? Estructura del Proyecto

```
DemoWinForms/
??? Business/
?   ??? DTOs/
?   ?   ??? FiltroTareasDto.cs
?   ?   ??? EstadisticasDto.cs
?   ??? Services/
?       ??? ITareaService.cs
?       ??? TareaService.cs
??? Common/
?   ??? Constants.cs
?   ??? Result.cs
??? Data/
?   ??? Repositories/
?   ?   ??? ITareaRepository.cs
?   ?   ??? TareaRepository.cs
?   ?   ??? IEtiquetaRepository.cs
?   ?   ??? EtiquetaRepository.cs
???? AppDbContext.cs
??? Domain/
?   ??? Entities/
?   ?   ??? Tarea.cs
?   ?   ??? Usuario.cs
?   ?   ??? Etiqueta.cs
?   ?   ??? TareaEtiqueta.cs
?   ??? Enums/
?       ??? PrioridadTarea.cs
?     ??? EstadoTarea.cs
??? Presentation/
?   ??? Forms/
?       ??? FormPrincipal.cs
?    ??? FormPrincipal.Designer.cs
?       ??? FormTarea.cs
?       ??? FormTarea.Designer.cs
??? appsettings.json
??? Program.cs
??? DemoWinForms.csproj
```

## ?? Interfaz de Usuario

### Pantalla Principal
- **Barra de Menú**: Archivo, Ayuda
- **Barra de Herramientas**: Nueva, Editar, Eliminar, Completar, Refrescar
- **Panel Izquierdo**: Filtros avanzados
- **Panel Central**: Lista de tareas en DataGridView
- **Barra de Estado**: Información del sistema

### Colores de Prioridad
- ?? **Crítica**: Rojo claro (#FFE6E6)
- ?? **Alta**: Amarillo claro (#FFF4E6)
- ?? **Media**: Azul claro (#E6F7FF)
- ?? **Baja**: Verde claro (#E6FFE6)

### Formulario de Tarea
- Campos: Título, Descripción, Prioridad, Estado, Categoría, Fecha de Vencimiento
- Validación en tiempo real
- Contadores de caracteres
- Opción "Sin vencimiento"

## ?? Base de Datos

### Tablas
1. **Usuarios**: Almacena información de usuarios
2. **Tareas**: Tareas con todas sus propiedades
3. **Etiquetas**: Etiquetas reutilizables
4. **TareasEtiquetas**: Relación many-to-many

### Modelo de Datos
```
Usuario (1) ----< (N) Tarea
Tarea (1) ----< (N) Tarea (Subtareas)
Tarea (N) ----< TareaEtiqueta >---- (N) Etiqueta
```

## ?? Configuración

Editar `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=tareas.db"
  },
  "Application": {
    "MaxTareasPorPagina": 50,
    "BackupAutomaticoHoras": 24,
    "UsuarioPorDefectoId": 1
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## ?? Desarrollo

### Agregar Nueva Migración
```bash
dotnet ef migrations add NombreDeLaMigracion --project DemoWinForms
dotnet ef database update --project DemoWinForms
```

### Ver Logs
Los logs se generan en: `DemoWinForms/logs/app-YYYY-MM-DD.txt`

### Testing
```bash
dotnet test
```

## ?? Uso de la Aplicación

### Crear una Tarea
1. Click en "? Nueva Tarea" en la barra de herramientas
2. Completar el formulario
3. Click en "?? Guardar"

### Editar una Tarea
1. Seleccionar la tarea en la lista
2. Click en "?? Editar" o doble click en la tarea
3. Modificar campos deseados
4. Click en "?? Guardar"

### Eliminar una Tarea
1. Seleccionar la tarea
2. Click en "??? Eliminar"
3. Confirmar la eliminación

### Completar una Tarea
1. Seleccionar la tarea
2. Click en "? Completar"

### Filtrar Tareas
- Usar checkboxes para filtrar por estado
- Usar radio buttons para filtrar por prioridad
- Seleccionar categoría del dropdown
- Escribir en el cuadro de búsqueda

## ?? Solución de Problemas

### Error: "No se puede conectar a la base de datos"
- Verificar que el archivo `tareas.db` existe
- Ejecutar `dotnet ef database update`

### Error: "Paquetes NuGet faltantes"
- Ejecutar `dotnet restore`
- Verificar conexión a internet

### La aplicación no inicia
- Verificar que .NET 8 está instalado
- Revisar logs en `DemoWinForms/logs/`

## ?? Mejoras Futuras

### Fase 2 (Próximamente)
- [ ] Subtareas
- [ ] Sistema de etiquetas completo
- [ ] Exportar a Excel/CSV
- [ ] Estadísticas y dashboards
- [ ] Notificaciones de tareas próximas a vencer
- [ ] Historial de cambios (auditoría)

### Fase 3
- [ ] Búsqueda avanzada con operadores
- [ ] Filtros guardados
- [ ] Temas personalizables
- [ ] Atajos de teclado configurables
- [ ] Sincronización con la nube

## ?? Contribuir

1. Fork el proyecto
2. Crear una rama (`git checkout -b feature/NuevaCaracteristica`)
3. Commit cambios (`git commit -m 'Agregar NuevaCaracteristica'`)
4. Push a la rama (`git push origin feature/NuevaCaracteristica`)
5. Abrir un Pull Request

## ?? Licencia

Este proyecto es parte de un ejercicio educativo.

## ?? Autor

Desarrollado como demostración de implementación de un PRD completo con .NET 8 y WinForms.

---

## ?? Soporte

Si encuentras algún problema o tienes sugerencias, por favor abre un issue en GitHub.

**¡Gracias por usar el Gestor de Tareas!** ??
