# DemoConsola - Gestión de Usuarios

Aplicación de consola en .NET 8 que permite recopilar, validar y almacenar información de usuarios en una base de datos SQLite.

## ?? Descripción

Esta aplicación de consola solicita datos de usuarios (nombre, edad, país, email y teléfono), valida la información introducida y la almacena en una base de datos SQLite utilizando Entity Framework Core. Posteriormente muestra todos los usuarios registrados en la base de datos.

## ? Características

- ? Captura de datos de usuario por consola
- ? Validación de entrada de datos:
  - **Edad**: Debe ser un número entero positivo
  - **Email**: Debe contener "@" y "."
  - **Teléfono**: Debe tener exactamente 9 dígitos numéricos
- ? Persistencia de datos con SQLite
- ? Entity Framework Core para acceso a datos
- ? Visualización de todos los usuarios almacenados

## ??? Tecnologías

- **.NET 8.0**
- **Entity Framework Core 9.0.10**
- **SQLite**

## ?? Dependencias

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.10" />
```

## ?? Instalación

1. Clona el repositorio:
```bash
git clone https://github.com/hispafox/251104-GithubCopilot.git
cd 251104-GithubCopilot/DemoConsola
```

2. Restaura las dependencias:
```bash
dotnet restore
```

3. Compila el proyecto:
```bash
dotnet build
```

## ?? Ejecución

Ejecuta la aplicación con:
```bash
dotnet run --project DemoConsola
```

## ?? Uso

Al ejecutar la aplicación, se te solicitará ingresar la siguiente información:

1. **Nombre**: Tu nombre completo
2. **Edad**: Un número entero positivo
3. **País de residencia**: Tu país
4. **Email**: Dirección de correo válida (debe contener @ y .)
5. **Número de teléfono**: 9 dígitos numéricos

La aplicación validará cada entrada y solicitará correcciones si los datos no cumplen los requisitos.

### Ejemplo de ejecución:

```
Hello, World!
Por favor, introduce los siguientes datos:
Nombre: Juan Pérez
Edad: 30
País de residencia: España
Email: juan.perez@example.com
Número de teléfono: 612345678

Resumen de los datos introducidos:
Nombre: Juan Pérez
Edad: 30
País de residencia: España
Email: juan.perez@example.com
Número de teléfono: 612345678
Gracias por proporcionar tus datos.

Usuarios en la base de datos:
1: Juan Pérez, 30, España, juan.perez@example.com, 612345678
```

## ?? Estructura del Proyecto

```
DemoConsola/
??? Program.cs           # Punto de entrada y lógica principal
??? Usuario.cs           # Modelo de datos Usuario
??? AppDbContext.cs      # Contexto de Entity Framework Core
??? Migrations/        # Migraciones de base de datos
? ??? 20251104105059_Inicial.cs
?   ??? 20251104105059_Inicial.Designer.cs
?   ??? AppDbContextModelSnapshot.cs
??? usuarios.db   # Base de datos SQLite (generada en tiempo de ejecución)
```

## ??? Modelo de Datos

### Usuario
```csharp
public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public string Pais { get; set; }
    public string Email { get; set; }
    public string Telefono { get; set; }
}
```

## ?? Migraciones

El proyecto incluye migraciones de Entity Framework Core. La base de datos se crea automáticamente al ejecutar la aplicación.

Para crear nuevas migraciones:
```bash
dotnet ef migrations add NombreDeLaMigracion --project DemoConsola
```

Para actualizar la base de datos:
```bash
dotnet ef database update --project DemoConsola
```

## ?? Base de Datos

La base de datos SQLite (`usuarios.db`) se crea automáticamente en el directorio del proyecto al ejecutar la aplicación por primera vez.

## ?? Contribuir

Las contribuciones son bienvenidas. Por favor:

1. Haz fork del repositorio
2. Crea una rama para tu feature (`git checkout -b feature/nueva-caracteristica`)
3. Commit tus cambios (`git commit -am 'Añade nueva característica'`)
4. Push a la rama (`git push origin feature/nueva-caracteristica`)
5. Abre un Pull Request

## ?? Licencia

Este proyecto es parte de un repositorio de demostración y aprendizaje.

## ?? Autor

**hispafox**
- GitHub: [@hispafox](https://github.com/hispafox)

---

? Si este proyecto te ha sido útil, considera darle una estrella en GitHub
