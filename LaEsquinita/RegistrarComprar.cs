using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LaEsquinita
{
    public partial class RegistrarComprar : Form
    {
        private string connectionString = "Data Source=.;Initial Catalog=InventarioLaEsquinita;Integrated Security=True";
        private int proveedorIdSeleccionado = -1;
        private DataTable productosAgregados;

        public RegistrarComprar()
        {
            InitializeComponent();
            CargarComboboxes();
            InicializarTablaProductos();
        }

        private DataRow BuscarProductoPorNombre(string nombre)
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 1 id, nombre, precio FROM Producto WHERE nombre LIKE @nombre";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@nombre", "%" + nombre + "%");

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable result = new DataTable();
                adapter.Fill(result);

                if (result.Rows.Count > 0)
                    return result.Rows[0];
                else
                    return null;
            }
        }

        private void InicializarTablaProductos()
        {
            productosAgregados = new DataTable();
            productosAgregados.Columns.Add("productoID", typeof(int));
            productosAgregados.Columns.Add("nombre", typeof(string));
            productosAgregados.Columns.Add("cantidad", typeof(int));
            productosAgregados.Columns.Add("valorUnitario", typeof(decimal));
            productosAgregados.Columns.Add("subtotal", typeof(decimal));

            dgvProdVent.DataSource = productosAgregados;
        }

        private void CargarComboboxes()
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    // Cargar categorías
                    string queryCategorias = "SELECT id, nombre FROM Categoria";
                    SqlDataAdapter daCategorias = new SqlDataAdapter(queryCategorias, conexion);
                    DataTable dtCategorias = new DataTable();
                    daCategorias.Fill(dtCategorias);

                    // Cargar tipos de unidad
                    string queryUnidades = "SELECT id, nombre FROM TipoUnidad";
                    SqlDataAdapter daUnidades = new SqlDataAdapter(queryUnidades, conexion);
                    DataTable dtUnidades = new DataTable();
                    daUnidades.Fill(dtUnidades);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProveedor_Click(object sender, EventArgs e)
        {
            Proveedores proveedores = new Proveedores();
            proveedores.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BuscarProveedores();
        }

        private void BuscarProveedores()
        {
            string textoBusqueda = txtProveedor.Text.Trim();

            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    string query = @"SELECT id, nombre, telefono, email FROM Proveedor 
                                    WHERE nombre LIKE @busqueda OR telefono LIKE @busqueda";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
                    adapter.SelectCommand.Parameters.AddWithValue("@busqueda", "%" + textoBusqueda + "%");

                    DataTable tablaResultados = new DataTable();
                    adapter.Fill(tablaResultados);

                    if (tablaResultados.Rows.Count > 0)
                    {
                        dgvProveedores.DataSource = tablaResultados;
                        dgvProveedores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron proveedores.", "Información",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar proveedores: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProveedores_SelectionChanged(object sender, EventArgs e)
        {
            // Verificar que haya una fila válida seleccionada
            if (dgvProveedores.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvProveedores.SelectedRows[0];

                // Verificar que no sea una fila nueva (si AllowUserToAddRows es true)
                if (!row.IsNewRow)
                {
                    try
                    {
                        proveedorIdSeleccionado = Convert.ToInt32(row.Cells["id"].Value);
                        txtProveedor.Text = row.Cells["nombre"].Value?.ToString() ?? "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al obtener datos del proveedor: {ex.Message}",
                                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnLimpiarProveedor_Click(object sender, EventArgs e)
        {
            // Limpiar la selección del proveedor
            proveedorIdSeleccionado = -1;
            txtProveedor.Text = "";
            txtProveedor.Enabled = true;
            txtProveedor.BackColor = SystemColors.Window;

            // Opcional: Deseleccionar fila en el DataGridView
            if (dgvProveedores.CurrentRow != null)
            {
                dgvProveedores.CurrentRow.Selected = false;
            }
        }
        private int RegistrarEntrada(SqlConnection conexion, SqlTransaction transaction)
        {
            // 1. Primero registrar en MovimientoDetalle
            int movimientoDetalleId = RegistrarMovimientoDetalle(conexion, transaction);

            // 2. Luego registrar en Entrada
            string query = @"INSERT INTO Entrada (fecha, detalle, movimientoID, total) 
                     VALUES (@fecha, @detalle, @movimientoID, @total);
                     SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, conexion, transaction))
            {
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                cmd.Parameters.AddWithValue("@detalle", txtDetalle.Text.Trim());
                cmd.Parameters.AddWithValue("@movimientoID", movimientoDetalleId);
                cmd.Parameters.AddWithValue("@total", Convert.ToDecimal(txtTotal.Text));

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private int RegistrarMovimientoDetalle(SqlConnection conexion, SqlTransaction transaction)
        {
            const int tipoMovimientoCompra = 1; // ID para "Compra" en TipoMovimiento

            string query = @"INSERT INTO MovimientoDetalle (proveedorID, clienteID, TipoMovimientoID) 
                     VALUES (@proveedorID, @clienteID, @TipoMovimientoID);
                     SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, conexion, transaction))
            {
                cmd.Parameters.AddWithValue("@proveedorID", proveedorIdSeleccionado);
                cmd.Parameters.AddWithValue("@clienteID", DBNull.Value); // No aplica para compras
                cmd.Parameters.AddWithValue("@TipoMovimientoID", tipoMovimientoCompra);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        private void btnRegistrarCompra_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
                return;

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                SqlTransaction transaction = conexion.BeginTransaction();

                try
                {
                    // 1. Registrar entrada y movimiento
                    int entradaId = RegistrarEntrada(conexion, transaction);

                    // 2. Registrar detalles de la entrada y actualizar stock
                    foreach (DataRow row in productosAgregados.Rows)
                    {
                        int productoId = (int)row["productoID"];
                        int cantidad = (int)row["cantidad"];
                        decimal valorUnitario = (decimal)row["valorUnitario"];

                        // Registrar detalle
                        RegistrarDetalleEntrada(conexion, transaction, entradaId, productoId, cantidad, valorUnitario);

                        // Actualizar stock
                        string queryStock = "UPDATE Producto SET stock = stock + @cantidad WHERE id = @id";
                        using (SqlCommand cmd = new SqlCommand(queryStock, conexion, transaction))
                        {
                            cmd.Parameters.AddWithValue("@cantidad", cantidad);
                            cmd.Parameters.AddWithValue("@id", productoId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // 3. Confirmar cambios
                    transaction.Commit();

                    MessageBox.Show("Compra registrada exitosamente.", "Éxito",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 4. Limpiar formulario y tabla de productos
                    LimpiarFormulario();
                    productosAgregados.Clear();
                    dgvProdVent.DataSource = null;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Error al registrar la compra: {ex.Message}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RegistrarDetalleEntrada(SqlConnection conexion, SqlTransaction transaction, int entradaId, int productoId, int cantidad, decimal valorUnitario)
        {
            string query = @"INSERT INTO DetalleEntrada 
                     (productoID, cantidad, valorUnitario, entradaID) 
                     VALUES (@productoID, @cantidad, @valorUnitario, @entradaID)";

            using (SqlCommand cmd = new SqlCommand(query, conexion, transaction))
            {
                cmd.Parameters.AddWithValue("@productoID", productoId);
                cmd.Parameters.AddWithValue("@cantidad", cantidad);
                cmd.Parameters.AddWithValue("@valorUnitario", valorUnitario);
                cmd.Parameters.AddWithValue("@entradaID", entradaId);

                cmd.ExecuteNonQuery();
            }
        }

        private bool ValidarDatos()
        {
            // Validar proveedor seleccionado
            if (proveedorIdSeleccionado <= 0)
            {
                MessageBox.Show("Debe seleccionar un proveedor válido", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validar campos del producto
            if (string.IsNullOrWhiteSpace(txtProducto.Text))
            {
                MessageBox.Show("El nombre del producto es requerido", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }


            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser un número positivo", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validar cálculos
            decimal subtotalCalculado = cantidad;
            decimal igv = subtotalCalculado * 0.18m;
            decimal totalCalculado = subtotalCalculado + igv;

            return true;
        }

        private void LimpiarFormulario()
        {
            txtProducto.Clear();
            txtCantidad.Clear();
            txtSubtotal.Clear();
            txtTotal.Clear();
            txtDetalle.Clear();
            txtProveedor.Clear();
            proveedorIdSeleccionado = -1;
            dgvProveedores.DataSource = null;
        }

        private void CalcularSubtotal()
        {
            if (int.TryParse(txtCantidad.Text, out int cantidad))
            {
                // Calcular subtotal (valor unitario × cantidad)
                decimal subtotal = 1 * cantidad;
                txtSubtotal.Text = subtotal.ToString("N2");

                // Calcular total (subtotal + 18% IGV)
                decimal igv = subtotal * 0.18m;
                decimal total = subtotal + igv;
                txtTotal.Text = total.ToString("N2");
            }
            else
            {
                txtSubtotal.Text = "0.00";
                txtTotal.Text = "0.00";
            }
        }
        private void txtValorUnitario_TextChanged(object sender, EventArgs e)
        {
            CalcularSubtotal();
        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            CalcularSubtotal();
        }

        private void RegistrarComprar_Load(object sender, EventArgs e)
        {
            dgvProveedores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProveedores.MultiSelect = false;
            dgvProveedores.ReadOnly = true;
            dgvProveedores.AllowUserToAddRows = false;
            dgvProveedores.AllowUserToDeleteRows = false;
            dgvProveedores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Establecer colores de selección
            dgvProveedores.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvProveedores.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Asegurar que el evento SelectionChanged esté suscrito
            dgvProveedores.SelectionChanged += dgvProveedores_SelectionChanged;

            if (dgvProveedores.Columns.Count > 0)
            {
                dgvProveedores.Columns["id"].Visible = false;
                dgvProveedores.Columns["nombre"].HeaderText = "Nombre del Proveedor";
                dgvProveedores.Columns["telefono"].HeaderText = "Teléfono";
                dgvProveedores.Columns["email"].HeaderText = "Email";
            }
        }
        private void dgvProveedores_SelectionChanged_1(object sender, EventArgs e)
        {

        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida.");
                return;
            }

            var producto = BuscarProductoPorNombre(txtProducto.Text.Trim());
            if (producto == null)
            {
                MessageBox.Show("Producto no encontrado.");
                return;
            }

            int productoId = Convert.ToInt32(producto["id"]);
            string nombre = producto["nombre"].ToString();
            decimal precio = Convert.ToDecimal(producto["precio"]);
            decimal subtotal = precio * cantidad;

            // Verificar si el producto ya fue agregado
            foreach (DataRow row in productosAgregados.Rows)
            {
                if (row["productoID"] != DBNull.Value && Convert.ToInt32(row["productoID"]) == productoId)
                {
                    int nuevaCantidad = Convert.ToInt32(row["cantidad"]) + cantidad;
                    row["cantidad"] = nuevaCantidad;
                    row["subtotal"] = nuevaCantidad * Convert.ToDecimal(row["valorUnitario"]);
                    productosAgregados.AcceptChanges();
                    CalcularTotalesCompra();
                    return;
                }
            }

            // ✅ Si no estaba, lo agregamos aquí:
            productosAgregados.Rows.Add(productoId, nombre, cantidad, precio, subtotal);
            CalcularTotalesCompra();
        }

        private void CalcularTotalesCompra()
        {
            decimal total = 0;

            foreach (DataRow row in productosAgregados.Rows)
                total += (decimal)row["subtotal"];

            decimal igv = total * 0.18m;
            txtSubtotal.Text = total.ToString("N2");
            txtTotal.Text = (total + igv).ToString("N2");
        }

        private void btnGestion_Click(object sender, EventArgs e)
        {
            Productos formularioProductos = new Productos(); // <- formulario que implementa CRUD
            formularioProductos.ShowDialog();
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            using (SeleccionarProducto selector = new SeleccionarProducto())
            {
                if (selector.ShowDialog() == DialogResult.OK)
                {
                    // Usamos el nombre directamente en txtProducto
                    txtProducto.Text = selector.NombreProductoSeleccionado;
                    txtProducto.Tag = selector.ProductoIDSeleccionado; // Guarda el ID por si se requiere
                }
            }
        }

    }
}
