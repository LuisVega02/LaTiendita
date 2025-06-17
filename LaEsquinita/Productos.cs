using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LaEsquinita
{
    public partial class Productos : Form
    {
        private string connectionString = "Data Source=.;Initial Catalog=InventarioLaEsquinita;Integrated Security=True";
        private DataTable productosOriginales;

        public Productos()
        {
            InitializeComponent();
        }

        private void Productos_Load(object sender, EventArgs e)
        {
            CargarComboboxes();
            CargarProductos();

            if (!dgvProductos.Columns.Contains("btnEditar"))
            {
                DataGridViewButtonColumn btnEditar = new DataGridViewButtonColumn();
                btnEditar.HeaderText = "Editar";
                btnEditar.Text = "Editar";
                btnEditar.UseColumnTextForButtonValue = true;
                btnEditar.Name = "btnEditar";
                dgvProductos.Columns.Add(btnEditar);
            }

            if (!dgvProductos.Columns.Contains("btnEliminar"))
            {
                DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();
                btnEliminar.HeaderText = "Eliminar";
                btnEliminar.Text = "Eliminar";
                btnEliminar.UseColumnTextForButtonValue = true;
                btnEliminar.Name = "btnEliminar";
                dgvProductos.Columns.Add(btnEliminar);
            }

            dgvProductos.CellClick += dgvProductos_CellClick;
        }


        private void CargarComboboxes()
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();

                // CATEGORÍAS
                SqlDataAdapter daCategorias = new SqlDataAdapter("SELECT id, nombre FROM Categoria", conexion);
                DataTable dtCategorias = new DataTable();
                daCategorias.Fill(dtCategorias);

                cmbCategorias.DataSource = dtCategorias;
                cmbCategorias.DisplayMember = "nombre";
                cmbCategorias.ValueMember = "id";
                cmbCategorias.SelectedIndex = -1;

                // PROVEEDORES
                SqlDataAdapter daProveedores = new SqlDataAdapter("SELECT id, nombre FROM Proveedor", conexion);
                DataTable dtProveedores = new DataTable();
                daProveedores.Fill(dtProveedores);

                cmbProveedor.DataSource = dtProveedores;
                cmbProveedor.DisplayMember = "nombre";
                cmbProveedor.ValueMember = "id";
                cmbProveedor.SelectedIndex = -1;
            }

            cmbCategorias.SelectedIndexChanged += Filtros_Changed;
            cmbProveedor.SelectedIndexChanged += Filtros_Changed;
        }

        private void CargarProductos()
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                string query = @"SELECT p.id, p.nombre, p.precio, p.stock, c.nombre AS categoria, pr.nombre AS proveedor 
                                 FROM Producto p
                                 INNER JOIN Categoria c ON p.categoriaID = c.id
                                 INNER JOIN Proveedor pr ON p.proveedorID = pr.id";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
                productosOriginales = new DataTable();
                adapter.Fill(productosOriginales);

                dgvProductos.DataSource = productosOriginales;
            }
        }

        private void btnBuscarProd_Click(object sender, EventArgs e)
        {
            FiltrarProductos();
        }

        private void Filtros_Changed(object sender, EventArgs e)
        {
            FiltrarProductos();
        }

        private void FiltrarProductos()
        {
            if (productosOriginales == null) return;

            string filtro = "1=1";

            if (cmbCategorias.SelectedIndex != -1)
                filtro += $" AND categoria = '{cmbCategorias.Text.Replace("'", "''")}'";

            if (cmbProveedor.SelectedIndex != -1)
                filtro += $" AND proveedor = '{cmbProveedor.Text.Replace("'", "''")}'";

            if (!string.IsNullOrWhiteSpace(txtProductos.Text))
                filtro += $" AND nombre LIKE '%{txtProductos.Text.Replace("'", "''")}%'";

            DataView vista = productosOriginales.DefaultView;
            vista.RowFilter = filtro;

            dgvProductos.DataSource = vista;
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvProductos.Columns[e.ColumnIndex].Name == "btnEditar")
            {
                int productoId = Convert.ToInt32(dgvProductos.Rows[e.RowIndex].Cells["id"].Value);
                FormProductoEditar frmEditar = new FormProductoEditar(productoId); // tu formulario de edición
                if (frmEditar.ShowDialog() == DialogResult.OK)
                {
                    CargarProductos();
                }
            }
            else if (dgvProductos.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int productoId = Convert.ToInt32(dgvProductos.Rows[e.RowIndex].Cells["id"].Value);
                string nombre = dgvProductos.Rows[e.RowIndex].Cells["nombre"].Value.ToString();

                var confirmar = MessageBox.Show($"¿Estás seguro de que deseas eliminar el producto '{nombre}'?",
                                                "Confirmar eliminación",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Warning);

                if (confirmar == DialogResult.Yes)
                {
                    EliminarProducto(productoId);
                    CargarProductos();
                }
            }
        }

        private void EliminarProducto(int idProducto)
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Producto WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@id", idProducto);

                try
                {
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar el producto: {ex.Message}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            FormProductoEditar frmNuevo = new FormProductoEditar(); // sin ID para nuevo
            if (frmNuevo.ShowDialog() == DialogResult.OK)
            {
                CargarProductos();
            }
        }


    }
}
