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
    public partial class Proveedores : Form
    {
        public Proveedores()
        {
            InitializeComponent();
            dgvProveedores.SelectionChanged += dgvClientes_SelectionChanged;
        }

        private void dgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProveedores.CurrentRow != null && dgvProveedores.CurrentRow.Index >= 0)
            {
                var valorId = dgvProveedores.CurrentRow.Cells["id"].Value;
                txtIDP.Text = valorId != null ? valorId.ToString() : "";
                var valorNombre = dgvProveedores.CurrentRow.Cells["nombre"].Value;
                txtNombreP.Text = valorNombre != null ? valorNombre.ToString() : "";
                var valorTelefono = dgvProveedores.CurrentRow.Cells["telefono"].Value;
                txtTelefonoP.Text = valorTelefono != null ? valorTelefono.ToString() : "";
                var valorEmail = dgvProveedores.CurrentRow.Cells["email"].Value;
                txtEmailP.Text = valorEmail != null ? valorEmail.ToString() : "";
                var valorDireccion = dgvProveedores.CurrentRow.Cells["direccion"].Value;
                txtDireccionP.Text = valorDireccion != null ? valorDireccion.ToString() : "";
                txtDireccionP.Enabled = false;
                txtEmailP.Enabled = false;
                txtNombreP.Enabled = false;
                txtTelefonoP.Enabled = false;
                btnEliminarP.Enabled = true;
                btnRegistrarP.Enabled = false;
                btnGuardarP.Enabled = false;
            }
        }
        private void btnEditarP_Click(object sender, EventArgs e)
        {
            txtDireccionP.Enabled = true;
            txtEmailP.Enabled = true;
            txtNombreP.Enabled = true;
            txtTelefonoP.Enabled = true;
            btnGuardarP.Enabled = true;
            btnEliminarP.Enabled = false;
            btnRegistrarP.Enabled = false;
        }

        private void btnGuardarP_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=.;Initial Catalog=InventarioLaEsquinita;Integrated Security=True";

            string query = "UPDATE Proveedor SET nombre = @nombre, telefono = @telefono, email = @email, direccion = @direccion WHERE id = @id";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombre", txtNombreP.Text.Trim());
                    cmd.Parameters.AddWithValue("@telefono", txtTelefonoP.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmailP.Text.Trim());
                    cmd.Parameters.AddWithValue("@direccion", txtDireccionP.Text.Trim());
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtIDP.Text.Trim()));

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Proveedor actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            txtDireccionP.Enabled = false;
            txtEmailP.Enabled = false;
            txtNombreP.Enabled = false;
            txtTelefonoP.Enabled = false;
            btnEliminarP.Enabled = true;
            btnGuardarP.Enabled = false;

            CargarProveedores();
        }

        private void btnNuevoP_Click(object sender, EventArgs e)
        {
            txtDireccionP.Enabled = true;
            txtEmailP.Enabled = true;
            txtNombreP.Enabled = true;
            txtTelefonoP.Enabled = true;
            btnGuardarP.Enabled = false;
            btnEliminarP.Enabled = false;
            txtIDP.Text = "";
            txtDireccionP.Text = "";
            txtEmailP.Text = "";
            txtNombreP.Text = "";
            txtTelefonoP.Text = "";
            btnRegistrarP.Enabled = true;
        }

        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            txtDireccionP.Enabled = false;
            txtEmailP.Enabled = false;
            txtNombreP.Enabled = false;
            txtTelefonoP.Enabled = false;
            btnEliminarP.Enabled = true;
            btnRegistrarP.Enabled = false;
            btnGuardarP.Enabled = false;
        }

        private void Proveedores_Load(object sender, EventArgs e)
        {
            CargarProveedores();
        }
        

        private void btnRegistrarP_Click(object sender, EventArgs e)
        {
            string campoFaltante = "";
            foreach (var control in new[] { txtNombreP, txtTelefonoP, txtEmailP, txtDireccionP })
            {
                switch (control.Name)
                {
                    case "txtNombreP":
                        if (string.IsNullOrWhiteSpace(control.Text))
                            campoFaltante = "Nombre";
                        break;
                    case "txtTelefonoP":
                        if (string.IsNullOrWhiteSpace(control.Text))
                            campoFaltante = "Teléfono";
                        break;
                    case "txtEmailP":
                        if (string.IsNullOrWhiteSpace(control.Text))
                            campoFaltante = "Email";
                        break;
                    case "txtDireccionP":
                        if (string.IsNullOrWhiteSpace(control.Text))
                            campoFaltante = "Dirección";
                        break;
                }
                if (!string.IsNullOrEmpty(campoFaltante))
                {
                    MessageBox.Show($"El campo {campoFaltante} es obligatorio.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            string connectionString = "Data Source=.;Initial Catalog=InventarioLaEsquinita;Integrated Security=True";

            string query = "INSERT INTO Proveedor (nombre, telefono, email, direccion) VALUES (@nombre, @telefono, @email, @direccion)";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombre", txtNombreP.Text.Trim());
                    cmd.Parameters.AddWithValue("@telefono", txtTelefonoP.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmailP.Text.Trim());
                    cmd.Parameters.AddWithValue("@direccion", txtDireccionP.Text.Trim());

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Proveedor registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            txtDireccionP.Enabled = false;
            txtEmailP.Enabled = false;
            txtNombreP.Enabled = false;
            txtTelefonoP.Enabled = false;
            btnEliminarP.Enabled = true;
            btnRegistrarP.Enabled = false;

            CargarProveedores();
        }

        private void CargarProveedores()
        {
            string connectionString = "Data Source=.;Initial Catalog=InventarioLaEsquinita;Integrated Security=True";
            string query = "SELECT * FROM Proveedor";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
                DataTable tablaClientes = new DataTable();
                adapter.Fill(tablaClientes);
                dgvProveedores.DataSource = tablaClientes;
                dgvProveedores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }

        private void btnBuscarP_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=.;Initial Catalog=InventarioLaEsquinita;Integrated Security=True";
            string textoBusqueda = txtProveedores.Text.Trim();

            string query = "SELECT * FROM Proveedor WHERE nombre LIKE @busqueda OR telefono LIKE @busqueda OR email LIKE @busqueda OR direccion LIKE @busqueda";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
                adapter.SelectCommand.Parameters.AddWithValue("@busqueda", "%" + textoBusqueda + "%");
                DataTable tablaClientes = new DataTable();
                adapter.Fill(tablaClientes);
                dgvProveedores.DataSource = tablaClientes;
                dgvProveedores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }

        private void btnEliminarP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIDP.Text))
            {
                MessageBox.Show("Selecciona un proveedor para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show(
                "¿Estás seguro de que deseas eliminar este proveedor?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                string connectionString = "Data Source=.;Initial Catalog=InventarioLaEsquinita;Integrated Security=True";
                string query = "DELETE FROM Proveedor WHERE id = @id";

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtIDP.Text.Trim()));
                        conexion.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Proveedor eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtIDP.Text = "";
                txtNombreP.Text = "";
                txtTelefonoP.Text = "";
                txtEmailP.Text = "";
                txtDireccionP.Text = "";

                txtDireccionP.Enabled = false;
                txtEmailP.Enabled = false;
                txtNombreP.Enabled = false;
                txtTelefonoP.Enabled = false;
                btnEliminarP.Enabled = false;
                btnRegistrarP.Enabled = false;
                btnGuardarP.Enabled = false;

                CargarProveedores();
            }
        }
    }
}
