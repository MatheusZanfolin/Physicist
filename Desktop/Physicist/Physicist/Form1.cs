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
        bool ehPossivelCancelarBroadcasting = false;
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
                btnListar.Text = "Buscar Dispositivos";
            }
            else {
                ehPossivelCancelar = true;
                btnListar.Text = "Cancelar";
                meuControlador.inicializarBroadcasting();
                procurarDispositivos();
                //procurarDispositivos é async
		    }
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            if (ehPossivelCancelarBroadcasting)
            {
                meuControlador.finalizarBroadcasting();
                btnConectar.Text = "Conectar com esse dispositivo\";
            }
            else
            {
                ehPossivelCancelar = true;
                btnConectar.Text = "Cancelar";
                meuControlador.inicializarBroadcasting();
                responderPeer(lsbDispositivos.SelectedIndex);
            }

        }
        private async void responderPeer(int indice)
        {
            try
            {
                meuControlador.responderBroadcasting();
            }
            catch(Exception ex)
            {

            }
        }
        private async void procurarDispositivos()
        {
		    try{
                meuControlador.testarBroadcasting();
		    }
		    catch(Exception ex){
			    if(ex.Message == "B"){
				    Peer achado = meuControlador.PeerAchado;
				    listaDispositivos.Add(achado);
				    lsbDispositivos.Items.Add(listaDispositivos.Last().ToString());

                    procurarDispositivos();
			    }
			    else{
				    MessageBox.Show("Busca cancelada!");
			    }	
		    }

        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            meuControlador = new Controlador();

        }
    }
}
