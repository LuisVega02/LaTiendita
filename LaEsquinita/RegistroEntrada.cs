using LaEsquinita;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace LaEsquinata
{
    public partial class RegistroEntrada : Form
    {
        private string connectionString = "Data Source=.;Initial Catalog=InventarioLaEsquinita;Integrated Security=True";
        private int clienteIdSeleccionado = -1;
        private int movimientoDetalleIdSeleccionado = -1;
        private int productoIdSeleccionado = -1;
        private int cantidadOriginal = 0;

        public RegistroEntrada()
        {
            InitializeComponent();
            ConfigurarDataGridViews();
        }

        private void ConfigurarDataGridViews()
        {
            // Configurar dgvClientes
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.MultiSelect = false;
            dgvClientes.ReadOnly = true;
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvClientes.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Configurar dgvProductos (compras del cliente)
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.MultiSelect = false;
            dgvProductos.ReadOnly = true;
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvProductos.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        private void Buscar_Click(object sender, EventArgs e)
        {
            BuscarClientes();
        }

        private void BuscarClientes()
        {
            string textoBusqueda = txtCliente.Text.Trim();

            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    string query = @"SELECT id, nombre, telefono, email, direccion FROM Cliente 
                                    WHERE nombre LIKE @busqueda OR telefono LIKE @busqueda";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
                    adapter.SelectCommand.Parameters.AddWithValue("@busqueda", "%" + textoBusqueda + "%");

                    DataTable tablaResultados = new DataTable();
                    adapter.Fill(tablaResultados);

                    if (tablaResultados.Rows.Count > 0)
                    {
                        dgvClientes.DataSource = tablaResultados;
                        ConfigurarColumnasClientes();
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron clientes.", "Información",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvClientes.DataSource = null;
                        dgvProductos.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar clientes: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnasClientes()
        {
            if (dgvClientes.Columns.Count > 0)
            {
                dgvClientes.Columns["id"].Visible = false;
                dgvClientes.Columns["nombre"].HeaderText = "Nombre del Cliente";
                dgvClientes.Columns["telefono"].HeaderText = "Teléfono";
                dgvClientes.Columns["email"].HeaderText = "Email";
                dgvClientes.Columns["direccion"].HeaderText = "Dirección";
            }
        }

        private void dgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count > 0 && !dgvClientes.SelectedRows[0].IsNewRow)
            {
                DataGridViewRow row = dgvClientes.SelectedRows[0];
                clienteIdSeleccionado = Convert.ToInt32(row.Cells["id"].Value);
                txtCliente.Text = row.Cells["nombre"].Value?.ToString() ?? "";
                CargarComprasDelCliente(clienteIdSeleccionado);
            }
        }

        private void CargarComprasDelCliente(int clienteId)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    string query = @"SELECT 
                                    de.id AS DetalleEntradaID,
                                    p.nombre AS Producto,
                                    de.cantidad AS Cantidad,
                                    de.valorUnitario AS PrecioUnitario,
                                    (de.cantidad * de.valorUnitario) AS Total,
                                    md.id AS MovimientoDetalleID,
                                    de.productoID AS ProductoID
                                    FROM MovimientoDetalle md
                                    INNER JOIN Entrada e ON md.id = e.movimientoID
                                    INNER JOIN DetalleEntrada de ON e.id = de.entradaID
                                    INNER JOIN Producto p ON de.productoID = p.id
                                    WHERE md.clienteID = @clienteId";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
                    adapter.SelectCommand.Parameters.AddWithValue("@clienteId", clienteId);

                    DataTable tablaResultados = new DataTable();
                    adapter.Fill(tablaResultados);

                    if (tablaResultados.Rows.Count > 0)
                    {
                        dgvProductos.DataSource = tablaResultados;
                        ConfigurarColumnasProductos();
                    }
                    else
                    {
                        MessageBox.Show("El cliente no tiene compras registradas.", "Información",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvProductos.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar compras del cliente: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnasProductos()
        {
            if (dgvProductos.Columns.Count > 0)
            {
                dgvProductos.Columns["DetalleEntradaID"].Visible = false;
                dgvProductos.Columns["MovimientoDetalleID"].Visible = false;
                dgvProductos.Columns["ProductoID"].Visible = false;
                dgvProductos.Columns["Producto"].HeaderText = "Producto";
                dgvProductos.Columns["Cantidad"].HeaderText = "Cantidad";
                dgvProductos.Columns["PrecioUnitario"].HeaderText = "Precio Unitario";
                dgvProductos.Columns["Total"].HeaderText = "Total";
            }
        }

        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0 && !dgvProductos.SelectedRows[0].IsNewRow)
            {
                DataGridViewRow row = dgvProductos.SelectedRows[0];
                movimientoDetalleIdSeleccionado = Convert.ToInt32(row.Cells["MovimientoDetalleID"].Value);
                productoIdSeleccionado = Convert.ToInt32(row.Cells["ProductoID"].Value);
                cantidadOriginal = Convert.ToInt32(row.Cells["Cantidad"].Value);

                txtProducto.Text = row.Cells["Producto"].Value.ToString();
                txtCantidad.Text = cantidadOriginal.ToString();
            }
        }

        private void btnLimpiarCliente_Click(object sender, EventArgs e)
        {
            clienteIdSeleccionado = -1;
            movimientoDetalleIdSeleccionado = -1;
            productoIdSeleccionado = -1;
            cantidadOriginal = 0;

            txtCliente.Text = "";
            txtProducto.Text = "";
            txtCantidad.Text = "";

            dgvClientes.DataSource = null;
            dgvProductos.DataSource = null;
        }

        private void btnRegistrarDevC_Click(object sender, EventArgs e)
        {
            if (!ValidarDatosDevolucion())
                return;

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                SqlTransaction transaction = conexion.BeginTransaction();

                try
                {
                    int cantidadDevolucion = Convert.ToInt32(txtCantidad.Text);

                    // 1. Registrar el movimiento de devolución
                    int nuevoMovimientoId = RegistrarMovimientoDevolucion(conexion, transaction);

                    // 2. Registrar la entrada de devolución
                    int entradaId = RegistrarEntradaDevolucion(conexion, transaction, nuevoMovimientoId);

                    // 3. Registrar el detalle de la devolución
                    RegistrarDetalleDevolucion(conexion, transaction, entradaId);

                    // 4. Actualizar el stock del producto
                    ActualizarStockProducto(conexion, transaction, cantidadDevolucion);

                    transaction.Commit();
                    MessageBox.Show("Devolución registrada exitosamente!", "Éxito",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Actualizar la lista de compras del cliente
                    CargarComprasDelCliente(clienteIdSeleccionado);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Error al registrar la devolución: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidarDatosDevolucion()
        {
            if (clienteIdSeleccionado <= 0)
            {
                MessageBox.Show("Debe seleccionar un cliente válido", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (movimientoDetalleIdSeleccionado <= 0 || productoIdSeleccionado <= 0)
            {
                MessageBox.Show("Debe seleccionar un producto de la lista de compras", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser un número positivo", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cantidad > cantidadOriginal)
            {
                MessageBox.Show("La cantidad a devolver no puede ser mayor que la cantidad original", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private int RegistrarMovimientoDevolucion(SqlConnection conexion, SqlTransaction transaction)
        {
            const int tipoMovimientoDevolucion = 3; // ID para "Devolución" en TipoMovimiento

            string query = @"INSERT INTO MovimientoDetalle (proveedorID, clienteID, TipoMovimientoID) 
                         VALUES (@proveedorID, @clienteID, @TipoMovimientoID);
                         SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, conexion, transaction))
            {
                cmd.Parameters.AddWithValue("@proveedorID", DBNull.Value);
                cmd.Parameters.AddWithValue("@clienteID", clienteIdSeleccionado);
                cmd.Parameters.AddWithValue("@TipoMovimientoID", tipoMovimientoDevolucion);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private int RegistrarEntradaDevolucion(SqlConnection conexion, SqlTransaction transaction, int movimientoId)
        {
            string query = @"INSERT INTO Entrada (fecha, detalle, movimientoID, total) 
                         VALUES (@fecha, @detalle, @movimientoID, @total);
                         SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, conexion, transaction))
            {
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                cmd.Parameters.AddWithValue("@detalle", "Devolución de cliente por producto: " + txtProducto.Text);
                cmd.Parameters.AddWithValue("@movimientoID", movimientoId);
                cmd.Parameters.AddWithValue("@total", 0); // Las devoluciones no generan ingresos

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void RegistrarDetalleDevolucion(SqlConnection conexion, SqlTransaction transaction, int entradaId)
        {
            // Obtener el precio unitario original del producto
            decimal precioUnitario = ObtenerPrecioProducto(conexion, transaction);

            string query = @"INSERT INTO DetalleEntrada 
                         (productoID, cantidad, valorUnitario, entradaID) 
                         VALUES (@productoID, @cantidad, @valorUnitario, @entradaID)";

            using (SqlCommand cmd = new SqlCommand(query, conexion, transaction))
            {
                cmd.Parameters.AddWithValue("@productoID", productoIdSeleccionado);
                cmd.Parameters.AddWithValue("@cantidad", Convert.ToInt32(txtCantidad.Text));
                cmd.Parameters.AddWithValue("@valorUnitario", precioUnitario);
                cmd.Parameters.AddWithValue("@entradaID", entradaId);

                cmd.ExecuteNonQuery();
            }
        }

        private decimal ObtenerPrecioProducto(SqlConnection conexion, SqlTransaction transaction)
        {
            string query = "SELECT precio FROM Producto WHERE id = @productoId";
            using (SqlCommand cmd = new SqlCommand(query, conexion, transaction))
            {
                cmd.Parameters.AddWithValue("@productoId", productoIdSeleccionado);
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }

        private void ActualizarStockProducto(SqlConnection conexion, SqlTransaction transaction, int cantidadDevolucion)
        {
            string query = "UPDATE Producto SET stock = stock + @cantidad WHERE id = @productoId";
            using (SqlCommand cmd = new SqlCommand(query, conexion, transaction))
            {
                cmd.Parameters.AddWithValue("@cantidad", cantidadDevolucion);
                cmd.Parameters.AddWithValue("@productoId", productoIdSeleccionado);
                cmd.ExecuteNonQuery();
            }
        }

        private void btnRegistrarCompra_Click(object sender, EventArgs e)
        {
            RegistrarComprar registrarComprar = new RegistrarComprar();
            registrarComprar.Show();
        }
    }
}