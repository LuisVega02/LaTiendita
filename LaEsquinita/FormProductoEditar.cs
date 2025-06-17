using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LaEsquinita
{
    public partial class FormProductoEditar : Form
    {
        private readonly int? productoId; // null para nuevo
        private string connectionString = "Data Source=.;Initial Catalog=InventarioLaEsquinita;Integrated Security=True";

        public FormProductoEditar(int? id = null)
        {
            InitializeComponent();
            productoId = id;
        }

        private void FormProductoEditar_Load(object sender, EventArgs e)
        {
            CargarCombos();

            if (productoId.HasValue)
                CargarProducto();
        }

        private void CargarCombos()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // CATEGORÍA
                SqlDataAdapter daCat = new SqlDataAdapter("SELECT id, nombre FROM Categoria", conn);
                DataTable dtCat = new DataTable();
                daCat.Fill(dtCat);
                cmbCategoria.DataSource = dtCat;
                cmbCategoria.DisplayMember = "nombre";
                cmbCategoria.ValueMember = "id";

                // PROVEEDOR
                SqlDataAdapter daProv = new SqlDataAdapter("SELECT id, nombre FROM Proveedor", conn);
                DataTable dtProv = new DataTable();
                daProv.Fill(dtProv);
                cmbProveedor.DataSource = dtProv;
                cmbProveedor.DisplayMember = "nombre";
                cmbProveedor.ValueMember = "id";

                // UNIDAD
                SqlDataAdapter daUnidad = new SqlDataAdapter("SELECT id, nombre FROM TipoUnidad", conn);
                DataTable dtUnidad = new DataTable();
                daUnidad.Fill(dtUnidad);
                cmbUnidad.DataSource = dtUnidad;
                cmbUnidad.DisplayMember = "nombre";
                cmbUnidad.ValueMember = "id";
            }
        }

        private void CargarProducto()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT nombre, precio, stock, stockMinimo, categoriaID, proveedorID, tipoUnidadID FROM Producto WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", productoId.Value);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtNombre.Text = reader["nombre"].ToString();
                        txtPrecio.Text = reader["precio"].ToString();
                        txtStock.Text = reader["stock"].ToString();
                        txtStockMinimo.Text = reader["stockMinimo"].ToString();

                        cmbCategoria.SelectedValue = reader["categoriaID"];
                        cmbProveedor.SelectedValue = reader["proveedorID"];
                        cmbUnidad.SelectedValue = reader["tipoUnidadID"];
                    }
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd;
                if (productoId.HasValue)
                {
                    // UPDATE
                    string query = @"UPDATE Producto SET nombre=@nombre, precio=@precio, stock=@stock, stockMinimo=@stockMinimo, 
                                     categoriaID=@categoriaID, proveedorID=@proveedorID, tipoUnidadID=@tipoUnidadID 
                                     WHERE id=@id";

                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", productoId.Value);
                }
                else
                {
                    // INSERT
                    string query = @"INSERT INTO Producto (nombre, precio, stock, stockMinimo, categoriaID, proveedorID, tipoUnidadID) 
                                     VALUES (@nombre, @precio, @stock, @stockMinimo, @categoriaID, @proveedorID, @tipoUnidadID)";
                    cmd = new SqlCommand(query, conn);
                }

                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                cmd.Parameters.AddWithValue("@precio", Convert.ToDecimal(txtPrecio.Text));
                cmd.Parameters.AddWithValue("@stock", Convert.ToInt32(txtStock.Text));
                cmd.Parameters.AddWithValue("@stockMinimo", Convert.ToInt32(txtStockMinimo.Text));
                cmd.Parameters.AddWithValue("@categoriaID", cmbCategoria.SelectedValue);
                cmd.Parameters.AddWithValue("@proveedorID", cmbProveedor.SelectedValue);
                cmd.Parameters.AddWithValue("@tipoUnidadID", cmbUnidad.SelectedValue);

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                !decimal.TryParse(txtPrecio.Text, out _) ||
                !int.TryParse(txtStock.Text, out _) ||
                !int.TryParse(txtStockMinimo.Text, out _))
            {
                MessageBox.Show("Verifica que todos los campos estén completos y sean válidos.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
