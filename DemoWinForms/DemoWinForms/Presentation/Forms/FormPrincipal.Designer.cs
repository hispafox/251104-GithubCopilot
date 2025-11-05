namespace DemoWinForms.Presentation.Forms
{
    partial class FormPrincipal
    {
        private System.ComponentModel.IContainer components = null;
   private MenuStrip menuStrip;
     private ToolStripMenuItem archivoToolStripMenuItem;
        private ToolStripMenuItem salirToolStripMenuItem;
        private ToolStripMenuItem ayudaToolStripMenuItem;
private ToolStripMenuItem acercaDeToolStripMenuItem;
        private ToolStrip toolStrip;
        private ToolStripButton btnNueva;
        private ToolStripButton btnEditar;
  private ToolStripButton btnEliminar;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton btnCompletar;
        private ToolStripButton btnRefrescar;
private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private SplitContainer splitContainer;
        private GroupBox grpFiltros;
        private CheckBox chkPendiente;
    private CheckBox chkEnProgreso;
        private CheckBox chkCompletada;
        private CheckBox chkCancelada;
 private Label lblFiltroEstado;
     private Label lblFiltroPrioridad;
     private RadioButton rbTodasPrioridades;
        private RadioButton rbCritica;
        private RadioButton rbAlta;
        private RadioButton rbMedia;
     private RadioButton rbBaja;
  private Label lblBuscar;
        private TextBox txtBuscar;
        private Button btnLimpiarFiltros;
      private DataGridView dgvTareas;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colTitulo;
        private DataGridViewTextBoxColumn colPrioridad;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colCategoria;
      private DataGridViewTextBoxColumn colVencimiento;
        private Label lblTotalTareas;
        private ComboBox cboCategoria;
   private Label lblCategoria;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
  {
        components.Dispose();
     }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
  components = new System.ComponentModel.Container();
 
     // MenuStrip
 menuStrip = new MenuStrip();
 archivoToolStripMenuItem = new ToolStripMenuItem();
       salirToolStripMenuItem = new ToolStripMenuItem();
      ayudaToolStripMenuItem = new ToolStripMenuItem();
         acercaDeToolStripMenuItem = new ToolStripMenuItem();

            // ToolStrip
            toolStrip = new ToolStrip();
            btnNueva = new ToolStripButton();
       btnEditar = new ToolStripButton();
            btnEliminar = new ToolStripButton();
   toolStripSeparator1 = new ToolStripSeparator();
            btnCompletar = new ToolStripButton();
         btnRefrescar = new ToolStripButton();

            // StatusStrip
     statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();

       // SplitContainer
            splitContainer = new SplitContainer();

        // Panel de Filtros
    grpFiltros = new GroupBox();
            lblFiltroEstado = new Label();
            chkPendiente = new CheckBox();
   chkEnProgreso = new CheckBox();
   chkCompletada = new CheckBox();
    chkCancelada = new CheckBox();
          lblFiltroPrioridad = new Label();
            rbTodasPrioridades = new RadioButton();
       rbCritica = new RadioButton();
 rbAlta = new RadioButton();
       rbMedia = new RadioButton();
        rbBaja = new RadioButton();
      lblCategoria = new Label();
   cboCategoria = new ComboBox();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
  btnLimpiarFiltros = new Button();

            // DataGridView
         dgvTareas = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
 colTitulo = new DataGridViewTextBoxColumn();
            colPrioridad = new DataGridViewTextBoxColumn();
    colEstado = new DataGridViewTextBoxColumn();
            colCategoria = new DataGridViewTextBoxColumn();
   colVencimiento = new DataGridViewTextBoxColumn();

    lblTotalTareas = new Label();

      ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
       splitContainer.Panel1.SuspendLayout();
         splitContainer.Panel2.SuspendLayout();
splitContainer.SuspendLayout();
grpFiltros.SuspendLayout();
    ((System.ComponentModel.ISupportInitialize)dgvTareas).BeginInit();
   SuspendLayout();

            // 
      // menuStrip
     // 
        menuStrip.Items.AddRange(new ToolStripItem[] { archivoToolStripMenuItem, ayudaToolStripMenuItem });
        menuStrip.Location = new Point(0, 0);
     menuStrip.Name = "menuStrip";
 menuStrip.Size = new Size(1200, 24);
          menuStrip.TabIndex = 0;

   // 
    // archivoToolStripMenuItem
    // 
       archivoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { salirToolStripMenuItem });
    archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
    archivoToolStripMenuItem.Size = new Size(60, 20);
   archivoToolStripMenuItem.Text = "&Archivo";

            // 
 // salirToolStripMenuItem
   // 
          salirToolStripMenuItem.Name = "salirToolStripMenuItem";
       salirToolStripMenuItem.Size = new Size(180, 22);
            salirToolStripMenuItem.Text = "&Salir";
      salirToolStripMenuItem.Click += SalirToolStripMenuItem_Click;

 // 
      // ayudaToolStripMenuItem
 // 
            ayudaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { acercaDeToolStripMenuItem });
    ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
      ayudaToolStripMenuItem.Size = new Size(53, 20);
         ayudaToolStripMenuItem.Text = "A&yuda";

            // 
          // acercaDeToolStripMenuItem
    // 
    acercaDeToolStripMenuItem.Name = "acercaDeToolStripMenuItem";
            acercaDeToolStripMenuItem.Size = new Size(180, 22);
      acercaDeToolStripMenuItem.Text = "&Acerca de...";
        acercaDeToolStripMenuItem.Click += AcercaDeToolStripMenuItem_Click;

 // 
         // toolStrip
      // 
            toolStrip.Items.AddRange(new ToolStripItem[] { 
     btnNueva, btnEditar, btnEliminar, toolStripSeparator1, btnCompletar, btnRefrescar 
     });
          toolStrip.Location = new Point(0, 24);
      toolStrip.Name = "toolStrip";
   toolStrip.Size = new Size(1200, 25);
            toolStrip.TabIndex = 1;

            // 
            // btnNueva
         // 
