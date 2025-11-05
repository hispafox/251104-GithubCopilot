namespace DemoWinForms.Presentation.Forms
{
    partial class FormTarea
    {
        private System.ComponentModel.IContainer components = null;
private Label lblTitulo;
     private TextBox txtTitulo;
        private Label lblDescripcion;
        private RichTextBox txtDescripcion;
        private Label lblPrioridad;
        private ComboBox cboPrioridad;
        private Label lblEstado;
        private ComboBox cboEstado;
        private Label lblCategoria;
        private ComboBox cboCategoria;
      private Label lblFechaVencimiento;
      private DateTimePicker dtpFechaVencimiento;
        private CheckBox chkSinVencimiento;
    private Button btnGuardar;
        private Button btnCancelar;
     private Label lblContadorTitulo;
        private Label lblContadorDescripcion;

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

 lblTitulo = new Label();
         txtTitulo = new TextBox();
   lblContadorTitulo = new Label();
      lblDescripcion = new Label();
            txtDescripcion = new RichTextBox();
            lblContadorDescripcion = new Label();
  lblPrioridad = new Label();
            cboPrioridad = new ComboBox();
            lblEstado = new Label();
            cboEstado = new ComboBox();
  lblCategoria = new Label();
            cboCategoria = new ComboBox();
 lblFechaVencimiento = new Label();
    dtpFechaVencimiento = new DateTimePicker();
  chkSinVencimiento = new CheckBox();
            btnGuardar = new Button();
    btnCancelar = new Button();

     SuspendLayout();

            // lblTitulo
 lblTitulo.AutoSize = true;
lblTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 20);
   lblTitulo.Name = "lblTitulo";
     lblTitulo.Size = new Size(54, 15);
    lblTitulo.TabIndex = 0;
         lblTitulo.Text = "Título: *";

    // txtTitulo
         txtTitulo.Location = new Point(20, 40);
   txtTitulo.MaxLength = 200;
   txtTitulo.Name = "txtTitulo";
       txtTitulo.Size = new Size(520, 23);
  txtTitulo.TabIndex = 1;
            txtTitulo.TextChanged += TxtTitulo_TextChanged;

         // lblContadorTitulo
            lblContadorTitulo.Location = new Point(420, 20);
            lblContadorTitulo.Name = "lblContadorTitulo";
  lblContadorTitulo.Size = new Size(120, 15);
  lblContadorTitulo.TabIndex = 2;
 lblContadorTitulo.Text = "0 / 200";
 lblContadorTitulo.TextAlign = ContentAlignment.TopRight;

     // lblDescripcion
          lblDescripcion.AutoSize = true;
       lblDescripcion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
   lblDescripcion.Location = new Point(20, 80);
    lblDescripcion.Name = "lblDescripcion";
  lblDescripcion.Size = new Size(80, 15);
   lblDescripcion.TabIndex = 3;
            lblDescripcion.Text = "Descripción:";

// txtDescripcion
  txtDescripcion.Location = new Point(20, 100);
            txtDescripcion.MaxLength = 2000;
            txtDescripcion.Name = "txtDescripcion";
      txtDescripcion.Size = new Size(520, 120);
         txtDescripcion.TabIndex = 4;
     txtDescripcion.Text = "";
 txtDescripcion.TextChanged += TxtDescripcion_TextChanged;

        // lblContadorDescripcion
            lblContadorDescripcion.Location = new Point(420, 80);
          lblContadorDescripcion.Name = "lblContadorDescripcion";
         lblContadorDescripcion.Size = new Size(120, 15);
       lblContadorDescripcion.TabIndex = 5;
  lblContadorDescripcion.Text = "0 / 2000";
       lblContadorDescripcion.TextAlign = ContentAlignment.TopRight;

            // lblPrioridad
   lblPrioridad.AutoSize = true;
            lblPrioridad.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPrioridad.Location = new Point(20, 240);
       lblPrioridad.Name = "lblPrioridad";
        lblPrioridad.Size = new Size(66, 15);
          lblPrioridad.TabIndex = 6;
            lblPrioridad.Text = "Prioridad:";

            // cboPrioridad
  cboPrioridad.DropDownStyle = ComboBoxStyle.DropDownList;
   cboPrioridad.FormattingEnabled = true;
            cboPrioridad.Location = new Point(20, 260);
          cboPrioridad.Name = "cboPrioridad";
            cboPrioridad.Size = new Size(150, 23);
            cboPrioridad.TabIndex = 7;

         // lblEstado
            lblEstado.AutoSize = true;
            lblEstado.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
 lblEstado.Location = new Point(200, 240);
            lblEstado.Name = "lblEstado";
     lblEstado.Size = new Size(49, 15);
            lblEstado.TabIndex = 8;
lblEstado.Text = "Estado:";

            // cboEstado
          cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.FormattingEnabled = true;
    cboEstado.Location = new Point(200, 260);
      cboEstado.Name = "cboEstado";
    cboEstado.Size = new Size(150, 23);
    cboEstado.TabIndex = 9;

        // lblCategoria
        lblCategoria.AutoSize = true;
    lblCategoria.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
          lblCategoria.Location = new Point(390, 240);
        lblCategoria.Name = "lblCategoria";
lblCategoria.Size = new Size(66, 15);
        lblCategoria.TabIndex = 10;
            lblCategoria.Text = "Categoría:";

          // cboCategoria
  cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
          cboCategoria.FormattingEnabled = true;
    cboCategoria.Location = new Point(390, 260);
        cboCategoria.Name = "cboCategoria";
     cboCategoria.Size = new Size(150, 23);
    cboCategoria.TabIndex = 11;

            // lblFechaVencimiento
            lblFechaVencimiento.AutoSize = true;
            lblFechaVencimiento.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFechaVencimiento.Location = new Point(20, 300);
      lblFechaVencimiento.Name = "lblFechaVencimiento";
       lblFechaVencimiento.Size = new Size(127, 15);
            lblFechaVencimiento.TabIndex = 12;
            lblFechaVencimiento.Text = "Fecha de vencimiento:";

     // dtpFechaVencimiento
   dtpFechaVencimiento.Format = DateTimePickerFormat.Short;
            dtpFechaVencimiento.Location = new Point(20, 320);
  dtpFechaVencimiento.Name = "dtpFechaVencimiento";
  dtpFechaVencimiento.Size = new Size(250, 23);
      dtpFechaVencimiento.TabIndex = 13;

  // chkSinVencimiento
          chkSinVencimiento.AutoSize = true;
  chkSinVencimiento.Location = new Point(290, 320);
      chkSinVencimiento.Name = "chkSinVencimiento";
     chkSinVencimiento.Size = new Size(114, 19);
            chkSinVencimiento.TabIndex = 14;
            chkSinVencimiento.Text = "Sin vencimiento";
          chkSinVencimiento.UseVisualStyleBackColor = true;
            chkSinVencimiento.CheckedChanged += ChkSinVencimiento_CheckedChanged;

// btnGuardar
            btnGuardar.BackColor = Color.FromArgb(0, 120, 215);
  btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
          btnGuardar.Location = new Point(340, 380);
      btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(100, 35);
    btnGuardar.TabIndex = 15;
            btnGuardar.Text = "?? Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += BtnGuardar_Click;

 // btnCancelar
            btnCancelar.Location = new Point(450, 380);
            btnCancelar.Name = "btnCancelar";
   btnCancelar.Size = new Size(100, 35);
 btnCancelar.TabIndex = 16;
      btnCancelar.Text = "? Cancelar";
      btnCancelar.UseVisualStyleBackColor = true;
       btnCancelar.Click += BtnCancelar_Click;

  // FormTarea
       AutoScaleDimensions = new SizeF(7F, 15F);
         AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(564, 435);
       Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(chkSinVencimiento);
          Controls.Add(dtpFechaVencimiento);
     Controls.Add(lblFechaVencimiento);
            Controls.Add(cboCategoria);
         Controls.Add(lblCategoria);
            Controls.Add(cboEstado);
            Controls.Add(lblEstado);
 Controls.Add(cboPrioridad);
  Controls.Add(lblPrioridad);
            Controls.Add(lblContadorDescripcion);
      Controls.Add(txtDescripcion);
            Controls.Add(lblDescripcion);
            Controls.Add(lblContadorTitulo);
     Controls.Add(txtTitulo);
    Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
        MinimizeBox = false;
            Name = "FormTarea";
            StartPosition = FormStartPosition.CenterParent;
       Text = "Nueva Tarea";
 Load += FormTarea_Load;
  ResumeLayout(false);
            PerformLayout();
    }
    }
}
