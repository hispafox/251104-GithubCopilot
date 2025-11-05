# ??? Comandos Útiles para la Migración

## ?? Fase 1: Completada ?

### Verificar Build
```powershell
# Build de toda la solución
dotnet build

# Build solo del Core
dotnet build DemoWinForms.Core/DemoWinForms.Core.csproj

# Build solo de WinForms
dotnet build DemoWinForms/DemoWinForms.csproj

# Build solo de WPF
dotnet build DemoWpf/DemoWpf.csproj
```

### Verificar Referencias
```powershell
# Ver referencias del proyecto WinForms
dotnet list DemoWinForms/DemoWinForms.csproj reference

# Ver referencias del proyecto WPF
dotnet list DemoWpf/DemoWpf.csproj reference
```

## ?? Fase 2: Configurar Proyecto WPF (SIGUIENTE)

### Paso 1: Agregar Referencia al Core
```powershell
cd DemoWpf
dotnet add reference ..\DemoWinForms.Core\DemoWinForms.Core.csproj
```

### Paso 2: Instalar Paquetes MVVM
```powershell
# MVVM Toolkit de Microsoft
dotnet add package CommunityToolkit.Mvvm --version 8.3.2

# Hosting para DI
dotnet add package Microsoft.Extensions.Hosting --version 8.0.1

# Serilog para WPF
dotnet add package Serilog.Extensions.Hosting --version 8.0.0
dotnet add package Serilog.Sinks.File --version 6.0.0

# Configuración JSON
dotnet add package Microsoft.Extensions.Configuration.Json --version 8.0.1
```

### Paso 3: Verificar Instalación
```powershell
dotnet restore DemoWpf/DemoWpf.csproj
dotnet build DemoWpf/DemoWpf.csproj
```

## ??? Entity Framework - Migraciones

### Crear Nueva Migración (desde Core)
```powershell
cd DemoWinForms.Core
dotnet ef migrations add NombreDeMigracion
```

### Ver Migraciones Existentes
```powershell
cd DemoWinForms.Core
dotnet ef migrations list
```

### Aplicar Migraciones (se hace automático en Program.cs)
```powershell
cd DemoWinForms.Core
dotnet ef database update
```

### Revertir Migración
```powershell
cd DemoWinForms.Core
dotnet ef database update MigracionAnterior
```

### Eliminar Última Migración
```powershell
cd DemoWinForms.Core
dotnet ef migrations remove
```

### Generar Script SQL
```powershell
cd DemoWinForms.Core
dotnet ef migrations script
```

## ?? Testing y Debugging

### Ejecutar WinForms
```powershell
cd DemoWinForms
dotnet run
```

### Ejecutar WPF (cuando esté listo)
```powershell
cd DemoWpf
dotnet run
```

### Limpiar Solución
```powershell
dotnet clean
```

### Restaurar Paquetes
```powershell
dotnet restore
```

### Rebuild Completo
```powershell
dotnet clean
dotnet restore
dotnet build
```

## ?? Información del Proyecto

### Ver Información de la Solución
```powershell
dotnet sln list
```

### Ver Propiedades del Proyecto
```powershell
dotnet msbuild DemoWinForms.Core/DemoWinForms.Core.csproj /t:GetPropertyValue /p:Property=TargetFramework
```

### Ver Paquetes Instalados
```powershell
# Ver paquetes del Core
dotnet list DemoWinForms.Core/DemoWinForms.Core.csproj package

# Ver paquetes de WinForms
dotnet list DemoWinForms/DemoWinForms.csproj package

# Ver paquetes de WPF
dotnet list DemoWpf/DemoWpf.csproj package
```

## ?? Análisis de Código

### Buscar referencias a un namespace
```powershell
# Windows PowerShell
Get-ChildItem -Recurse -Include *.cs | Select-String "using DemoWinForms"
```

### Contar líneas de código
```powershell
# Contar líneas en el Core
(Get-ChildItem -Path DemoWinForms.Core -Recurse -Include *.cs | Get-Content | Measure-Object -Line).Lines

# Contar líneas en WinForms
(Get-ChildItem -Path DemoWinForms -Recurse -Include *.cs | Get-Content | Measure-Object -Line).Lines
```

## ??? Git Comandos Útiles

### Commit de Fase 1
```bash
git add .
git commit -m "? Fase 1: Refactorización completada - Proyecto Core creado"
git push
```

### Crear Branch para Fase 2
```bash
git checkout -b feature/fase2-wpf-setup
```

### Ver Status
```bash
git status
git log --oneline -10
```

## ?? Notas Importantes

### Rutas de Archivos Clave

**Core:**
- `DemoWinForms.Core/DemoWinForms.Core.csproj`
- `DemoWinForms.Core/Data/AppDbContext.cs`
- `DemoWinForms.Core/README.md`

**WinForms:**
- `DemoWinForms/DemoWinForms.csproj`
- `DemoWinForms/Program.cs`
- `DemoWinForms/appsettings.json`

**WPF:**
- `DemoWpf/DemoWpf.csproj`
- `DemoWpf/App.xaml`
- `DemoWpf/App.xaml.cs`

### Base de Datos
- Ubicación: `tareas.db` en el directorio del proyecto
- Provider: SQLite
- Migraciones: Automáticas al iniciar

### Logs
- Ubicación: `logs/app-YYYYMMDD.txt`
- Rotación: Diaria
- Framework: Serilog

## ?? Checklist Fase 2

```
[ ] Agregar referencia al Core en DemoWpf
[ ] Instalar paquetes MVVM (CommunityToolkit.Mvvm)
[ ] Instalar Microsoft.Extensions.Hosting
[ ] Instalar Serilog.Extensions.Hosting
[ ] Crear carpeta Views/
[ ] Crear carpeta ViewModels/
[ ] Crear carpeta Converters/
[ ] Crear carpeta Resources/
[ ] Configurar App.xaml.cs con IHost
[ ] Configurar ServiceCollection
[ ] Copiar appsettings.json
[ ] Verificar build
```

## ?? Referencias

- [Documentación .NET 8](https://docs.microsoft.com/dotnet/core/whats-new/dotnet-8)
- [WPF en .NET](https://docs.microsoft.com/dotnet/desktop/wpf/)
- [MVVM Toolkit](https://learn.microsoft.com/windows/communitytoolkit/mvvm/introduction)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Serilog](https://serilog.net/)

---

**Tip:** Mantén este archivo abierto durante la migración para acceso rápido a comandos.
