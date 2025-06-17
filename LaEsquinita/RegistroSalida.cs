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
    public partial class RegistroSalida : Form
    {
        public RegistroSalida()
        {
            InitializeComponent();
        }

        private void btnRegistrarDevolucionP_Click(object sender, EventArgs e)
        {
            RegistrarDevolucionProveedor registrarDevolucionProveedor = new RegistrarDevolucionProveedor();
            registrarDevolucionProveedor.Show();
        }
    }
}
