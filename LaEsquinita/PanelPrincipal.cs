using LaEsquinata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaEsquinita
{
    public partial class PanelPrincipal : Form
    {
        public PanelPrincipal()
        {
            InitializeComponent();
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            Productos productos = new Productos();
            productos.Show();
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            Clientes productos = new Clientes();
            productos.Show();
        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
            Proveedores proveedores = new Proveedores();
            proveedores.Show();
        }

        private void btnRegistrarEntrada_Click(object sender, EventArgs e)
        {
            RegistroEntrada entrada = new RegistroEntrada();
            entrada.Show();
        }

        private void btnRegistrarSalida_Click(object sender, EventArgs e)
        {
            RegistroSalida salida = new RegistroSalida();
            salida.Show();
        }
    }
}
