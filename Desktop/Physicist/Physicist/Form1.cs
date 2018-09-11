using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Physicist
{
    public partial class frmPrincipal : Form
    {
        private static Controlador meuControlador;
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            meuControlador = new Controlador();
            procurarDispositivos
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {

        }
        private void procurarDispositivos()
        {

            meuControlador.tratarBroadcast();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            meuControlador.finalizarBroadcasting()
        }
    }
}
