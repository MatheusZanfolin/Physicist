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
        bool primeiraEscuta = true;
        Task obtencao = null;
        bool escutando = true;
        public static bool novoPeer = false;
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
        

        private void btnConectar_Click(object sender, EventArgs e)
        {
            escutando = false;
                if(!DesenhavelRepositorio.estaVazio())
                    atualizarForm();
            btnConectar.Text = "Verificar resposta";
            meuControlador.inicializarRespostaBroadcasting(lsbDispositivos.SelectedIndex);
            responderPeer();
            meuControlador.finalizarRespostaBroadcasting();
            //.. esperar a resposta
                //responder peer é async
                
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            obterPeers();
        }
    
        private async void responderPeer()
        {
           meuControlador.responderBroadcasting();
           
        }
       
        private async void procurarDispositivos()
        {
            meuControlador.testarBroadcasting();
		 
        }
        private void atualizarForm()
        {
            //Form2 novoform = new Form2();
            //novoform.Show();
            MessageBox.Show("Recebeu o TCP");
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
        private void obterPeers()
        {
            btnConectar.Visible = false;
            
            if(primeiraEscuta)
                meuControlador = new ControladorConexao();
            escutando = true;
            Action<object> obterPeers = (object obj) =>
            {
                bool receber = true;

                while (escutando)
                {
                    if (!novoPeer)
                    {
                        if (primeiraEscuta)
                        {
                            meuControlador.inicializarBroadcasting();
                            primeiraEscuta = false;
                        }
                        if (receber)
                        {
                            procurarDispositivos();
                            meuControlador.finalizarBroadcasting();
                            receber = false;
                        }
                        
                    }
                    else
                    {
                        listarDispositivos();
                        inserirNaLista();
                        btnConectar.Visible = true;
                        novoPeer = false;
                        receber = true;
                    }
                    
                }
            };
            obtencao = new Task(obterPeers, "obtenção de Peers");
            obtencao.Start();
        }
        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            if (obtencao != null)
            {
                escutando = false;
                //obtencao.Dispose();

            }
            obtencao = null;
            obterPeers();
        }
    }
}