btnNueva.Image = null; // Agregar iconos si los tienes
            btnNueva.ImageTransparentColor = Color.Magenta;
     btnNueva.Name = "btnNueva";
   btnNueva.Size = new Size(88, 22);
          btnNueva.Text = "? Nueva Tarea";
            btnNueva.Click += BtnNueva_Click;

       // 
            // btnEditar
            // 
     btnEditar.Image = null;
  btnEditar.ImageTransparentColor = Color.Magenta;
            btnEditar.Name = "btnEditar";
   btnEditar.Size = new Size(67, 22);
            btnEditar.Text = "?? Editar";
            btnEditar.Click += BtnEditar_Click;

   // 
// btnEliminar
 // 
       btnEliminar.Image = null;
  btnEliminar.ImageTransparentColor = Color.Magenta;
            btnEliminar.Name = "btnEliminar";
      btnEliminar.Size = new Size(79, 22);
            btnEliminar.Text = "??? Eliminar";
     btnEliminar.Click += BtnEliminar_Click;

      // 
// toolStripSeparator1
     // 
            toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(6, 25);

     // 
       // btnCompletar
            // 
        btnCompletar.Image = null;
            btnCompletar.ImageTransparentColor = Color.Magenta;
         btnCompletar.Name = "btnCompletar";
            btnCompletar.Size = new Size(87, 22);
            btnCompletar.Text = "? Completar";
     btnCompletar.Click += BtnCompletar_Click;

    // 
    // btnRefrescar
      // 
            btnRefrescar.Image = null;
            btnRefrescar.ImageTransparentColor = Color.Magenta;
            btnRefrescar.Name = "btnRefrescar";
            btnRefrescar.Size = new Size(79, 22);
       btnRefrescar.Text = "?? Refrescar";
            btnRefrescar.Click += BtnRefrescar_Click;

   // 
       // statusStrip
        // 
  statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip.Location = new Point(0, 676);
            statusStrip.Name = "statusStrip";
   statusStrip.Size = new Size(1200, 22);
            statusStrip.TabIndex = 2;

            // 
        // lblStatus
       // 
     lblStatus.Name = "lblStatus";
