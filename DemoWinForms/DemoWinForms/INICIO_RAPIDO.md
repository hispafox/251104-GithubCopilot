# ?? INICIO RÁPIDO - Gestor de Tareas

## ? Ejecutar la Aplicación (3 pasos)

### 1?? Abrir el Proyecto
```bash
# Opción A: Visual Studio
- Abrir Visual Studio 2022
- File ? Open ? Project/Solution
- Seleccionar: DemoWinForms\DemoWinForms.csproj

# Opción B: Visual Studio Code
- Abrir VS Code
- File ? Open Folder
- Seleccionar: DemoWinForms\
```

### 2?? Restaurar Paquetes (automático)
```bash
# Visual Studio: Se hace automáticamente
# Línea de comandos:
dotnet restore
```

### 3?? Ejecutar
```bash
# Visual Studio: Presionar F5 o click en ?? DemoWinForms
# Línea de comandos:
dotnet run
```

**¡Listo!** La aplicación se abrirá y la base de datos se creará automáticamente.

---

## ?? Primera Vez - Qué Verás

### Al Abrir la Aplicación
? Ventana principal con filtros a la izquierda
? Lista de tareas vacía (o con 1 tarea de ejemplo)
? Barra de herramientas superior
? Menú Archivo y Ayuda

### Base de Datos Creada
?? Ubicación: `DemoWinForms\tareas.db`
?? Tamaño inicial: ~100 KB

Contiene:
- 1 usuario demo
- 5 etiquetas predefinidas
- 1 tarea de bienvenida

---

## ?? Primeros Pasos

### 1. Crear Tu Primera Tarea
1. Click en **"? Nueva Tarea"** (barra superior)
2. Completa:
   - **Título**: "Mi primera tarea"
   - **Descripción**: (opcional)
   - **Prioridad**: Media
   - **Estado**: Pendiente
   - **Categoría**: Personal
   - **Vencimiento**: +7 días
3. Click **"?? Guardar"**

### 2. Probar Filtros
1. Crear 3-4 tareas más con diferentes prioridades
2. En el panel izquierdo:
   - Seleccionar solo "? Pendiente"
   - Probar "?? Crítica" en Prioridad
3. Ver cómo cambia la lista automáticamente

### 3. Editar una Tarea
1. **Doble click** en cualquier tarea
2. Cambiar campos
3. **Guardar**
4. Ver cambios reflejados

### 4. Completar una Tarea
1. Seleccionar una tarea
2. Click **"? Completar"**
3. Ver cómo se tacha y cambia de color

---

## ?? Colores y Significados

### Filas de Tareas
- ?? **Fondo rojo claro**: Prioridad Crítica
- ?? **Fondo naranja claro**: Prioridad Alta
- ?? **Fondo azul claro**: Prioridad Media
- ?? **Fondo verde claro**: Prioridad Baja

### Estados
- ? **Normal**: Pendiente
- ?? **Normal**: En Progreso
- ~~Tachado~~ **Gris**: Completada
- **Rojo Negrita**: Vencida y no completada

---

## ?? Funciones Especiales

### Búsqueda Rápida
Escribe en el cuadro "?? Buscar" ? Se filtran tareas en tiempo real

### Limpiar Filtros
Click en **"?? Limpiar Filtros"** ? Resetea todos los filtros

### Refrescar
Click en **"?? Refrescar"** ? Recarga la lista desde la base de datos

### Atajos
- **Doble Click** en tarea ? Editar
- **Enter** en formulario ? Guardar (si está válido)
- **Esc** en formulario ? Cancelar

---

## ?? Archivos Importantes

```
DemoWinForms/
??? tareas.db              ? Base de datos SQLite
??? appsettings.json    ? Configuración
??? logs/
?   ??? app-YYYY-MM-DD.txt ? Logs diarios
??? README.md    ? Documentación completa
```

---

## ?? Configuración Rápida

### Cambiar Usuario por Defecto
Editar `appsettings.json`:
```json
"Application": {
  "UsuarioPorDefectoId": 1  ? Cambiar este ID
}
```

### Habilitar Logs Detallados
```json
"Logging": {
  "LogLevel": {
    "Default": "Debug"  ? Cambiar de Information a Debug
  }
}
```

---

## ? Problemas Comunes

### "No se puede abrir la base de datos"
**Solución**: Verificar que `tareas.db` no esté abierto en otro programa

### "Error al cargar formulario"
**Solución**: Revisar logs en `DemoWinForms/logs/`

### "Tarea no se guarda"
**Solución**: Verificar campos obligatorios (Título debe tener contenido)

### Resetear Base de Datos
```bash
# 1. Cerrar la aplicación
# 2. Eliminar archivo
rm DemoWinForms/tareas.db

# 3. Ejecutar la aplicación ? Se creará nueva BD
dotnet run
```

---

## ?? Más Información

### Documentación Completa
?? `README.md` - Guía completa del proyecto

### Detalles de Implementación
?? `IMPLEMENTACION_COMPLETA.md` - Resumen técnico

### PRD Original
?? (Consultar la conversación de Copilot)

---

## ?? ¡Listo para Usar!

La aplicación está **100% funcional** y lista para:
- ? Gestionar tareas personales
- ? Organizar proyectos
- ? Filtrar y buscar eficientemente
- ? Visualizar prioridades

**¿Necesitas ayuda?**
- Revisar `README.md`
- Consultar logs en `logs/`
- Abrir issue en GitHub

---

**Versión**: 1.0.0  
**Última actualización**: 2024  
**Tecnología**: .NET 8 + WinForms + EF Core + SQLite
