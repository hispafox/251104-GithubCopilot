# ?? ¡Sistema de Etiquetas Implementado!

## ? Funcionalidades Completadas

He implementado completamente el sistema de etiquetas en tu aplicación WPF de gestión de tareas. Ahora puedes:

### 1. **Gestionar Etiquetas** ??
- **Crear** nuevas etiquetas con nombre y color personalizado
- **Editar** etiquetas existentes
- **Eliminar** etiquetas que ya no necesites
- **Ver** todas las etiquetas en una lista visual

### 2. **Asignar Etiquetas a Tareas** ???
- Al crear o editar una tarea, verás un selector de etiquetas
- Puedes seleccionar **múltiples etiquetas** para cada tarea
- Las etiquetas se guardan automáticamente al guardar la tarea

### 3. **Visualizar Etiquetas** ??
- Las etiquetas aparecen en el listado de tareas como **chips de colores**
- Cada etiqueta muestra su color distintivo
- El texto de la etiqueta es blanco para mejor contraste

---

## ?? Guía de Uso Paso a Paso

### ??? Paso 1: Crear tus Etiquetas

1. Abre la aplicación
2. Ve al menú **Gestión** ? **Etiquetas**
3. En la parte superior verás un formulario con:
   - **Nombre de la Etiqueta**: Escribe el nombre (ej: "Urgente", "Trabajo", "Personal")
   - **Color**: Ingresa un código de color hexadecimal (ej: `#FF0000` para rojo)
     - Puedes usar herramientas online como [HTML Color Picker](https://htmlcolorcodes.com/)
   - Vista previa del color en el círculo
4. Haz clic en **Guardar** (o **Agregar**)

**Etiquetas Pre-cargadas:**
La aplicación ya tiene 5 etiquetas por defecto:
- ?? **Urgente** (#DC3545)
- ?? **Proyecto** (#007BFF)
- ?? **Personal** (#28A745)
- ?? **Trabajo** (#FFC107)
- ?? **Importante** (#FF5733)

### ?? Paso 2: Editar una Etiqueta

1. En el Gestor de Etiquetas, busca la etiqueta en la lista
2. Haz clic en el botón **Editar**
3. Los datos se cargan en el formulario superior
4. Modifica el nombre o el color
5. Haz clic en **Actualizar**
6. O haz clic en **Cancelar** para descartar los cambios

### ??? Paso 3: Eliminar una Etiqueta

1. En el Gestor de Etiquetas, busca la etiqueta en la lista
2. Haz clic en el botón **Eliminar**
3. **Nota**: Si la etiqueta está asignada a tareas, se eliminará de todas ellas automáticamente (Cascade Delete)

### ?? Paso 4: Asignar Etiquetas a una Tarea

#### Al **Crear una Nueva Tarea**:
1. Desde la ventana principal, haz clic en **Nueva Tarea**
2. Rellena los datos de la tarea:
   - **Título** (obligatorio)
   - **Descripción** (opcional)
   - **Fecha de Vencimiento**
   - **Estado**
   - **Prioridad**
3. Desplázate hacia abajo hasta la sección **Etiquetas**
4. Verás una lista con todas las etiquetas disponibles
5. **Marca las casillas** de las etiquetas que deseas asignar
6. Haz clic en **Guardar**

#### Al **Editar una Tarea Existente**:
1. En el listado de tareas, selecciona una tarea
2. Haz clic en **Editar**
3. Verás las etiquetas ya asignadas marcadas
4. Marca o desmarca las etiquetas que desees
5. Haz clic en **Guardar**

### ??? Paso 5: Ver las Etiquetas en el Listado

1. Ve a **Ver Todas las Tareas** desde la ventana principal
2. En el listado, verás una columna llamada **Etiquetas**
3. Cada tarea muestra sus etiquetas como **chips de colores**
4. Los chips tienen:
 - El **color de fondo** de la etiqueta
   - El **nombre** de la etiqueta en texto blanco
   - Bordes redondeados para mejor diseño

---

## ?? Paleta de Colores Recomendada

Aquí tienes algunos colores que puedes usar para tus etiquetas:

| Color | Nombre | Código Hex | Uso Recomendado |
|-------|--------|------------|-----------------|
| ?? | Rojo | `#DC3545` | Urgente, Crítico |
| ?? | Naranja | `#FF5733` | Importante, Prioridad Alta |
| ?? | Amarillo | `#FFC107` | Advertencia, En Revisión |
| ?? | Verde | `#28A745` | Personal, Completado |
| ?? | Azul | `#007BFF` | Proyecto, Informativo |
| ?? | Púrpura | `#6F42C1` | Documentación, Ideas |
| ?? | Marrón | `#8B4513` | Reunión, Agenda |
| ?? | Rosa | `#E91E63` | Creatividad, Diseño |
| ?? | Cian | `#00BCD4` | Investigación, Aprendizaje |
| ? | Gris | `#6C757D` | Archivado, Baja Prioridad |

**Tip**: Usa colores que tengan buen contraste con el blanco para que el texto sea legible.

---

## ?? Características Técnicas Implementadas

### Frontend (WPF/XAML):
- ? Selector de etiquetas con checkboxes en `TareaEditWindow`
- ? Chips de colores visuales en el `DataGrid`
- ? Conversor `EtiquetasVisibilityConverter` para mostrar/ocultar etiquetas
- ? Scroll vertical en el selector de etiquetas
- ? Ventana redimensionable para mejor experiencia

### Backend (C#):
- ? `EtiquetaSeleccionable` - Clase auxiliar para binding de checkboxes
- ? Carga de etiquetas disponibles al abrir `TareaEditWindow`
- ? Guardado de relaciones many-to-many en `TareaEtiqueta`
- ? Include de etiquetas en consultas de EF Core
- ? Eliminación en cascada de relaciones

### Base de Datos:
- ? Tabla `Etiquetas` (Id, Nombre, ColorHex)
- ? Tabla `TareasEtiquetas` (relación many-to-many)
- ? Índice único en `Etiquetas.Nombre`
- ? 5 etiquetas pre-cargadas por defecto

---

## ?? Solución de Problemas

### ? No veo las etiquetas en el listado
**Solución**: Asegúrate de que la tarea tiene etiquetas asignadas. Si acabas de crear etiquetas, asígnalas editando la tarea.

### ? No puedo guardar una etiqueta
**Posibles causas**:
- El nombre ya existe (debe ser único)
- El color no está en formato hexadecimal válido (ej: `#RRGGBB`)
- El nombre está vacío

### ? Las etiquetas no se guardan al crear una tarea
**Solución**: Verifica que has marcado las casillas de las etiquetas antes de hacer clic en Guardar.

### ? El color no se muestra correctamente
**Solución**: Usa el formato hexadecimal completo con 6 dígitos (ej: `#FF0000` en lugar de `#F00`). Debe empezar con `#`.

---

## ?? Ejemplo de Uso

### Escenario: Organizar un Proyecto de Desarrollo Web

1. **Crear Etiquetas**:
   - ?? Frontend (#007BFF)
   - ?? Backend (#28A745)
   - ?? Testing (#FFC107)
   - ?? Bug (#DC3545)
   - ?? Documentación (#6F42C1)

2. **Crear Tareas con Etiquetas**:
   - "Diseñar página de inicio" ? Frontend, Documentación
   - "Crear API REST" ? Backend
   - "Corregir error de login" ? Bug, Backend
   - "Pruebas de integración" ? Testing, Backend
   - "Documentar API" ? Documentación, Backend

3. **Filtrar y Visualizar**:
   - Todas las tareas de Backend se verán con el chip verde
   - Los bugs se destacarán con el chip rojo
   - Puedes identificar rápidamente el tipo de trabajo

---

## ?? Tips y Mejores Prácticas

### Organización:
- ?? **Usa entre 5-10 etiquetas**: Más de 10 puede volverse confuso
- ?? **Nombres cortos**: "Urgente" es mejor que "Muy Urgente e Importante"
- ?? **Colores distintivos**: Usa colores que contrasten entre sí

### Productividad:
- ?? **Combina etiquetas**: Una tarea puede tener "Urgente" + "Trabajo"
- ?? **Usa estados + etiquetas**: Estado "En Progreso" + Etiqueta "Bug"
- ?? **Agrupa por color**: Asigna un color a cada área de tu vida/trabajo

### Mantenimiento:
- ?? **Revisa tus etiquetas mensualmente**: Elimina las que no uses
- ?? **Renombra si es necesario**: Las etiquetas pueden evolucionar
- ?? **Colores consistentes**: No cambies colores frecuentemente

---

## ?? Recursos Adicionales

### Herramientas de Colores:
- [HTML Color Codes](https://htmlcolorcodes.com/)
- [Coolors](https://coolors.co/) - Generador de paletas
- [Adobe Color](https://color.adobe.com/) - Rueda de colores

### Inspiración:
- [Material Design Colors](https://materialui.co/colors)
- [Flat UI Colors](https://flatuicolors.com/)

---

## ?? ¡Disfruta tu Sistema de Etiquetas!

Ahora tu aplicación de gestión de tareas es mucho más poderosa y visual. Las etiquetas te ayudarán a:
- ? Organizar mejor tus tareas
- ? Identificar rápidamente el tipo de tarea
- ? Filtrar y buscar más eficientemente
- ? Mantener un sistema visual y atractivo

**¡Comienza a etiquetar tus tareas y aumenta tu productividad!** ??

---

### Contacto y Soporte
Si tienes dudas o problemas, consulta:
1. El archivo `GUIA_ETIQUETAS.md` para información básica
2. El código fuente en `ViewModels/TareaEditViewModel.cs`
3. La ventana de etiquetas en `Views/EtiquetaManagerWindow.xaml`

**Versión**: 1.0
**Fecha**: $(Get-Date -Format "dd/MM/yyyy")
**Desarrollado para**: WPF + .NET 8