lblStatus.Size = new Size(39, 17);
            lblStatus.Text = "Listo";

 // 
         // splitContainer
    // 
       splitContainer.Dock = DockStyle.Fill;
       splitContainer.Location = new Point(0, 49);
  splitContainer.Name = "splitContainer";
            splitContainer.Size = new Size(1200, 627);
        splitContainer.SplitterDistance = 250;
   splitContainer.TabIndex = 3;

        // 
   // grpFiltros (Panel1)
      // 
  grpFiltros.Controls.Add(lblFiltroEstado);
         grpFiltros.Controls.Add(chkPendiente);
         grpFiltros.Controls.Add(chkEnProgreso);
     grpFiltros.Controls.Add(chkCompletada);
     grpFiltros.Controls.Add(chkCancelada);
 grpFiltros.Controls.Add(lblFiltroPrioridad);
         grpFiltros.Controls.Add(rbTodasPrioridades);
            grpFiltros.Controls.Add(rbCritica);
grpFiltros.Controls.Add(rbAlta);
         grpFiltros.Controls.Add(rbMedia);
            grpFiltros.Controls.Add(rbBaja);
       grpFiltros.Controls.Add(lblCategoria);
         grpFiltros.Controls.Add(cboCategoria);
            grpFiltros.Controls.Add(lblBuscar);
            grpFiltros.Controls.Add(txtBuscar);
            grpFiltros.Controls.Add(btnLimpiarFiltros);
    grpFiltros.Dock = DockStyle.Fill;
            grpFiltros.Location = new Point(0, 0);
   grpFiltros.Name = "grpFiltros";
            grpFiltros.Padding = new Padding(10);
      grpFiltros.Size = new Size(250, 627);
    grpFiltros.TabIndex = 0;
 grpFiltros.TabStop = false;
            grpFiltros.Text = "?? Filtros";

       // lblFiltroEstado
            lblFiltroEstado.AutoSize = true;
     lblFiltroEstado.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFiltroEstado.Location = new Point(13, 30);
      lblFiltroEstado.Name = "lblFiltroEstado";
lblFiltroEstado.Size = new Size(46, 15);
            lblFiltroEstado.TabIndex = 0;
lblFiltroEstado.Text = "Estado:";

      // chkPendiente
    chkPendiente.AutoSize = true;
   chkPendiente.Checked = true;
            chkPendiente.Location = new Point(13, 50);
         chkPendiente.Name = "chkPendiente";
     chkPendiente.Size = new Size(82, 19);
            chkPendiente.TabIndex = 1;
   chkPendiente.Text = "? Pendiente";
 chkPendiente.CheckedChanged += FiltrosChanged;

            // chkEnProgreso
            chkEnProgreso.AutoSize = true;
  chkEnProgreso.Checked = true;
            chkEnProgreso.Location = new Point(13, 75);
       chkEnProgreso.Name = "chkEnProgreso";
            chkEnProgreso.Size = new Size(98, 19);
            chkEnProgreso.TabIndex = 2;
       chkEnProgreso.Text = "?? En Progreso";
   chkEnProgreso.CheckedChanged += FiltrosChanged;

            // chkCompletada
    chkCompletada.AutoSize = true;
  chkCompletada.Location = new Point(13, 100);
   chkCompletada.Name = "chkCompletada";
          chkCompletada.Size = new Size(100, 19);
   chkCompletada.TabIndex = 3;
          chkCompletada.Text = "? Completada";
chkCompletada.CheckedChanged += FiltrosChanged;

            // chkCancelada
            chkCancelada.AutoSize = true;
       chkCancelada.Location = new Point(13, 125);
        chkCancelada.Name = "chkCancelada";
      chkCancelada.Size = new Size(90, 19);
          chkCancelada.TabIndex = 4;
   chkCancelada.Text = "? Cancelada";
            chkCancelada.CheckedChanged += FiltrosChanged;

          // lblFiltroPrioridad
       lblFiltroPrioridad.AutoSize = true;
    lblFiltroPrioridad.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
