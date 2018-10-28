using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Physicist
{
    public partial class frmPrincipal : Form
    {
        bool ehPossivelCancelar = false;
        bool ehPossivelCancelarResposta= false;
        private static ControladorConexao meuControlador;
        private static List<Peer> listaDispositivos = new List<Peer>();
        private static byte[] buffer;
        private static int numElementosListaForm = 0;
        //private static System.Windows.Forms.Timer timerLista;
        private static bool flagSairTimer;
       
        //private static DesenhavelRepositorio repDes = null;
        //private const byte flagImagem = 0x00;
  
        public frmPrincipal()
        {
            InitializeComponent();
            
        }
        /*private static void TimerEventProcessor(Object myObject,
                                            EventArgs myEventArgs)
        {
            timerLista.Stop();

            // Displays a message box asking whether to continue running the timer.
            if (listaDispositivos.Count != numElementosListaForm)
            {
                // Restarts the timer and increments the counter.
                
                timerLista.Enabled = true;
            }
            else
            {
                // Stops the timer.
                flagSairTimer = true;
            }
        }
        private void inicializarTimer()
        {
            timerLista = new System.Windows.Forms.Timer();
            timerLista.Tick += new EventHandler(TimerEventProcessor);
            timerLista.Interval = 1000;
            timerLista.Start();
        }*/
        private void btnListar_Click(object sender, EventArgs e)
        {
            if (ehPossivelCancelar)
            {
                inserirNaLista();
                meuControlador.finalizarBroadcasting();
                ehPossivelCancelar = false;
                btnListar.Text = "Buscar Dispositivos";
            }
            else {
                ehPossivelCancelar = true;
                btnListar.Text = "Verificar Busca";
                meuControlador.inicializarBroadcasting();
                procurarDispositivos();
                meuControlador.finalizarBroadcasting();
              

            }
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            if (ehPossivelCancelarResposta)
            {
                if(!DesenhavelRepositorio.estaVazio())
                    atualizarForm();
                meuControlador.finalizarRespostaBroadcasting();
                ehPossivelCancelar = false;
                btnConectar.Text = "Conectar com esse dispositivo";
            }
            else
            {
                ehPossivelCancelar = true;
                btnConectar.Text = "Verificar resposta";
                meuControlador.inicializarRespostaBroadcasting();
                responderPeer(lsbDispositivos.SelectedIndex);
                meuControlador.finalizarRespostaBroadcasting();
                //.. esperar a resposta
                //responder peer é async
                
            }

        }
        private async void responderPeer(int indice)
        {
           /* try
            {*/
                meuControlador.responderBroadcasting();
           
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
        private void atualizarForm()
        {
            Form2 novoform = new Form2();
            novoform.Show();
            //CHAMAR Form2
        }
        private void inserirNaLista()
        {
            if (numElementosListaForm != listaDispositivos.Count)
            
            {
                lsbDispositivos.Items.Add(listaDispositivos.Last().ToString());
                numElementosListaForm++;
            }
        }
        public static void listarDispositivos() {
            Peer achado = ControladorConexao.PeerAchado;
            listaDispositivos.Add(achado);
            
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            
            meuControlador = new ControladorConexao();

        }
    }
}
