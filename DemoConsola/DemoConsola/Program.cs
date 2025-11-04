using DemoConsola;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// Esta aplicación tiene como objetivo pedir los datos de usuarios y mostrarlos por pantalla
// Se pide el nombre, la edad y el país de residencia, el email y el número de teléfono
// Validar que la edad es un número entero positivo
// Validar que el email contiene un "@" y un "."
// Validar que el número de teléfono contiene solo números y tiene una longitud de 9 dígitos
// Mostrar un resumen de los datos introducidos al finalizar
Console.WriteLine("Por favor, introduce los siguientes datos:");
Console.Write("Nombre: ");
string nombre = Console.ReadLine() ?? "";
Console.Write("Edad: ");
string edadInput = Console.ReadLine() ?? "";
int edad;
while (!int.TryParse(edadInput, out edad) || edad < 0)
{
    Console.Write("Edad inválida. Por favor, introduce una edad válida: ");
    edadInput = Console.ReadLine() ?? "";
}
Console.Write("País de residencia: ");
string pais = Console.ReadLine() ?? "";
Console.Write("Email: ");
string email = Console.ReadLine() ?? "";
while (!email.Contains("@") || !email.Contains("."))
{
    Console.Write("Email inválido. Por favor, introduce un email válido: ");
    email = Console.ReadLine() ?? "";
}
Console.Write("Número de teléfono: ");
string telefono = Console.ReadLine() ?? "";
while (telefono.Length != 9 || !telefono.All(char.IsDigit))
{
    Console.Write("Número de teléfono inválido. Por favor, introduce un número de teléfono válido (9 dígitos): ");
    telefono = Console.ReadLine() ?? "";
}
MostrarResumen(nombre, edad, pais, email, telefono);

// Guardar usuario en la base de datos SQLite usando EF Core
using (var context = new AppDbContext())
{
    context.Database.EnsureCreated(); // Crea la BD si no existe
    var usuario = new Usuario {
        Nombre = nombre,
        Edad = edad,
        Pais = pais,
        Email = email,
        Telefono = telefono
    };
    context.Usuarios.Add(usuario);
    context.SaveChanges();
    Console.WriteLine("\nUsuarios en la base de datos:");
    foreach (var u in context.Usuarios)
    {
        Console.WriteLine($"{u.Id}: {u.Nombre}, {u.Edad}, {u.Pais}, {u.Email}, {u.Telefono}");
    }
}


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