lblFiltroPrioridad.Location = new Point(13, 160);
     lblFiltroPrioridad.Name = "lblFiltroPrioridad";
          lblFiltroPrioridad.Size = new Size(63, 15);
   lblFiltroPrioridad.TabIndex = 5;
      lblFiltroPrioridad.Text = "Prioridad:";

         // rbTodasPrioridades
            rbTodasPrioridades.AutoSize = true;
            rbTodasPrioridades.Checked = true;
            rbTodasPrioridades.Location = new Point(13, 180);
      rbTodasPrioridades.Name = "rbTodasPrioridades";
    rbTodasPrioridades.Size = new Size(56, 19);
      rbTodasPrioridades.TabIndex = 6;
      rbTodasPrioridades.TabStop = true;
            rbTodasPrioridades.Text = "Todas";
     rbTodasPrioridades.CheckedChanged += FiltrosChanged;

            // rbCritica
         rbCritica.AutoSize = true;
    rbCritica.Location = new Point(13, 205);
        rbCritica.Name = "rbCritica";
            rbCritica.Size = new Size(75, 19);
       rbCritica.TabIndex = 7;
   rbCritica.Text = "?? Crítica";
rbCritica.CheckedChanged += FiltrosChanged;

            // rbAlta
            rbAlta.AutoSize = true;
  rbAlta.Location = new Point(13, 230);
         rbAlta.Name = "rbAlta";
            rbAlta.Size = new Size(67, 19);
            rbAlta.TabIndex = 8;
            rbAlta.Text = "?? Alta";
      rbAlta.CheckedChanged += FiltrosChanged;

   // rbMedia
    rbMedia.AutoSize = true;
   rbMedia.Location = new Point(13, 255);
      rbMedia.Name = "rbMedia";
   rbMedia.Size = new Size(78, 19);
            rbMedia.TabIndex = 9;
      rbMedia.Text = "?? Media";
    rbMedia.CheckedChanged += FiltrosChanged;

            // rbBaja
         rbBaja.AutoSize = true;
            rbBaja.Location = new Point(13, 280);
            rbBaja.Name = "rbBaja";
          rbBaja.Size = new Size(68, 19);
            rbBaja.TabIndex = 10;
            rbBaja.Text = "?? Baja";
  rbBaja.CheckedChanged += FiltrosChanged;

            // lblCategoria
          lblCategoria.AutoSize = true;
            lblCategoria.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
         lblCategoria.Location = new Point(13, 315);
          lblCategoria.Name = "lblCategoria";
    lblCategoria.Size = new Size(63, 15);
 lblCategoria.TabIndex = 11;
         lblCategoria.Text = "Categoría:";

     // cboCategoria
   cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
   cboCategoria.FormattingEnabled = true;
         cboCategoria.Location = new Point(13, 335);
            cboCategoria.Name = "cboCategoria";
          cboCategoria.Size = new Size(220, 23);
