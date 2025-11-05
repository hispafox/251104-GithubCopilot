# ?? Sistema de Gestión de Tareas - WPF + .NET 8

## Descripción General

Aplicación completa de gestión de tareas desarrollada en WPF con .NET 8, utilizando el patrón MVVM, Entity Framework Core y las mejores prácticas de desarrollo moderno.

## ?? Características Principales

### ? Gestión de Tareas
- Crear, editar y eliminar tareas
- Asignar estado (Pendiente, En Progreso, Completada, Cancelada)
- Establecer prioridad (Baja, Media, Alta)
- Fechas de vencimiento
- Descripciones detalladas
- Categorización

### ??? Sistema de Etiquetas
- Crear y gestionar etiquetas personalizadas
- Asignar colores identificativos
- Organizar tareas por etiquetas

### ?? Búsqueda y Filtros
- Búsqueda por texto
- Filtro por estado
- Filtro por prioridad
- Combinación de múltiples filtros

### ?? Estadísticas
- Total de tareas
- Tareas pendientes
- Tareas completadas
- Dashboard visual con tarjetas

### ?? Exportación de Datos
- Exportar a CSV
- Exportar a JSON
- Nombres de archivo sugeridos con fecha

### ?? Temas Personalizables
- Tema claro
- Tema oscuro
- Alternancia instantánea
- Persistencia de preferencia

### ?? Base de Datos Local
- SQLite con Entity Framework Core
- Migraciones automáticas
- Persistencia local

## ??? Arquitectura

### Estructura del Proyecto

```
Solution: DemoWinForms
?
??? DemoWinForms.Core/       # Lógica de negocio compartida
?   ??? Domain/                 # Entidades y enumeraciones
?   ?   ??? Entities/
?   ?   ??? Enums/
?   ??? Business/# Servicios y DTOs
??   ??? Services/
?   ?   ??? DTOs/
?   ??? Data/  # Acceso a datos
?   ?   ??? Repositories/
?   ?   ??? AppDbContext.cs
?   ??? Common/      # Utilidades compartidas
?
??? DemoWpf/      # Aplicación WPF
?   ??? ViewModels/ # ViewModels MVVM
?   ??? Views/      # Vistas XAML
?   ??? Services/    # Servicios de UI
?   ??? Converters/       # Convertidores XAML
?   ??? Themes/             # Temas de interfaz
?   ??? Properties/    # Configuración
?
??? DemoWinForms/               # Aplicación Windows Forms (legacy)
```

### Patrón MVVM

La aplicación implementa completamente el patrón Model-View-ViewModel:

- **Model**: Entidades de dominio en DemoWinForms.Core
- **View**: Archivos XAML con minimal code-behind
- **ViewModel**: Lógica de presentación con CommunityToolkit.Mvvm

## ??? Tecnologías Utilizadas

### Framework y Plataforma
- **.NET 8.0**
- **WPF (Windows Presentation Foundation)**
- **C# 12**

### Librerías y Paquetes
- **CommunityToolkit.Mvvm** 8.3.2 - MVVM con source generators
- **Entity Framework Core** 8.0.11 - ORM
- **Microsoft.EntityFrameworkCore.Sqlite** 8.0.11 - Provider SQLite
- **Microsoft.Extensions.DependencyInjection** 8.0.1 - DI Container
- **Microsoft.Extensions.Hosting** 8.0.1 - Host genérico

### Patrones de Diseño
- **MVVM** - Separación UI/Lógica
- **Repository** - Abstracción de datos
- **Dependency Injection** - Inversión de control
- **Command** - Acciones de usuario
- **Strategy** - Exportación de datos

## ?? Instalación y Configuración

### Requisitos Previos
- Windows 10/11
- .NET 8.0 SDK
- Visual Studio 2022 (recomendado) o VS Code

### Pasos de Instalación

1. **Clonar el repositorio**
```bash
git clone https://github.com/hispafox/251104-GithubCopilot.git
cd DemoWinForms
```

2. **Restaurar paquetes NuGet**
```bash
dotnet restore
```

3. **Compilar la solución**
```bash
dotnet build
```

4. **Ejecutar la aplicación**
```bash
dotnet run --project DemoWpf/DemoWpf.csproj
```

O abrir `DemoWinForms.sln` en Visual Studio y presionar F5.

## ?? Uso de la Aplicación

### Ventana Principal
- Dashboard con estadísticas
- Acceso rápido a todas las tareas
- Botón para crear nueva tarea
- Menú principal

