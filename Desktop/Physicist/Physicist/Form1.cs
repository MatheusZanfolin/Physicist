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
        private static List<Peer> listaDispositivos = new List<Peer>();
        byte[] buffer;
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
                btnListar.Text = "Cancelar Busca";
                while (true)
                {
                    meuControlador.inicializarBroadcasting();
                    procurarDispositivos();
                    //procurarDispositivos é async
                    inserirNaLista();
                }

            }
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            if (ehPossivelCancelarBroadcasting)
            {
                meuControlador.finalizarRespostaBroadcasting();
                ehPossivelCancelar = false;
                btnConectar.Text = "Conectar com esse dispositivo";
            }
            else
            {
                ehPossivelCancelar = true;
                btnConectar.Text = "Cancelar";
                meuControlador.inicializarRespostaBroadcasting();
                responderPeer(lsbDispositivos.SelectedIndex);
                //responder peer é async
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
                if(ex.Message == "D")
                {
                    this.interpretarBuffer(meuControlador.Buffer);
                    //interpretar o buffer
                }
            }
        }
        private void interpretarBuffer(byte[] buffer)
        {
            // enfileirar dados
        }
        private async void procurarDispositivos()
        {
		    //try{
                meuControlador.testarBroadcasting();
		    /*}
		    catch(Exception ex){
			    if(ex.Message == "B"){
				    
			    }
			    else{
				    MessageBox.Show("Busca cancelada!");
			    }	
		    }*/

        }
        private void inserirNaLista()
        {
            if (listaDispositivos.Count < 1)
            {
                MessageBox.Show("Busca cancelada!");
            }
            else
            {
                lsbDispositivos.Items.Add(listaDispositivos.Last().ToString());
            }
        }
        public static void listarDispositivos() {
            Peer achado = meuControlador.PeerAchado;
            listaDispositivos.Add(achado);
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            meuControlador = new Controlador();

        }
    }
}
