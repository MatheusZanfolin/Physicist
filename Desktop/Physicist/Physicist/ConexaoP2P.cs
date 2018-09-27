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

        private List<Peer> peers;
        public const int portaP2P = 1337;
        TcpListener receptorP2P;
        TcpClient emissorP2P;
        bool conectadoP2P;
        IPAddress meuIP;
        public static Task<TcpClient> escutarPeer;
        //public static Task<int> lerStream;
        const int indTarefa = 2;
        System.Threading.Timer timerP2P;
        private byte[] buffer;
        //private const string msgReq = "Requisitando";
        //precisamos fazer um "protocolo" para interpretar os dados recebidos
        public void inicializarPeer()
        {
            this.receptorP2P = new TcpListener(meuIP, portaP2P);
            this.receptorP2P.Start();
        }
        private void inicializarTimer(/*int indTarefa*/)
        {//não tenho ctz se ele é async ou não
            var autoEvento = new AutoResetEvent(false);
            var checador = new ChecadorStatus(indTarefa);
            timerP2P = new Timer(checador.checarStatus, autoEvento, 0, intervaloTimer);
            autoEvento.WaitOne();
            //liberar e reiniciar o timer
            //"flag" autoevento foi alterada
            timerP2P.Dispose();
            throw new Exception("Timer Peer to Peer acabou!");
        }

        public async void aceitarPeer()
        {
            try
            {
                escutarPeer = this.receptorP2P.AcceptTcpClientAsync();
                inicializarTimer();
            }
            catch(Exception ex)
            {
                switch (estadoEscutaPeer())
                {
                    case TaskStatus.Faulted:
                        this.finalizarConexao();
                        throw new Exception("Caso aparentemente impossível!");
                        break;
                    case TaskStatus.RanToCompletion:
                        //deu tudo certo!!!
                        //não dar dispose!!
                        //tratar Broadcasting!
                        throw new Exception("D");
                        break;
                    case TaskStatus.WaitingForChildrenToComplete:
                        this.finalizarConexao();
                        throw new Exception("Comportamento inesperado da Task");
                        break;
                    case TaskStatus.Canceled:

                        this.finalizarConexao();
                        throw new Exception("Task mal inicializada!");
                        break;
                    default:
                        this.finalizarConexao();
                        throw new Exception("Comportamento inesperado do Timer");
                }
            }
        }
        public async void tratarDados()
        {
            //sla como faço isso por enquanto
            this.emissorP2P = escutarPeer.Result;
            NetworkStream stream = emissorP2P.GetStream();
            byte[] meuBuffer = new byte[2048];
            int res = await stream.ReadAsync(meuBuffer, 0, 2048);
            if (res < 0)
                throw new Exception("Leitura de Buffer não deu certo!");
            byte[] buffer = new byte[res];
            for (int i = 0; i < res; i++)
                buffer[i] = meuBuffer[i];

        }
        public byte[] Buffer
        {
            get
            {
                return this.buffer;
            }
        }

        public async void testarBroadcasting() {
            try{
			    peers[0].receber();
        	}
		    catch(Exception ex){
			    if(ex.Message == "A")
				    throw new Exception("A");
			    else
				    throw new Exception("Broadcasting falhou!");	
		    }
	    }
        public async void responderBroadcasting()
        {
            try
            {
                peers[0].enviar();
            }
            catch(Exception ex)
            {
                if (ex.Message == "C")
                    throw new Exception("C");
                else
                    throw new Exception("Resposta ao Broadcasting falhou!");
            }
        }
	    public Peer ultimoPeer(){
		    if(this.peers.Count != 0)
			    return this.peers.Last();
		    else
			    throw new Exception("Não foi achado nenhum outro peer ainda!");
	    }
        public void finalizarConexao() {
            try
            {
                if (escutarPeer != null)
                    escutarPeer.Dispose();
                if (emissorP2P != null)
                {
                    emissorP2P.Close();
                    emissorP2P = null;
                }
                if (receptorP2P != null)
                {
                    receptorP2P.Stop();
                    receptorP2P = null;
                }
                if (timerP2P != null)
                    timerP2P.Dispose();
            }
            catch { }
        }
        public void finalizarBroadcasting()
        {
            this.peers[0].finalizarBroadcasting();
           // this.receptorP2P.Stop();
            
        }
        public void inicializarBroadcasting()
        {
            this.peers[0].inicializarBroadcasting();
        }
        public void finalizarRespostaBroadcasting()
        {
            this.peers[0].finalizarRespostaBroadcasting();
            // this.receptorP2P.Stop();

        }
        public void inicializarRespostaBroadcasting()
        {
            this.peers[0].inicializarRespostaBroadcasting();
        }
        public async void tratarBroadcast()
        {
            try
            {
                this.peers[0].tratarBroadcast();
            }
            catch (Exception ex)
            {
                if (ex.Message == "B")
                {
                    IPEndPoint ipAchado = this.peers[0].IPConectando;
                    Peer novoPeer = new Peer(ipAchado);
                    peers.Add(novoPeer);
                }
                else
                    throw new Exception("Caso aparentemente impossível!");
                    //caso aparentemente impossível
            }
        } 
        public TaskStatus estadoBroadcasting()
        {
            return this.peers[0].estadoBroadcasting();
        }
        public TaskStatus estadoRespostaBroadcasting()
        {
            return this.peers[0].estadoRespostaBroadcasting();
        }
        public TaskStatus estadoEscutaPeer()
        {
            return ConexaoP2P.escutarPeer.Status;
        }
        public ConexaoP2P(IPAddress meuIP)
        {
            if (meuIP == null)
                throw new ArgumentNullException("ConexãoP2P: IP nulo");
            this.meuIP = meuIP;
            this.peers = new List<Peer>();
            this.peers.Insert(0, new Peer(meuIP));
            //this.receptorP2P = new TcpListener(IPAddress.Any, ConexaoP2P.portaP2P);
            this.conectadoP2P = false;

        }
        /*public int acharIndicePeer(String ip)
        {

            IPAddress esseIp = new IPEndPoint()
        }*/
    }
}