cboCategoria.TabIndex = 12;
      cboCategoria.SelectedIndexChanged += FiltrosChanged;

       // lblBuscar
   lblBuscar.AutoSize = true;
 lblBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
      lblBuscar.Location = new Point(13, 375);
            lblBuscar.Name = "lblBuscar";
     lblBuscar.Size = new Size(47, 15);
            lblBuscar.TabIndex = 13;
 lblBuscar.Text = "Buscar:";

    // txtBuscar
         txtBuscar.Location = new Point(13, 395);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Buscar tareas...";
            txtBuscar.Size = new Size(220, 23);
     txtBuscar.TabIndex = 14;
     txtBuscar.TextChanged += TxtBuscar_TextChanged;

            // btnLimpiarFiltros
    btnLimpiarFiltros.Location = new Point(13, 435);
       btnLimpiarFiltros.Name = "btnLimpiarFiltros";
      btnLimpiarFiltros.Size = new Size(220, 30);
    btnLimpiarFiltros.TabIndex = 15;
            btnLimpiarFiltros.Text = "?? Limpiar Filtros";
            btnLimpiarFiltros.UseVisualStyleBackColor = true;
            btnLimpiarFiltros.Click += BtnLimpiarFiltros_Click;

     splitContainer.Panel1.Controls.Add(grpFiltros);

            // 
   // DataGridView Panel (Panel2)
          // 
            var panel2Content = new Panel();
         panel2Content.Dock = DockStyle.Fill;
            
            dgvTareas.AllowUserToAddRows = false;
   dgvTareas.AllowUserToDeleteRows = false;
 dgvTareas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTareas.BackgroundColor = SystemColors.Window;
     dgvTareas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
   dgvTareas.Columns.AddRange(new DataGridViewColumn[] {
             colId, colTitulo, colPrioridad, colEstado, colCategoria, colVencimiento
   });
            dgvTareas.Dock = DockStyle.Fill;
            dgvTareas.Location = new Point(0, 0);
    dgvTareas.MultiSelect = false;
   dgvTareas.Name = "dgvTareas";
            dgvTareas.ReadOnly = true;
            dgvTareas.RowHeadersVisible = false;
            dgvTareas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
         dgvTareas.Size = new Size(946, 597);
            dgvTareas.TabIndex = 0;
     dgvTareas.CellDoubleClick += DgvTareas_CellDoubleClick;

      // Columns
        colId.DataPropertyName = "Id";
       colId.HeaderText = "ID";
            colId.Name = "colId";
      colId.Visible = false;

 colTitulo.DataPropertyName = "Titulo";
          colTitulo.HeaderText = "Título";
            colTitulo.Name = "colTitulo";
            colTitulo.FillWeight = 40;

      colPrioridad.DataPropertyName = "Prioridad";
 colPrioridad.HeaderText = "Prioridad";
       colPrioridad.Name = "colPrioridad";
            colPrioridad.FillWeight = 15;

     colEstado.DataPropertyName = "Estado";
  colEstado.HeaderText = "Estado";
    colEstado.Name = "colEstado";
       colEstado.FillWeight = 15;

        colCategoria.DataPropertyName = "Categoria";
     colCategoria.HeaderText = "Categoría";
    colCategoria.Name = "colCategoria";
         colCategoria.FillWeight = 15;

       colVencimiento.DataPropertyName = "FechaVencimiento";
    colVencimiento.HeaderText = "Vencimiento";
            colVencimiento.Name = "colVencimiento";
            colVencimiento.FillWeight = 15;

       lblTotalTareas.Dock = DockStyle.Bottom;
      lblTotalTareas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
      lblTotalTareas.Location = new Point(0, 597);
            lblTotalTareas.Name = "lblTotalTareas";
lblTotalTareas.Padding = new Padding(10, 5, 10, 5);
lblTotalTareas.Size = new Size(946, 30);
      lblTotalTareas.TabIndex = 1;
lblTotalTareas.Text = "Total: 0 tareas";
      lblTotalTareas.TextAlign = ContentAlignment.MiddleLeft;

    panel2Content.Controls.Add(dgvTareas);
   panel2Content.Controls.Add(lblTotalTareas);
   splitContainer.Panel2.Controls.Add(panel2Content);

            // 
     // FormPrincipal
          // 
AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
 ClientSize = new Size(1200, 698);
       Controls.Add(splitContainer);
   Controls.Add(toolStrip);
       Controls.Add(menuStrip);
          Controls.Add(statusStrip);
     MainMenuStrip = menuStrip;
      Name = "FormPrincipal";
      StartPosition = FormStartPosition.CenterScreen;
            Text = "?? Gestor de Tareas - .NET 8";
            Load += FormPrincipal_Load;

     splitContainer.Panel1.ResumeLayout(false);
      splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
  splitContainer.ResumeLayout(false);
     grpFiltros.ResumeLayout(false);
      grpFiltros.PerformLayout();
((System.ComponentModel.ISupportInitialize)dgvTareas).EndInit();
     ResumeLayout(false);
            PerformLayout();
        }
    }
}
