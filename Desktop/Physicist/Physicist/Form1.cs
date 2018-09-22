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
        bool ehPossivelCancelar = false;
        private static Controlador meuControlador;
        List<Peer> listaDispositivos = new List<Peer>();
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            if (ehPossivelCancelar)
            {
                meuControlador.finalizarBroadcasting();
                ehPossivelCancelar = false;
                btnListar.Text = "BuscarDispositivos";
            }
            else {
                meuControlador = new Controlador();
                ehPossivelCancelar = true;
                btnListar.Text = "Cancelar";
                procurarDispositivos();
                //procurarDispositivos é async
            }
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {

        }
        private async void procurarDispositivos()
        {
            meuControlador.testarBroadcasting();

        }
        
    }
}
