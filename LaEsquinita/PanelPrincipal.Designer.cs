namespace LaEsquinita
{
    partial class PanelPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvMovimientos = new System.Windows.Forms.DataGridView();
            this.rbCompras = new System.Windows.Forms.RadioButton();
            this.rbVentas = new System.Windows.Forms.RadioButton();
            this.rbDevolucionC = new System.Windows.Forms.RadioButton();
            this.rbDevolucionP = new System.Windows.Forms.RadioButton();
            this.rbConsumo = new System.Windows.Forms.RadioButton();
            this.rbAjustes = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnProveedores = new System.Windows.Forms.Button();
            this.rbTodo = new System.Windows.Forms.RadioButton();
            this.btnClientes = new System.Windows.Forms.Button();
            this.btnProductos = new System.Windows.Forms.Button();
            this.txtBusqueda = new System.Windows.Forms.TextBox();
            this.btnRegistrarEntrada = new System.Windows.Forms.Button();
            this.btnRegistrarSalida = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimientos)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMovimientos
            // 
            this.dgvMovimientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMovimientos.Location = new System.Drawing.Point(12, 111);
            this.dgvMovimientos.Name = "dgvMovimientos";
            this.dgvMovimientos.RowHeadersWidth = 51;
            this.dgvMovimientos.RowTemplate.Height = 24;
            this.dgvMovimientos.Size = new System.Drawing.Size(420, 229);
            this.dgvMovimientos.TabIndex = 0;
            // 
            // rbCompras
            // 
            this.rbCompras.AutoSize = true;
            this.rbCompras.Location = new System.Drawing.Point(461, 175);
            this.rbCompras.Name = "rbCompras";
            this.rbCompras.Size = new System.Drawing.Size(83, 20);
            this.rbCompras.TabIndex = 1;
            this.rbCompras.Text = "Compras";
            this.rbCompras.UseVisualStyleBackColor = true;
            // 
            // rbVentas
            // 
            this.rbVentas.AutoSize = true;
            this.rbVentas.Location = new System.Drawing.Point(461, 201);
            this.rbVentas.Name = "rbVentas";
            this.rbVentas.Size = new System.Drawing.Size(70, 20);
            this.rbVentas.TabIndex = 2;
            this.rbVentas.Text = "Ventas";
            this.rbVentas.UseVisualStyleBackColor = true;
            // 
            // rbDevolucionC
            // 
            this.rbDevolucionC.AutoSize = true;
            this.rbDevolucionC.Location = new System.Drawing.Point(461, 227);
            this.rbDevolucionC.Name = "rbDevolucionC";
            this.rbDevolucionC.Size = new System.Drawing.Size(138, 20);
            this.rbDevolucionC.TabIndex = 3;
            this.rbDevolucionC.Text = "Devolucion cliente";
            this.rbDevolucionC.UseVisualStyleBackColor = true;
            // 
            // rbDevolucionP
            // 
            this.rbDevolucionP.AutoSize = true;
            this.rbDevolucionP.Location = new System.Drawing.Point(461, 253);
            this.rbDevolucionP.Name = "rbDevolucionP";
            this.rbDevolucionP.Size = new System.Drawing.Size(162, 20);
            this.rbDevolucionP.TabIndex = 4;
            this.rbDevolucionP.Text = "Devolucion proveedor";
            this.rbDevolucionP.UseVisualStyleBackColor = true;
            // 
            // rbConsumo
            // 
            this.rbConsumo.AutoSize = true;
            this.rbConsumo.Location = new System.Drawing.Point(461, 279);
            this.rbConsumo.Name = "rbConsumo";
            this.rbConsumo.Size = new System.Drawing.Size(85, 20);
            this.rbConsumo.TabIndex = 5;
            this.rbConsumo.Text = "Consumo";
            this.rbConsumo.UseVisualStyleBackColor = true;
            // 
            // rbAjustes
            // 
            this.rbAjustes.AutoSize = true;
            this.rbAjustes.Location = new System.Drawing.Point(461, 305);
            this.rbAjustes.Name = "rbAjustes";
            this.rbAjustes.Size = new System.Drawing.Size(72, 20);
            this.rbAjustes.TabIndex = 6;
            this.rbAjustes.Text = "Ajustes";
            this.rbAjustes.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(450, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "Mostrar:";
            // 
            // btnProveedores
            // 
            this.btnProveedores.Location = new System.Drawing.Point(481, 82);
            this.btnProveedores.Name = "btnProveedores";
            this.btnProveedores.Size = new System.Drawing.Size(118, 29);
            this.btnProveedores.TabIndex = 8;
            this.btnProveedores.Text = "Proveedores";
            this.btnProveedores.UseVisualStyleBackColor = true;
            this.btnProveedores.Click += new System.EventHandler(this.btnProveedores_Click);
            // 
            // rbTodo
            // 
            this.rbTodo.AutoSize = true;
            this.rbTodo.Checked = true;
            this.rbTodo.Location = new System.Drawing.Point(461, 149);
            this.rbTodo.Name = "rbTodo";
            this.rbTodo.Size = new System.Drawing.Size(61, 20);
            this.rbTodo.TabIndex = 9;
            this.rbTodo.TabStop = true;
            this.rbTodo.Text = "Todo";
            this.rbTodo.UseVisualStyleBackColor = true;
            // 
            // btnClientes
            // 
            this.btnClientes.Location = new System.Drawing.Point(481, 47);
            this.btnClientes.Name = "btnClientes";
            this.btnClientes.Size = new System.Drawing.Size(118, 29);
            this.btnClientes.TabIndex = 10;
            this.btnClientes.Text = "Clientes";
            this.btnClientes.UseVisualStyleBackColor = true;
            this.btnClientes.Click += new System.EventHandler(this.btnClientes_Click);
            // 
            // btnProductos
            // 
            this.btnProductos.Location = new System.Drawing.Point(481, 12);
            this.btnProductos.Name = "btnProductos";
            this.btnProductos.Size = new System.Drawing.Size(118, 29);
            this.btnProductos.TabIndex = 11;
            this.btnProductos.Text = "Productos";
            this.btnProductos.UseVisualStyleBackColor = true;
            this.btnProductos.Click += new System.EventHandler(this.btnProductos_Click);
            // 
            // txtBusqueda
            // 
            this.txtBusqueda.Location = new System.Drawing.Point(12, 78);
            this.txtBusqueda.Name = "txtBusqueda";
            this.txtBusqueda.Size = new System.Drawing.Size(307, 22);
            this.txtBusqueda.TabIndex = 12;
            // 
            // btnRegistrarEntrada
            // 
            this.btnRegistrarEntrada.Location = new System.Drawing.Point(17, 12);
            this.btnRegistrarEntrada.Name = "btnRegistrarEntrada";
            this.btnRegistrarEntrada.Size = new System.Drawing.Size(148, 49);
            this.btnRegistrarEntrada.TabIndex = 13;
            this.btnRegistrarEntrada.Text = "Registrar entrada";
            this.btnRegistrarEntrada.UseVisualStyleBackColor = true;
            this.btnRegistrarEntrada.Click += new System.EventHandler(this.btnRegistrarEntrada_Click);
            // 
            // btnRegistrarSalida
            // 
            this.btnRegistrarSalida.Location = new System.Drawing.Point(171, 12);
            this.btnRegistrarSalida.Name = "btnRegistrarSalida";
            this.btnRegistrarSalida.Size = new System.Drawing.Size(148, 49);
            this.btnRegistrarSalida.TabIndex = 14;
            this.btnRegistrarSalida.Text = "Registrar salida";
            this.btnRegistrarSalida.UseVisualStyleBackColor = true;
            this.btnRegistrarSalida.Click += new System.EventHandler(this.btnRegistrarSalida_Click);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuscar.Location = new System.Drawing.Point(332, 78);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(100, 26);
            this.btnBuscar.TabIndex = 16;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            // 
            // PanelPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 352);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.btnRegistrarSalida);
            this.Controls.Add(this.btnRegistrarEntrada);
            this.Controls.Add(this.txtBusqueda);
            this.Controls.Add(this.btnProductos);
            this.Controls.Add(this.btnClientes);
            this.Controls.Add(this.rbTodo);
            this.Controls.Add(this.btnProveedores);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbAjustes);
            this.Controls.Add(this.rbConsumo);
            this.Controls.Add(this.rbDevolucionP);
            this.Controls.Add(this.rbDevolucionC);
            this.Controls.Add(this.rbVentas);
            this.Controls.Add(this.rbCompras);
            this.Controls.Add(this.dgvMovimientos);
            this.Name = "PanelPrincipal";
            this.Text = "Panel principal";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimientos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMovimientos;
        private System.Windows.Forms.RadioButton rbCompras;
        private System.Windows.Forms.RadioButton rbVentas;
        private System.Windows.Forms.RadioButton rbDevolucionC;
        private System.Windows.Forms.RadioButton rbDevolucionP;
        private System.Windows.Forms.RadioButton rbConsumo;
        private System.Windows.Forms.RadioButton rbAjustes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnProveedores;
        private System.Windows.Forms.RadioButton rbTodo;
        private System.Windows.Forms.Button btnClientes;
        private System.Windows.Forms.Button btnProductos;
        private System.Windows.Forms.TextBox txtBusqueda;
        private System.Windows.Forms.Button btnRegistrarEntrada;
        private System.Windows.Forms.Button btnRegistrarSalida;
        private System.Windows.Forms.Button btnBuscar;
    }
}