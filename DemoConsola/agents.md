# ?? Guía de agentes de IA en251104-GithubCopilot

![.NET8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=.net&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-9.x-512BD4)
![SQLite](https://img.shields.io/badge/SQLite-3-blue?logo=sqlite&logoColor=white)
![GitHub Copilot](https://img.shields.io/badge/Copilot-Enabled-00B2FF?logo=githubcopilot&logoColor=white)

Documento de referencia sobre cómo se ha utilizado GitHub Copilot y agentes de IA en el desarrollo del proyecto `DemoConsola` del repositorio251104-GithubCopilot.

- Repositorio: https://github.com/hispafox/251104-GithubCopilot
- Proyecto: DemoConsola (Aplicación de gestión de usuarios en .NET8)
- Tecnologías: .NET8, Entity Framework Core, SQLite
- Propósito: Demostración del uso de GitHub Copilot en desarrollo .NET


## 1. Introducción ??
Este documento describe el uso de agentes de IA (principalmente GitHub Copilot y GitHub Copilot Chat) durante el desarrollo del proyecto `DemoConsola`. Se incluyen ejemplos reales del código, mejores prácticas para prompts, atajos útiles en Visual Studio2022 y aprendizajes.


## 2. Agentes utilizados

### GitHub Copilot
- Rol en el proyecto: asistente de autocompletado y generación de bloques de código para interacción por consola, validaciones básicas y acceso a datos con EF Core.
- Ejemplos de código generado (referencias):
 - `DemoConsola/Program.cs`: bucles `while` para validaciones de edad, email y teléfono; método `MostrarResumen`; interacción con EF Core (`EnsureCreated`, `Add`, `SaveChanges`).
 - `DemoConsola/Usuario.cs`: clase `Usuario` con `Id` y propiedades de dominio.
 - `DemoConsola/AppDbContext.cs`: `OnConfiguring` con `UseSqlite` y resolución dinámica de ruta a `usuarios.db`.
- Funcionalidades utilizadas:
 - Autocompletado de código (sugerencias inline)
 - Generación de métodos completos (por ejemplo, `MostrarResumen`)
 - Creación de validaciones (edad, email, teléfono)
 - Documentación XML (resúmenes y comentarios `///` en clases y métodos cuando aplica)

### GitHub Copilot Chat
- Consultas realizadas durante el desarrollo (ejemplos):
 - "¿Cómo validar un email de forma simple en C# para una app de consola?"
 - "Configura EF Core con SQLite en .NET8 y guarda el archivo .db en el directorio del proyecto."
 - "Crea la migración inicial y explica los pasos para aplicarla."
 - "¿Cómo recorrer directorios desde `AppDomain.CurrentDomain.BaseDirectory` hasta encontrar el `.csproj`?"
- Conversaciones útiles (patrones de uso):
 - Prototipos rápidos de validaciones y refactor fino posterior.
 - Explicaciones paso a paso para comandos `dotnet ef`.
- Resolución de problemas específicos:
 - Rutas relativas del archivo SQLite en entornos de ejecución distintos (Debug/Release).
 - Campos no nulos en el modelo y su reflejo en migraciones.

- Copilot vs Copilot Chat (comparativa breve):

| Herramienta | Uso principal | Cuándo usar |
|---|---|---|
| GitHub Copilot | Sugerencias inline y generación de código | Al escribir código repetitivo o boilerplate, validaciones y métodos auxiliares |
| GitHub Copilot Chat | Asistencia conversacional, explicación y comandos | Para dudas, configuración (EF/SQLite), migraciones y resolución de problemas |


## 3. Casos de uso documentados

### `Program.cs`
Archivo: `DemoConsola/Program.cs`

- Generación de lógica de captura de datos y validaciones básicas:
```csharp
Console.Write("Edad: ");
string edadInput = Console.ReadLine() ?? "";
int edad;
while (!int.TryParse(edadInput, out edad) || edad <0)
{
 Console.Write("Edad inválida. Por favor, introduce una edad válida: ");
 edadInput = Console.ReadLine() ?? "";
}

Console.Write("Email: ");
string email = Console.ReadLine() ?? "";
while (!email.Contains("@") || !email.Contains("."))
{
 Console.Write("Email inválido. Por favor, introduce un email válido: ");
 email = Console.ReadLine() ?? "";
}

Console.Write("Número de teléfono: ");
string telefono = Console.ReadLine() ?? "";
while (telefono.Length !=9 || !telefono.All(char.IsDigit))
{
 Console.Write("Número de teléfono inválido. Por favor, introduce un número de teléfono válido (9 dígitos): ");
 telefono = Console.ReadLine() ?? "";
}
```

- Método generado para mostrar un resumen:
```csharp
static void MostrarResumen(string nombre, int edad, string pais, string email, string telefono)
{
 Console.WriteLine("\nResumen de los datos introducidos:");
 Console.WriteLine($"Nombre: {nombre}");
 Console.WriteLine($"Edad: {edad}");
 Console.WriteLine($"País de residencia: {pais}");
 Console.WriteLine($"Email: {email}");
 Console.WriteLine($"Número de teléfono: {telefono}");
 Console.WriteLine("Gracias por proporcionar tus datos.");
}
```

- Interacción con base de datos (EF Core + SQLite):
```csharp
using (var context = new AppDbContext())
{
 context.Database.EnsureCreated();
 var usuario = new Usuario {
 Nombre = nombre,
 Edad = edad,
 Pais = pais,
 Email = email,
 Telefono = telefono
 };
 context.Usuarios.Add(usuario);
 context.SaveChanges();
}
```


### `Usuario.cs`
Archivo: `DemoConsola/Usuario.cs`

- Generación del modelo de datos con anotaciones:
```csharp
public class Usuario
{
 [Key]
 public int Id { get; set; }
 public string Nombre { get; set; }
 public int Edad { get; set; }
 public string Pais { get; set; }
 public string Email { get; set; }
 public string Telefono { get; set; }
}
```
- Validaciones y convenciones:
 - Se usan tipos no anulables (`string`/`int`) que EF Core traduce a columnas `NOT NULL` en la migración.
 - Mejoras sugeridas por Copilot/Chat para robustez: `[Required]`, `[EmailAddress]`, `[Phone]`, `[StringLength]` según necesidades de dominio.
- Ejemplo breve de documentación XML sugerida por Copilot:
```csharp
/// <summary>
/// Entidad de usuario para persistencia en SQLite mediante EF Core.
/// </summary>
public class Usuario { /* ... */ }
```


### `AppDbContext.cs`
Archivo: `DemoConsola/AppDbContext.cs`

- Configuración de contexto EF Core y conexión SQLite con ruta calculada dinámicamente:
```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
 var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
 while (dir != null && !dir.EnumerateFiles("*.csproj").Any())
 {
 dir = dir.Parent;
 }

 string projectDir = dir?.FullName ?? AppDomain.CurrentDomain.BaseDirectory;
 string dbPath = Path.GetFullPath(Path.Combine(projectDir, "usuarios.db"));
 optionsBuilder.UseSqlite($"Data Source={dbPath}");
}
```
- Beneficio: la base de datos se crea siempre en el directorio del proyecto, independientemente del `bin/Debug|Release`.


### Migraciones
- Archivos creados:
 - `DemoConsola/Migrations/20251104105059_Inicial.cs`
 - `DemoConsola/Migrations/20251104105059_Inicial.Designer.cs`
 - `DemoConsola/Migrations/AppDbContextModelSnapshot.cs`
- Comandos generados/ejecutados con ayuda de Copilot Chat:
```powershell
# Crear migración inicial
dotnet ef migrations add Inicial

# Aplicar migraciones a la base de datos
dotnet ef database update
```
- Fragmento de la migración (`Up`):
```csharp
migrationBuilder.CreateTable(
 name: "Usuarios",
 columns: table => new
 {
 Id = table.Column<int>(type: "INTEGER", nullable: false)
 .Annotation("Sqlite:Autoincrement", true),
 Nombre = table.Column<string>(type: "TEXT", nullable: false),
 Edad = table.Column<int>(type: "INTEGER", nullable: false),
 Pais = table.Column<string>(type: "TEXT", nullable: false),
 Email = table.Column<string>(type: "TEXT", nullable: false),
 Telefono = table.Column<string>(type: "TEXT", nullable: false)
 },
 constraints: table =>
 {
 table.PrimaryKey("PK_Usuarios", x => x.Id);
 });
```


## 4. Mejores prácticas ??
- Cómo formular prompts efectivos:
 - Contexto + objetivo + restricciones. Ej.: "Genera un método en C# que valide un teléfono español de9 dígitos sin expresiones regulares."
 - Pide variantes: "Propón2 alternativas con pros/contras".
 - Refactor guiado: "Convierte estos `while` en un método reutilizable `TryReadInt` con reintentos limitados".
- Patrones de uso identificados:
 - Copilot para prototipos rápidos; revisión humana para reglas de negocio y bordes.
 - Extracción de métodos y documentación `///` con sugerencias contextuales.
 - Uso de Copilot Chat para comandos CLI y rutas/entornos.
- Tips específicos para .NET con Copilot:
 - Añade `XML doc` con `///` y deja que Copilot sugiera resúmenes iniciales.
 - Pide "tests sugeridos" para detectar escenarios no cubiertos.
 - Para EF Core, valida siempre las sugerencias de tipos nulos y restricciones.


## 5. Beneficios observados ?
- Reducción de tiempo de desarrollo:
 - Esqueleto de `Program.cs` + validaciones básicas: ~40-60 líneas sugeridas por Copilot en minutos.
 - `AppDbContext` con ruta dinámica a `.csproj`: generado en ~1-2 iteraciones de prompt.
- Mejora en calidad del código:
 - Sugerencias de separación de responsabilidades (método `MostrarResumen`).
 - Convenciones EF Core bien aplicadas (no nulos/PKs).
- Ejemplo cuantitativo (estimaciones):

| Tarea | Manual | Con Copilot |
|---|---:|---:|
| Bucle de validación (3 campos) |15-20 min |3-5 min |
| Configuración EF Core + SQLite |30-45 min |10-15 min |
| Migración inicial |10 min |2-3 min |


## 6. Limitaciones y consideraciones ??
- Copilot puede sugerir validaciones simplificadas (por ejemplo, email con `Contains("@")`). Reforzar con `[EmailAddress]` o expresiones regulares según el caso.
- Revisión humana necesaria para reglas de negocio, mensajes al usuario y tratamiento de datos personales.
- Seguridad: no aceptar credenciales en texto plano ni exponer rutas sensibles; aplicar validaciones de entrada y sanitización.
- EF Core: revisar que los tipos anulables/no anulables reflejen correctamente las reglas de dominio.


## 7. Comandos y atajos útiles ??
- CLI (EF Core):
```powershell
# Instalar herramientas EF (si fuese necesario)
dotnet tool install --global dotnet-ef

# Añadir migración y actualizar BD
dotnet ef migrations add Inicial
dotnet ef database update
```

- Atajos de GitHub Copilot en Visual Studio2022 (por defecto, configurables):
 - Aceptar sugerencia: Tab
 - Siguiente/Anterior sugerencia: Alt+] / Alt+[
 - Forzar/Mostrar sugerencia: Alt+\
 - Abrir Copilot Chat: Ctrl+Alt+/

- Comandos de Chat usados (ejemplos):
 - "Explica esta migración y cómo revertirla"
 - "Refactoriza la validación de teléfono a un método reutilizable con tests"
 - "Configura EF Core con SQLite y ruta relativa al proyecto"

- Referencias a documentación oficial:
 - GitHub Copilot para Visual Studio: https://docs.github.com/copilot/getting-started-with-github-copilot/getting-started-with-github-copilot-in-visual-studio
 - GitHub Copilot Chat: https://docs.github.com/copilot/getting-started-with-github-copilot/getting-started-with-github-copilot-chat
 - EF Core + SQLite: https://learn.microsoft.com/ef/core/providers/sqlite/
 - EF Core Migrations: https://learn.microsoft.com/ef/core/managing-schemas/migrations/


## 8. Recursos adicionales ??
- Tutorial: Consola .NET8 + EF Core (patrones generales): https://learn.microsoft.com/dotnet/
- Buenas prácticas de validación con Data Annotations: https://learn.microsoft.com/aspnet/core/mvc/models/validation#data-annotations
- Comunidad y soporte Copilot: https://github.com/orgs/community/discussions/categories/copilot


---

## Apéndice: Prompts de ejemplo para reproducir
- "Genera un `DbContext` para SQLite que coloque el archivo `usuarios.db` en el directorio del proyecto en tiempo de ejecución."
- "Añade validaciones básicas por consola para edad, email y teléfono."
- "Crea la primera migración EF Core y explica los pasos para actualizar la base de datos."
- "Refactoriza el código de entrada por consola en métodos `TryReadInt`, `TryReadEmail`, `TryReadPhone`."

## Objetivo del documento
1) Guía de referencia para desarrolladores que usan Copilot.
2) Demostración del valor de IA en desarrollo .NET.
3) Documentación técnica del proceso de desarrollo del proyecto.
4) Recurso educativo para aprender a trabajar con GitHub Copilot.
