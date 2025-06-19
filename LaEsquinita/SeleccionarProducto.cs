using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaEsquinita
{
    public partial class SeleccionarProducto : Form
    {
        private string connectionString = "Data Source=.;Initial Catalog=InventarioLaEsquinita;Integrated Security=True";
        private DataTable productosOriginales;
        public int ProductoIDSeleccionado { get; private set; }
        public string NombreProductoSeleccionado { get; private set; }
        public SeleccionarProducto()
        {
            InitializeComponent();
            CargarComboboxes();
            CargarProductos();

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

        private void dgvProductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];
                ProductoIDSeleccionado = Convert.ToInt32(fila.Cells["id"].Value);
                NombreProductoSeleccionado = fila.Cells["nombre"].Value.ToString();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void SeleccionarProducto_Load(object sender, EventArgs e)
        {
            dgvProductos.ReadOnly = true;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;

            dgvProductos.CellDoubleClick += dgvProductos_CellDoubleClick;
        }
    }
}
