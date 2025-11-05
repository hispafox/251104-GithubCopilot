# ??? Guía de Uso de Etiquetas

## ?? ¿Qué son las Etiquetas?

Las **etiquetas** son categorías personalizables que puedes asignar a tus tareas para organizarlas mejor. Por ejemplo:
- **Urgente** (rojo) para tareas críticas
- **Trabajo** (amarillo) para tareas laborales
- **Personal** (verde) para tareas personales
- **Proyecto** (azul) para tareas de proyectos específicos

## ?? Funciones Actuales

### ? Lo que YA funciona:

1. **Gestor de Etiquetas** (menú `Gestión` ? `Etiquetas`):
   - ? Crear nuevas etiquetas
   - ? Editar etiquetas existentes
   - ? Eliminar etiquetas
   - ? Asignar colores a las etiquetas

### ?? Lo que FALTA implementar:

1. **Asignar etiquetas a las tareas**:
   - ? No hay interfaz en la ventana "Nueva Tarea"/"Editar Tarea"
   - ? No se muestran las etiquetas en el listado de tareas
   - ? No se pueden filtrar tareas por etiqueta

## ?? Cómo usar las Etiquetas (Estado Actual)

### 1. Crear Etiquetas

1. Abre la aplicación
2. Ve al menú **Gestión** ? **Etiquetas**
3. En el formulario superior:
 - Escribe el **nombre** de la etiqueta (ej: "Urgente")
   - Elige un **color** en formato hexadecimal (ej: #FF0000 para rojo)
   - Haz clic en **Guardar**

### 2. Editar Etiquetas

1. En el Gestor de Etiquetas, busca la etiqueta en la lista
2. Haz clic en **Editar**
3. Modifica el nombre o color
4. Haz clic en **Guardar** (o **Cancelar** para descartar cambios)

### 3. Eliminar Etiquetas

1. En el Gestor de Etiquetas, busca la etiqueta en la lista
2. Haz clic en **Eliminar**
3. Confirma la eliminación

## ?? Próximas Mejoras Recomendadas

Para que el sistema de etiquetas sea completamente funcional, se necesita:

### 1. Selector de Etiquetas en Edición de Tareas
Agregar un control en `TareaEditWindow.xaml` para:
- Mostrar todas las etiquetas disponibles
- Permitir seleccionar múltiples etiquetas (CheckBox o ListBox multiselección)
- Guardar la relación tarea-etiqueta en la base de datos

### 2. Visualización de Etiquetas en el Listado
Mostrar las etiquetas asignadas en cada tarea del listado:
- Como chips de colores
- Con el nombre de la etiqueta
- De forma compacta y visual

### 3. Filtrado por Etiquetas
Agregar filtros en la ventana de listado de tareas:
- Filtrar por una o varias etiquetas
- Combinar con otros filtros (estado, prioridad, etc.)

## ?? Estructura de Datos

Las etiquetas se almacenan en la base de datos SQLite en las siguientes tablas:

- **Etiquetas**: Almacena las etiquetas (Id, Nombre, ColorHex)
- **TareasEtiquetas**: Tabla de relación many-to-many (TareaId, EtiquetaId)
- **Tareas**: Cada tarea tiene una colección `TareaEtiquetas`

## ?? Ejemplos de Uso

### Paleta de Colores Recomendada:

| Etiqueta | Color | Código Hex |
|----------|-------|------------|
| Urgente | ?? Rojo | #DC3545 |
| Importante | ?? Naranja | #FF5733 |
| Trabajo | ?? Amarillo | #FFC107 |
| Personal | ?? Verde | #28A745 |
| Proyecto | ?? Azul | #007BFF |
| Documentación | ?? Púrpura | #6F42C1 |
| Reunión | ?? Marrón | #8B4513 |

## ?? Consejos de Organización

1. **No crees demasiadas etiquetas**: 5-10 etiquetas son suficientes
2. **Usa nombres claros**: "Urgente" es mejor que "Importante Muy"
3. **Colores distintivos**: Usa colores que contrasten fácilmente
4. **Categorización consistente**: Define un sistema y síguelo

## ? Preguntas Frecuentes

**P: ¿Puedo asignar varias etiquetas a una tarea?**  
R: Sí, la base de datos está preparada para relaciones many-to-many, solo falta la interfaz.

**P: ¿Qué pasa si elimino una etiqueta que está asignada a tareas?**  
R: La etiqueta se elimina de todas las tareas automáticamente (Cascade Delete).

**P: ¿Puedo cambiar el color de una etiqueta después de crearla?**  
R: Sí, solo edítala y cambia el color.

**P: ¿Las etiquetas ya vienen pre-cargadas?**  
R: Sí, hay 5 etiquetas por defecto: Urgente, Proyecto, Personal, Trabajo, Importante.

## ?? Soporte

Si necesitas ayuda para implementar las funcionalidades faltantes, consulta la documentación técnica o contacta al desarrollador.
