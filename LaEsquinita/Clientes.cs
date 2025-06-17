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
    public partial class Clientes : Form
    {
        public Clientes()
        {
            InitializeComponent();
            dgvClientes.SelectionChanged += dgvClientes_SelectionChanged;
        }

        private void dgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClientes.CurrentRow != null && dgvClientes.CurrentRow.Index >= 0)
            {
                var valorId = dgvClientes.CurrentRow.Cells["id"].Value;
                txtID.Text = valorId != null ? valorId.ToString() : "";
                var valorNombre = dgvClientes.CurrentRow.Cells["nombre"].Value;
                txtNombreC.Text = valorNombre != null ? valorNombre.ToString() : "";
                var valorTelefono = dgvClientes.CurrentRow.Cells["telefono"].Value;
                txtTelefonoC.Text = valorTelefono != null ? valorTelefono.ToString() : "";
                var valorEmail = dgvClientes.CurrentRow.Cells["email"].Value;
                txtEmailC.Text = valorEmail != null ? valorEmail.ToString() : "";
                var valorDireccion = dgvClientes.CurrentRow.Cells["direccion"].Value;
                txtDireccionC.Text = valorDireccion != null ? valorDireccion.ToString() : "";
                txtDireccionC.Enabled = false;
                txtEmailC.Enabled = false;
                txtNombreC.Enabled = false;
                txtTelefonoC.Enabled = false;
                btnEliminar.Enabled = true;
                btnRegistrar.Enabled = false;
                btnGuardar.Enabled = false;
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            txtDireccionC.Enabled = true;
            txtEmailC.Enabled = true;
            txtNombreC.Enabled = true;
            txtTelefonoC.Enabled = true;
            btnGuardar.Enabled = true;
            btnEliminar.Enabled = false;
            btnRegistrar.Enabled = false;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=.;Initial Catalog=InventarioLaEsquinita;Integrated Security=True";
           
            string query = "UPDATE Cliente SET nombre = @nombre, telefono = @telefono, email = @email, direccion = @direccion WHERE id = @id";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombre", txtNombreC.Text.Trim());
                    cmd.Parameters.AddWithValue("@telefono", txtTelefonoC.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmailC.Text.Trim());
                    cmd.Parameters.AddWithValue("@direccion", txtDireccionC.Text.Trim());
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text.Trim()));

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cliente actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            txtDireccionC.Enabled = false;
            txtEmailC.Enabled = false;
            txtNombreC.Enabled = false;
            txtTelefonoC.Enabled = false;
            btnEliminar.Enabled = true;
            btnGuardar.Enabled = false;

            CargarClientes();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            txtDireccionC.Enabled = true;
            txtEmailC.Enabled = true;
            txtNombreC.Enabled = true;
            txtTelefonoC.Enabled = true;
            btnGuardar.Enabled = false;
            btnEliminar.Enabled = false;
            txtID.Text = "";
            txtDireccionC.Text = "";
            txtEmailC.Text = "";
            txtNombreC.Text = "";
            txtTelefonoC.Text = "";
            btnRegistrar.Enabled = true;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string campoFaltante = "";
            foreach (var control in new[] { txtNombreC, txtTelefonoC, txtEmailC, txtDireccionC })
            {
                switch (control.Name)
                {
                    case "txtNombreC":
                        if (string.IsNullOrWhiteSpace(control.Text))
                            campoFaltante = "Nombre";
                        break;
                    case "txtTelefonoC":
                        if (string.IsNullOrWhiteSpace(control.Text))
                            campoFaltante = "Teléfono";
                        break;
                    case "txtEmailC":
                        if (string.IsNullOrWhiteSpace(control.Text))
                            campoFaltante = "Email";
                        break;
                    case "txtDireccionC":
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

            string query = "INSERT INTO Cliente (nombre, telefono, email, direccion) VALUES (@nombre, @telefono, @email, @direccion)";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombre", txtNombreC.Text.Trim());
                    cmd.Parameters.AddWithValue("@telefono", txtTelefonoC.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmailC.Text.Trim());
                    cmd.Parameters.AddWithValue("@direccion", txtDireccionC.Text.Trim());

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cliente registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            txtDireccionC.Enabled = false;
            txtEmailC.Enabled = false;
            txtNombreC.Enabled = false;
            txtTelefonoC.Enabled = false;
            btnEliminar.Enabled = true;
            btnRegistrar.Enabled = false;

            CargarClientes();
        }

        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            txtDireccionC.Enabled = false;
            txtEmailC.Enabled = false;
            txtNombreC.Enabled = false;
            txtTelefonoC.Enabled = false;
            btnEliminar.Enabled = true;
            btnRegistrar.Enabled = false;
            btnGuardar.Enabled = false;
            txtID.Text = "";
            txtDireccionC.Text = "";
            txtEmailC.Text = "";
            txtNombreC.Text = "";
            txtTelefonoC.Text = "";
        }

        private void Clientes_Load(object sender, EventArgs e)
        {
            CargarClientes();
        }

        private void btnBuscarC_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=.;Initial Catalog=InventarioLaEsquinita;Integrated Security=True";
            string textoBusqueda = txtClientes.Text.Trim();

            string query = "SELECT * FROM Cliente WHERE nombre LIKE @busqueda OR telefono LIKE @busqueda OR email LIKE @busqueda OR direccion LIKE @busqueda";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
                adapter.SelectCommand.Parameters.AddWithValue("@busqueda", "%" + textoBusqueda + "%");
                DataTable tablaClientes = new DataTable();
                adapter.Fill(tablaClientes);
                dgvClientes.DataSource = tablaClientes;
                dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Selecciona un cliente para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show(
                "¿Estás seguro de que deseas eliminar este cliente?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                string connectionString = "Data Source=.;Initial Catalog=InventarioLaEsquinita;Integrated Security=True";
                string query = "DELETE FROM Cliente WHERE id = @id";

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text.Trim()));
                        conexion.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Cliente eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtID.Text = "";
                txtNombreC.Text = "";
                txtTelefonoC.Text = "";
                txtEmailC.Text = "";
                txtDireccionC.Text = "";

                txtDireccionC.Enabled = false;
                txtEmailC.Enabled = false;
                txtNombreC.Enabled = false;
                txtTelefonoC.Enabled = false;
                btnEliminar.Enabled = false;
                btnRegistrar.Enabled = false;
                btnGuardar.Enabled = false;

                CargarClientes();
            }
        }

        private void CargarClientes()
        {
            string connectionString = "Data Source=.;Initial Catalog=InventarioLaEsquinita;Integrated Security=True";
            string query = "SELECT * FROM Cliente";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
                DataTable tablaClientes = new DataTable();
                adapter.Fill(tablaClientes);
                dgvClientes.DataSource = tablaClientes;
                dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }
    }
}
