using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using DemoWinForms.Business.Services;
using DemoWinForms.Domain.Entities;
using DemoWinForms.Domain.Enums;
using DemoWinForms.Common;

namespace DemoWinForms.Presentation.Forms;

/// <summary>
/// Formulario para crear o editar tareas
/// </summary>
public partial class FormTarea : Form
{
    private readonly ITareaService _tareaService;
    private readonly ILogger<FormTarea> _logger;
    private readonly IConfiguration _configuration;
    private int? _tareaId;
    private bool _esEdicion;

    public FormTarea(ITareaService tareaService, ILogger<FormTarea> logger, IConfiguration configuration)
    {
        _tareaService = tareaService;
        _logger = logger;
        _configuration = configuration;
        InitializeComponent();
    }

    private void FormTarea_Load(object sender, EventArgs e)
    {
        try
        {
            InicializarCombos();
     ActualizarContadores();

     if (_esEdicion && _tareaId.HasValue)
            {
      Text = "Editar Tarea";
        CargarDatosTarea();
    }
   else
            {
       Text = "Nueva Tarea";
  EstablecerValoresPorDefecto();
         }
      }
        catch (Exception ex)
    {
     _logger.LogError(ex, "Error al cargar formulario de tarea");
     MessageBox.Show($"Error al cargar el formulario: {ex.Message}", 
    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void InicializarCombos()
    {
        // Prioridad
        cboPrioridad.Items.Add(new ComboBoxItem("Baja", PrioridadTarea.Baja));
        cboPrioridad.Items.Add(new ComboBoxItem("Media", PrioridadTarea.Media));
        cboPrioridad.Items.Add(new ComboBoxItem("Alta", PrioridadTarea.Alta));
        cboPrioridad.Items.Add(new ComboBoxItem("Critica", PrioridadTarea.Critica));
        cboPrioridad.DisplayMember = "Text";
        cboPrioridad.ValueMember = "Value";

  // Estado
    cboEstado.Items.Add(new ComboBoxItem("Pendiente", EstadoTarea.Pendiente));
        cboEstado.Items.Add(new ComboBoxItem("En Progreso", EstadoTarea.EnProgreso));
        cboEstado.Items.Add(new ComboBoxItem("Completada", EstadoTarea.Completada));
        cboEstado.Items.Add(new ComboBoxItem("Cancelada", EstadoTarea.Cancelada));
        cboEstado.DisplayMember = "Text";
        cboEstado.ValueMember = "Value";

        // Categoría
        foreach (var categoria in Constants.Categorias.Todas)
 {
            cboCategoria.Items.Add(categoria);
        }
    }

    private void EstablecerValoresPorDefecto()
  {
        cboPrioridad.SelectedIndex = 1; // Media
        cboEstado.SelectedIndex = 0; // Pendiente
     cboCategoria.SelectedIndex = 1; // Personal
   dtpFechaVencimiento.Value = DateTime.Now.AddDays(7);
    }

    public void CargarTarea(int tareaId)
    {
        _tareaId = tareaId;
        _esEdicion = true;
    }

    private async void CargarDatosTarea()
    {
        if (!_tareaId.HasValue) return;

   try
        {
  var resultado = await _tareaService.GetByIdAsync(_tareaId.Value);
     
            if (resultado.IsSuccess && resultado.Value != null)
       {
         var tarea = resultado.Value;
     
         txtTitulo.Text = tarea.Titulo;
   txtDescripcion.Text = tarea.Descripcion ?? string.Empty;
      
                // Seleccionar prioridad
  foreach (ComboBoxItem item in cboPrioridad.Items)
       {
       if ((PrioridadTarea)item.Value == tarea.Prioridad)
       {
      cboPrioridad.SelectedItem = item;
  break;
      }
                }
       
        // Seleccionar estado
       foreach (ComboBoxItem item in cboEstado.Items)
         {
   if ((EstadoTarea)item.Value == tarea.Estado)
        {
              cboEstado.SelectedItem = item;
                 break;
         }
           }
     
              // Seleccionar categoría
                cboCategoria.SelectedItem = tarea.Categoria;
     
          // Fecha de vencimiento
    if (tarea.FechaVencimiento.HasValue)
              {
  dtpFechaVencimiento.Value = tarea.FechaVencimiento.Value;
    chkSinVencimiento.Checked = false;
                }
                else
        {
   chkSinVencimiento.Checked = true;
     }
          }
       else
  {
         MessageBox.Show("No se pudo cargar la tarea.", "Error", 
    MessageBoxButtons.OK, MessageBoxIcon.Error);
           Close();
 }
        }
        catch (Exception ex)
        {
        _logger.LogError(ex, "Error al cargar tarea {TareaId}", _tareaId);
            MessageBox.Show($"Error al cargar la tarea: {ex.Message}", 
           "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }
    }

    private async void BtnGuardar_Click(object sender, EventArgs e)
    {
        if (!ValidarFormulario())
        {
 return;
        }

        try
{
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;

            var usuarioPorDefecto = 1; // Valor por defecto
            if (int.TryParse(_configuration["Application:UsuarioPorDefectoId"], out int userId))
       {
    usuarioPorDefecto = userId;
          }

            var tarea = new Tarea
  {
   Id = _tareaId ?? 0,
    Titulo = txtTitulo.Text.Trim(),
 Descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim(),
      Prioridad = (PrioridadTarea)((ComboBoxItem)cboPrioridad.SelectedItem).Value,
      Estado = (EstadoTarea)((ComboBoxItem)cboEstado.SelectedItem).Value,
           Categoria = cboCategoria.SelectedItem?.ToString() ?? "Personal",
          FechaVencimiento = chkSinVencimiento.Checked ? null : dtpFechaVencimiento.Value,
       UsuarioId = usuarioPorDefecto
    };

       if (_esEdicion && _tareaId.HasValue)
  {
                var resultado = await _tareaService.ActualizarTareaAsync(tarea);
   if (resultado.IsSuccess)
       {
    MessageBox.Show("Tarea actualizada exitosamente.", "Éxito", 
           MessageBoxButtons.OK, MessageBoxIcon.Information);
   DialogResult = DialogResult.OK;
       Close();
       }
  else
    {
  MessageBox.Show(resultado.Error, "Error", 
    MessageBoxButtons.OK, MessageBoxIcon.Error);
   }
     }
      else
 {
      var resultado = await _tareaService.CrearTareaAsync(tarea);
       if (resultado.IsSuccess)
    {
MessageBox.Show("Tarea creada exitosamente.", "Éxito", 
          MessageBoxButtons.OK, MessageBoxIcon.Information);
           DialogResult = DialogResult.OK;
     Close();
       }
      else
   {
       MessageBox.Show(resultado.Error, "Error", 
 MessageBoxButtons.OK, MessageBoxIcon.Error);
}
            }
     }
   catch (Exception ex)
        {
     _logger.LogError(ex, "Error al guardar tarea");
        MessageBox.Show($"Error al guardar la tarea: {ex.Message}", 
       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
   }
        finally
        {
       btnGuardar.Enabled = true;
    btnCancelar.Enabled = true;
        }
    }

    private bool ValidarFormulario()
    {
        if (string.IsNullOrWhiteSpace(txtTitulo.Text))
        {
   MessageBox.Show("El título es obligatorio.", "Validación", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtTitulo.Focus();
       return false;
 }

    if (txtTitulo.Text.Length > 200)
    {
   MessageBox.Show("El título no puede exceder 200 caracteres.", "Validación", 
      MessageBoxButtons.OK, MessageBoxIcon.Warning);
txtTitulo.Focus();
         return false;
   }

        if (txtDescripcion.Text.Length > 2000)
        {
     MessageBox.Show("La descripción no puede exceder 2000 caracteres.", "Validación", 
             MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtDescripcion.Focus();
     return false;
        }

      if (cboPrioridad.SelectedIndex < 0)
        {
            MessageBox.Show("Debe seleccionar una prioridad.", "Validación", 
     MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cboPrioridad.Focus();
return false;
        }

        if (cboEstado.SelectedIndex < 0)
        {
        MessageBox.Show("Debe seleccionar un estado.", "Validación", 
          MessageBoxButtons.OK, MessageBoxIcon.Warning);
    cboEstado.Focus();
            return false;
     }

   if (cboCategoria.SelectedIndex < 0)
        {
  MessageBox.Show("Debe seleccionar una categoría.", "Validación", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cboCategoria.Focus();
            return false;
        }

        if (!chkSinVencimiento.Checked && dtpFechaVencimiento.Value.Date < DateTime.Now.Date)
   {
            MessageBox.Show("La fecha de vencimiento no puede ser anterior a hoy.", "Validación", 
          MessageBoxButtons.OK, MessageBoxIcon.Warning);
     dtpFechaVencimiento.Focus();
 return false;
        }

        return true;
    }

    private void BtnCancelar_Click(object sender, EventArgs e)
    {
 DialogResult = DialogResult.Cancel;
        Close();
    }

    private void TxtTitulo_TextChanged(object sender, EventArgs e)
  {
        ActualizarContadores();
    }

    private void TxtDescripcion_TextChanged(object sender, EventArgs e)
    {
        ActualizarContadores();
    }

    private void ActualizarContadores()
    {
        lblContadorTitulo.Text = $"{txtTitulo.Text.Length} / 200";
        lblContadorDescripcion.Text = $"{txtDescripcion.Text.Length} / 2000";

     // Cambiar color si se acerca al límite
   lblContadorTitulo.ForeColor = txtTitulo.Text.Length > 180 ? Color.Red : Color.Gray;
      lblContadorDescripcion.ForeColor = txtDescripcion.Text.Length > 1800 ? Color.Red : Color.Gray;
    }

    private void ChkSinVencimiento_CheckedChanged(object sender, EventArgs e)
    {
  dtpFechaVencimiento.Enabled = !chkSinVencimiento.Checked;
    }

    /// <summary>
    /// Clase auxiliar para items de ComboBox con valor personalizado
    /// </summary>
    private class ComboBoxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

public ComboBoxItem(string text, object value)
        {
         Text = text;
      Value = value;
}

        public override string ToString()
    {
         return Text;
        }
    }
}
