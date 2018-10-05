using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Physicist
{
    class ConexaoP2P
    {
        const int intervaloTimer = 1000;

        private static List<Peer> peers;
        public const int portaP2P = 1337;
        private static TcpListener receptorP2P;
        private static TcpClient emissorP2P;
        bool conectadoP2P;
        private static IPAddress meuIP;
        public static Task<TcpClient> escutarPeer;
        //public static Task<int> lerStream;
        const int indTarefa = 2;
        private static System.Threading.Timer timerP2P;
        private static byte[] buffer;
        //private const string msgReq = "Requisitando";
        //precisamos fazer um "protocolo" para interpretar os dados recebidos
        public static void inicializarPeer()
        {
            receptorP2P = new TcpListener(meuIP, portaP2P);
            receptorP2P.Start();
            aceitarPeer();
        }
        public static void inicializarTimer(/*int indTarefa*/)
        {//não tenho ctz se ele é async ou não
            var autoEvento = new AutoResetEvent(false);
            var checador = new ChecadorStatus(indTarefa);
            timerP2P = new Timer(checador.CheckStatus, autoEvento, 0, intervaloTimer);
            //autoEvento.WaitOne();
            //liberar e reiniciar o timer
            //"flag" autoevento foi alterada
            //timerP2P.Dispose();
            //throw new Exception("Timer Peer to Peer acabou!");
            
        }
        public static void finalizarTimer()
        {
            if(ConexaoP2P.estadoEscutaPeer() == TaskStatus.RanToCompletion)
            {
                ConexaoP2P.tratarDados();
            }
            else
            {
                ConexaoP2P.finalizarConexao();
            }
        }

        public static async void aceitarPeer()
        {
           /* try
            {*/
                escutarPeer = receptorP2P.AcceptTcpClientAsync();
                inicializarTimer();
            /*}
            catch(Exception ex)
            {
                switch (estadoEscutaPeer())
                {
                    case TaskStatus.Faulted:
                        finalizarConexao();
                        throw new Exception("Caso aparentemente impossível!");
                        break;
                    case TaskStatus.RanToCompletion:
                        //deu tudo certo!!!
                        //não dar dispose!!
                        //tratar Broadcasting!
                        throw new Exception("D");
                        break;
                    case TaskStatus.WaitingForChildrenToComplete:
                        finalizarConexao();
                        throw new Exception("Comportamento inesperado da Task");
                        break;
                    case TaskStatus.Canceled:

                        finalizarConexao();
                        throw new Exception("Task mal inicializada!");
                        break;
                    default:
                        finalizarConexao();
                        throw new Exception("Comportamento inesperado do Timer");
                }
            }*/
        }
        public static async void tratarDados()
        {
            //sla como faço isso por enquanto
            emissorP2P = escutarPeer.Result;
            NetworkStream stream = emissorP2P.GetStream();
            byte[] meuBuffer = new byte[2048];
            int res = await stream.ReadAsync(meuBuffer, 0, 2048);
            if (res < 0)
                throw new Exception("Leitura de Buffer não deu certo!");
            byte[] buffer = new byte[res];
            for (int i = 0; i < res; i++)
                buffer[i] = meuBuffer[i];
            finalizarConexao();
            Controlador.tratarDados();
        }
        public static byte[] Buffer
        {
            get
            {
                return buffer;
            }
        }

        public async void testarBroadcasting() {
            //try{
			    peers[0].receber();
        	/*}
		    catch(Exception ex){
			    if(ex.Message == "A")
				    throw new Exception("A");
			    else
				    throw new Exception("Broadcasting falhou!");	
		    }*/
	    }
       // public static void depois
        public async void responderBroadcasting()
        {
            /*try
            {*/
                peers[0].enviar();
            /*}
            catch(Exception ex)
            {
                if (ex.Message == "C")
                    throw new Exception("C");
                else
                    throw new Exception("Resposta ao Broadcasting falhou!");
            }*/
        }
	    public static Peer ultimoPeer(){
		    if(peers.Count != 0)
			    return peers.Last();
		    else
			    throw new Exception("Não foi achado nenhum outro peer ainda!");
	    }
        public static void finalizarConexao() {
            try
            {
                if (escutarPeer != null)
                    escutarPeer.Dispose();
                if (emissorP2P != null)
                {
                    emissorP2P.Close();
                    //emissorP2P = null;
                }
                if (receptorP2P != null)
                {
                    receptorP2P.Stop();
                    //receptorP2P = null;
                }
                if (timerP2P != null)
                    timerP2P.Dispose();
            }
            catch { }
        }
        public void finalizarBroadcasting()
        {

            // receptorP2P.Stop();
            Peer.finalizarBroadcasting();
            
        }
        public void inicializarBroadcasting()
        {
            peers[0].inicializarBroadcasting();
        }
        public void finalizarRespostaBroadcasting()
        {
            //peers[0].finalizarRespostaBroadcasting();
            // receptorP2P.Stop();
            Peer.finalizarRespostaBroadcasting();
        }
        public void inicializarRespostaBroadcasting()
        {
            peers[0].inicializarRespostaBroadcasting();
        }
        public static async void tratarBroadcast()
        {
            IPEndPoint ipAchado = peers[0].IPConectando;
            Peer novoPeer = new Peer(ipAchado);
            peers.Add(novoPeer);
            Controlador.tratarBroadcast();
               
                   // throw new Exception("Caso aparentemente impossível!");
                    //caso aparentemente impossível
        } 
        public TaskStatus estadoBroadcasting()
        {
            return Peer.estadoBroadcasting();
        }
        public TaskStatus estadoRespostaBroadcasting()
        {
            return Peer.estadoRespostaBroadcasting();
        }
        public static TaskStatus estadoEscutaPeer()
        {
            return ConexaoP2P.escutarPeer.Status;
        }
        public ConexaoP2P(IPAddress IP)
        {
            if (IP == null)
                throw new ArgumentNullException("ConexãoP2P: IP nulo");
            meuIP = IP;
            peers = new List<Peer>();
            peers.Insert(0, new Peer(meuIP));
            //receptorP2P = new TcpListener(IPAddress.Any, ConexaoP2P.portaP2P);
            conectadoP2P = false;

        }
        /*public int acharIndicePeer(String ip)
        {

            IPAddress esseIp = new IPEndPoint()
        }*/
    }
}
