using Microsoft.Extensions.Logging;
using DemoWinForms.Business.Services;
using DemoWinForms.Business.DTOs;
using DemoWinForms.Domain.Entities;
using DemoWinForms.Domain.Enums;
using DemoWinForms.Common;

namespace DemoWinForms.Presentation.Forms;

/// <summary>
/// Formulario principal de la aplicación
/// </summary>
public partial class FormPrincipal : Form
{
    private readonly ITareaService _tareaService;
    private readonly ILogger<FormPrincipal> _logger;
    private List<Tarea> _tareasActuales = new();

    public FormPrincipal(ITareaService tareaService, ILogger<FormPrincipal> logger)
    {
_tareaService = tareaService;
    _logger = logger;
        InitializeComponent();
    }

    private async void FormPrincipal_Load(object sender, EventArgs e)
    {
        try
        {
            InicializarCombos();
   await CargarTareasAsync();
            _logger.LogInformation("Formulario principal cargado correctamente");
     }
     catch (Exception ex)
        {
    _logger.LogError(ex, "Error al cargar formulario principal");
      MessageBox.Show($"Error al inicializar la aplicación: {ex.Message}", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void InicializarCombos()
    {
      // Categorías
        cboCategoria.Items.Add("Todas");
        cboCategoria.Items.AddRange(Constants.Categorias.Todas);
        cboCategoria.SelectedIndex = 0;
    }

    private async Task CargarTareasAsync()
    {
    try
        {
        lblStatus.Text = "Cargando tareas...";
            Application.DoEvents();

            var filtros = ObtenerFiltros();
      var resultado = await _tareaService.GetByFiltrosAsync(filtros);

       if (resultado.IsSuccess && resultado.Value != null)
     {
    _tareasActuales = resultado.Value.ToList();
     dgvTareas.DataSource = _tareasActuales;
  
     // Aplicar formato a las filas
   AplicarFormatoFilas();
    
       lblTotalTareas.Text = $"Total: {_tareasActuales.Count} tarea(s)";
       lblStatus.Text = "Listo";
          }
     else
            {
        MessageBox.Show(resultado.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        lblStatus.Text = "Error al cargar tareas";
            }
        }
     catch (Exception ex)
        {
_logger.LogError(ex, "Error al cargar tareas");
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
   }
    }

    private FiltroTareasDto ObtenerFiltros()
    {
        var filtros = new FiltroTareasDto();

        // Filtrar por estado
        var estadosFiltrados = new List<EstadoTarea>();
        if (chkPendiente.Checked) estadosFiltrados.Add(EstadoTarea.Pendiente);
    if (chkEnProgreso.Checked) estadosFiltrados.Add(EstadoTarea.EnProgreso);
        if (chkCompletada.Checked) estadosFiltrados.Add(EstadoTarea.Completada);
        if (chkCancelada.Checked) estadosFiltrados.Add(EstadoTarea.Cancelada);

        // Si no hay ningún estado seleccionado, seleccionar todos
     if (estadosFiltrados.Count == 0)
   {
            filtros.Estado = null; // Mostrará todos
   }
        else if (estadosFiltrados.Count == 1)
        {
filtros.Estado = estadosFiltrados[0];
  }

        // Filtrar por prioridad
    if (rbCritica.Checked) filtros.Prioridad = PrioridadTarea.Critica;
        else if (rbAlta.Checked) filtros.Prioridad = PrioridadTarea.Alta;
        else if (rbMedia.Checked) filtros.Prioridad = PrioridadTarea.Media;
 else if (rbBaja.Checked) filtros.Prioridad = PrioridadTarea.Baja;

        // Filtrar por categoría
if (cboCategoria.SelectedIndex > 0)
        {
            filtros.Categoria = cboCategoria.SelectedItem?.ToString();
        }

  // Búsqueda por texto
     if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
    {
    filtros.BusquedaTexto = txtBuscar.Text;
    }

   filtros.OrdenarPor = "prioridad";
        filtros.TamañoPagina = 0; // Sin paginación por ahora

        return filtros;
    }

    private void AplicarFormatoFilas()
    {
      foreach (DataGridViewRow row in dgvTareas.Rows)
        {
  if (row.DataBoundItem is Tarea tarea)
       {
     // Colorear según prioridad
 switch (tarea.Prioridad)
     {
        case PrioridadTarea.Critica:
       row.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFE6E6");
           break;
        case PrioridadTarea.Alta:
        row.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFF4E6");
         break;
   case PrioridadTarea.Media:
  row.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#E6F7FF");
    break;
          case PrioridadTarea.Baja:
 row.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#E6FFE6");
      break;
    }

 // Tachar si está completada
    if (tarea.Estado == EstadoTarea.Completada)
                {
          row.DefaultCellStyle.Font = new Font(dgvTareas.Font, FontStyle.Strikeout);
        row.DefaultCellStyle.ForeColor = Color.Gray;
      }

            // Resaltar si está vencida
       if (tarea.FechaVencimiento.HasValue && 
      tarea.FechaVencimiento.Value < DateTime.Now && 
       tarea.Estado != EstadoTarea.Completada)
      {
          row.DefaultCellStyle.ForeColor = Color.Red;
             row.DefaultCellStyle.Font = new Font(dgvTareas.Font, FontStyle.Bold);
       }
     }
        }
    }

    private async void BtnNueva_Click(object sender, EventArgs e)
    {
        using var formTarea = Program.ServiceProvider.GetService(typeof(FormTarea)) as FormTarea;
        if (formTarea != null && formTarea.ShowDialog() == DialogResult.OK)
    {
            await CargarTareasAsync();
        }
    }

  private async void BtnEditar_Click(object sender, EventArgs e)
    {
        if (dgvTareas.SelectedRows.Count == 0)
 {
            MessageBox.Show("Por favor, seleccione una tarea para editar.", 
           "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
  return;
        }

        var tarea = dgvTareas.SelectedRows[0].DataBoundItem as Tarea;
        if (tarea == null) return;

        using var formTarea = Program.ServiceProvider.GetService(typeof(FormTarea)) as FormTarea;
        if (formTarea != null)
    {
            formTarea.CargarTarea(tarea.Id);
   if (formTarea.ShowDialog() == DialogResult.OK)
{
                await CargarTareasAsync();
         }
        }
    }

    private async void BtnEliminar_Click(object sender, EventArgs e)
    {
        if (dgvTareas.SelectedRows.Count == 0)
        {
     MessageBox.Show("Por favor, seleccione una tarea para eliminar.", 
       "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var tarea = dgvTareas.SelectedRows[0].DataBoundItem as Tarea;
        if (tarea == null) return;

        var confirmacion = MessageBox.Show(
         $"¿Está seguro de eliminar la tarea '{tarea.Titulo}'?",
       "Confirmar eliminación",
        MessageBoxButtons.YesNo,
   MessageBoxIcon.Question);

 if (confirmacion == DialogResult.Yes)
        {
            var resultado = await _tareaService.EliminarTareaAsync(tarea.Id);
    if (resultado.IsSuccess)
            {
    MessageBox.Show("Tarea eliminada exitosamente.", 
  "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
          await CargarTareasAsync();
  }
      else
            {
    MessageBox.Show(resultado.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private async void BtnCompletar_Click(object sender, EventArgs e)
    {
        if (dgvTareas.SelectedRows.Count == 0)
        {
            MessageBox.Show("Por favor, seleccione una tarea para completar.", 
  "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var tarea = dgvTareas.SelectedRows[0].DataBoundItem as Tarea;
        if (tarea == null) return;

        var resultado = await _tareaService.MarcarComoCompletadaAsync(tarea.Id);
        if (resultado.IsSuccess)
{
            MessageBox.Show("Tarea marcada como completada.", 
    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
         await CargarTareasAsync();
    }
        else
        {
   MessageBox.Show(resultado.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnRefrescar_Click(object sender, EventArgs e)
{
        await CargarTareasAsync();
    }

    private async void FiltrosChanged(object sender, EventArgs e)
    {
        await CargarTareasAsync();
    }

    private System.Threading.Timer? _busquedaTimer;

    private void TxtBuscar_TextChanged(object sender, EventArgs e)
    {
      // Implementar debounce para no buscar en cada tecla
        _busquedaTimer?.Dispose();
      _busquedaTimer = new System.Threading.Timer(async _ =>
        {
   if (InvokeRequired)
     {
                Invoke(async () => await CargarTareasAsync());
    }
            else
  {
        await CargarTareasAsync();
    }
        }, null, 500, Timeout.Infinite);
    }

    private void BtnLimpiarFiltros_Click(object sender, EventArgs e)
    {
        chkPendiente.Checked = true;
    chkEnProgreso.Checked = true;
    chkCompletada.Checked = false;
      chkCancelada.Checked = false;
        rbTodasPrioridades.Checked = true;
        cboCategoria.SelectedIndex = 0;
        txtBuscar.Clear();
    }

    private async void DgvTareas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
    if (e.RowIndex >= 0)
        {
            BtnEditar_Click(sender, e);
        }
    }

    private void SalirToolStripMenuItem_Click(object sender, EventArgs e)
  {
        Application.Exit();
    }

  private void AcercaDeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
 "Gestor de Tareas v1.0\n\n" +
            "Aplicación de escritorio para gestionar tareas\n" +
     "Desarrollado con .NET 8 y WinForms\n\n" +
     "© 2024",
      "Acerca de",
  MessageBoxButtons.OK,
  MessageBoxIcon.Information);
    }
}