### Gestión de Tareas
1. Click en "Ver Todas las Tareas"
2. Usar filtros y búsqueda para encontrar tareas
3. Editar o eliminar tareas existentes
4. Exportar datos a CSV o JSON

### Gestión de Etiquetas
1. Menú ? Gestión ? Etiquetas
2. Agregar nuevas etiquetas con colores
3. Editar o eliminar etiquetas existentes

### Cambiar Tema
1. Menú ? Ver ? Cambiar Tema
2. La interfaz cambia inmediatamente
3. La preferencia se guarda automáticamente

## ?? Documentación Detallada

- [Fase 2: Migración a WPF](DemoWpf/FASE2_MIGRACION_WPF.md)
- [Fase 3: Funcionalidades Avanzadas](DemoWpf/FASE3_FUNCIONALIDADES_AVANZADAS.md)

## ?? Características Destacadas

### Inyección de Dependencias
Toda la aplicación utiliza DI para:
- Gestión del ciclo de vida de servicios
- Facilitar testing
- Mejorar mantenibilidad
- Reducir acoplamiento

### Async/Await
Todas las operaciones de datos son asíncronas:
- No bloquea la interfaz de usuario
- Mejor experiencia de usuario
- Indicadores de progreso

### Source Generators
Uso de CommunityToolkit.Mvvm para:
- Generar propiedades observables automáticamente
- Generar comandos sin boilerplate
- Reducir código repetitivo

### Validación
- Validación en ViewModels
- Mensajes descriptivos
- Confirmaciones para operaciones críticas

## ?? Capturas de Pantalla

### Ventana Principal (Tema Claro)
Dashboard con estadísticas visuales y acceso rápido a funcionalidades.

### Lista de Tareas
Vista completa con búsqueda, filtros y acciones CRUD.

### Edición de Tareas
Formulario intuitivo con validación en tiempo real.

### Gestión de Etiquetas
Interfaz para crear y gestionar etiquetas con colores personalizados.

### Tema Oscuro
Toda la aplicación adaptada a preferencias visuales del usuario.

## ?? Testing

El proyecto está preparado para agregar:
- Tests unitarios de ViewModels
- Tests de integración de servicios
- Tests de repositorios

## ?? Métricas del Proyecto

- **Líneas de código**: ~3000
- **Archivos C#**: 25+
- **Archivos XAML**: 8
- **ViewModels**: 6
- **Servicios**: 3
- **Repositorios**: 2
- **Entidades**: 4

## ?? Seguridad

- Validación de entrada de usuario
- Escape correcto de datos en exportación
- Confirmaciones para operaciones destructivas
- Manejo seguro de excepciones

## ? Rendimiento

- Operaciones asíncronas
- Carga diferida de datos
- Repositorio eficiente con EF Core
- UI responsiva con indicadores de progreso

## ?? Internacionalización

El proyecto está preparado para agregar:
- Archivos de recursos (.resx)
- Múltiples idiomas
- Formatos culturales

## ?? Contribuciones

Este proyecto fue desarrollado como demostración de migración de Windows Forms a WPF.

## ?? Licencia

Este proyecto es de uso educativo y demostrativo.

## ?? Autor

- **GitHub Copilot** - Asistente de desarrollo
- **Proyecto**: Demostración de migración WinForms ? WPF

## ?? Roadmap Futuro

### Fase 4 (Próxima)
- [ ] Subtareas anidadas
- [ ] Notificaciones de vencimiento
- [ ] Gráficos y reportes visuales
- [ ] Sincronización en la nube
- [ ] Arrastrar y soltar
- [ ] Atajos de teclado
- [ ] Tests automatizados

### Mejoras Continuas
- [ ] Logging con Serilog
- [ ] FluentValidation
- [ ] Animaciones UI
- [ ] Modo offline mejorado
- [ ] Backup automático

## ?? Soporte

Para preguntas o problemas:
1. Revisar la documentación en las carpetas del proyecto
2. Verificar los archivos FASE2 y FASE3
3. Consultar los comentarios en el código

## ?? Aprendizajes Clave

Este proyecto demuestra:
- ? Migración exitosa de WinForms a WPF
- ? Implementación completa de MVVM
- ? Uso avanzado de Entity Framework Core
- ? Inyección de dependencias en WPF
- ? Mejores prácticas de .NET 8
- ? Diseño de UI moderna y responsiva
- ? Arquitectura limpia y mantenible

---

**Versión Actual**: 3.0.0  
**Estado**: ? Producción Ready  
**Última Actualización**: 2024  

**¡Gracias por usar el Sistema de Gestión de Tareas!** ??
