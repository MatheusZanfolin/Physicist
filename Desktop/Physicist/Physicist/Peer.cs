using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace Physicist
{
    public class Peer
    {
        const int intervaloTimer = 1000;
        private static System.Threading.Timer timer;
        public static Task<UdpReceiveResult> broadcasting;
        public static Task<int> respostaBroadcasting;
        private const int portaBroadcast = 1639;
        private const int portaEnviar = 1729;
        private const string msgReq = "Requisitando";
        private const string msgResp = "Respondendo";
        private static IPAddress endMulticast = IPAddress.Parse("236.0.0.0");
        private bool receptorAtribuido = false;
        private static byte[] msgRespBytes = (byte[])Encoding.ASCII.GetBytes(msgResp);
        private IPAddress IP;
        private IPEndPoint meuEnd;
        static UdpClient servidorBroadcast;
        static UdpClient emissorResposta;
        String nome;
        private static IPEndPoint iPConectando;
        public String ToString()
        {
            return this.IP.ToString();
        }
        public IPEndPoint IPEndPoint
        {
            get
            {
                return new IPEndPoint(IP, portaEnviar);
            }
        }
        public IPEndPoint IPConectando{
            get
            {
                return iPConectando;
            }
            
        }
        private void inicializarTimer(int indTarefa)
        {//não tenho ctz se ele é async ou não
            //indTarefa = 0 => broadcasting
            //indTarefa = 1 => respostaBroadcasting
            //estado inicial falso, ficará true caso chegar no estado callback
            var autoEvento = new AutoResetEvent(false);
            var checador = new ChecadorStatus(indTarefa);
            timer = new Timer(checador.CheckStatus, autoEvento, intervaloTimer, intervaloTimer);
            //autoEvento.WaitOne();
            //liberar e reiniciar o timer
            //"flag" autoevento foi alterada

            /*timer.Dispose();
	        throw new Exception("Timer Peer acabou!");*/
        }
        public static void finalizarTimer() {
            try
            {
                timer.Dispose();
                timer = null;
            }
            catch{ }
            if (broadcasting != null && respostaBroadcasting == null)
            {
                if (estadoBroadcasting() == TaskStatus.RanToCompletion)
                {
                    tratarBroadcast();
                }
                else
                {
                    finalizarBroadcasting();
                }
                        
            }
            if(respostaBroadcasting != null)
            {
                if (estadoRespostaBroadcasting() == TaskStatus.RanToCompletion)
                {
                    finalizarRespostaBroadcasting();

                    ConexaoP2P.inicializarPeer(3);
                }
                else
                {
                
                    finalizarRespostaBroadcasting();
                }
            }
        }

        private static void tratarBroadcast() {
            //timer.Dispose();
            //não sei se isso deve ser realmente assíncrono
            IPEndPoint IPQuemMandou;
            UdpReceiveResult req;
            //CancellationToken token = new CancellationToken(true);

            //receber();
            //byte[] datagrama = servidorBroadcast.ReceiveAsync(/*ref IPQuemMandou*/);
            req = broadcasting.Result;
            string requisição = Encoding.ASCII.GetString(req.Buffer);
            IPQuemMandou = req.RemoteEndPoint;

            finalizarBroadcasting();
            if (!requisição.Equals("Requisitando"))
            {
                //throw new SocketException("Ataque vírus!!!");
                throw new Exception("Esperava por requisição \" Requisitando\", porém achou \" "+ requisição +" \" !");
            }
            iPConectando = IPQuemMandou;
            ConexaoP2P.tratarBroadcast();
	        //throw new Exception("B");
        }
        
        public async void receber()
        {
            /*try
            {*/
                broadcasting = (servidorBroadcast.ReceiveAsync());
                inicializarTimer(0);
            
            /*}
            catch (Exception ex)//ObjectDisposedException pode ser tbm
            {
		            

            }*/
        }
        public async void enviar()
        {
            /*try
            {*/
                respostaBroadcasting = (emissorResposta.SendAsync(Peer.msgRespBytes, Peer.msgRespBytes.Length));
                inicializarTimer(1);
            /*}
            catch (Exception ex)//ObjectDisposedException pode ser tbm
            {
                switch (estadoRespostaBroadcasting())
                {
                    case TaskStatus.Faulted:
                        this.finalizarRespostaBroadcasting();
                        throw new Exception("Caso aparentemente impossível!");
                        break;
                    case TaskStatus.RanToCompletion:
                        //deu tudo certo!!!
                        //não dar dispose!!
                        //esperar resposta de conexão!
                        throw new Exception("C");
                        break;
                    case TaskStatus.WaitingForChildrenToComplete:
                        this.finalizarRespostaBroadcasting();
                        throw new Exception("Comportamento inesperado da Task");
                        break;
                    case TaskStatus.Canceled:

                        this.finalizarRespostaBroadcasting();
                        throw new Exception("Task mal inicializada!");
                        break;
                    default:
                        this.finalizarRespostaBroadcasting();
                        throw new Exception("Comportamento insesperado do Timer");

                }

            }*/
        }
        public static TaskStatus estadoBroadcasting()
        {
            return broadcasting.Status;
        }
        public static TaskStatus estadoRespostaBroadcasting()
        {
            return respostaBroadcasting.Status;
        }
        public void inicializarBroadcasting()
        {//   236.0.0.0
            

            if (this.receptorAtribuido)
            {
                try
                {
                    servidorBroadcast.Close();
                    servidorBroadcast.DropMulticastGroup(endMulticast);
                    servidorBroadcast.Dispose();
                    servidorBroadcast = null;
                }
                catch(Exception ex)
                {
                    servidorBroadcast = null;
                    broadcasting = null;
                }
            }
                
            
                servidorBroadcast = new UdpClient();
                

                servidorBroadcast.DontFragment = true;//não quero que fragmente os pacotes
                this.meuEnd = new IPEndPoint(IPAddress.Any, portaBroadcast);
                servidorBroadcast.Client.Bind(meuEnd);
                //servidorBroadcast.
                servidorBroadcast.JoinMulticastGroup(endMulticast);
                this.receptorAtribuido = true;
            
           

        }
        public static void finalizarBroadcasting()
        {
            try
            {
                if (broadcasting != null)
                {
                    broadcasting.Dispose();
                    broadcasting = null;
                }
                if (servidorBroadcast != null)
                {
                    servidorBroadcast.Close();
                    //servidorBroadcast.DropMulticastGroup(endMulticast);
                    // servidorBroadcast.Dispose();
                }
                if (timer != null)
			        timer.Dispose();
                 }
            catch {}
        }
        public void inicializarRespostaBroadcasting(IPEndPoint aConectar)
        {
            Peer.iPConectando = aConectar;
           /* servidorBroadcast.Dispose();
            servidorBroadcast = null;*/
            Peer.iPConectando.Port = portaEnviar;
            emissorResposta = new UdpClient();
            emissorResposta.DontFragment = true;
            emissorResposta.Connect(IPConectando);
            emissorResposta.EnableBroadcast = false;//pode enviar e/ou receber broadcast
            emissorResposta.MulticastLoopback = true;
            //uma mensagem será enviada para o dispositivo que fez um multicast

        }
        public static void finalizarRespostaBroadcasting()
        {
            try
            {
                if (respostaBroadcasting != null)
                    respostaBroadcasting.Dispose();
                if (emissorResposta != null)
                {
                    emissorResposta.Close();
                    
                    //servidorBroadcast.DropMulticastGroup(endMulticast);
                   // servidorBroadcast.Dispose();
                }
                if (servidorBroadcast != null)
                {
                    servidorBroadcast.DropMulticastGroup(endMulticast);
                    servidorBroadcast.Dispose();
                    servidorBroadcast = null;
                }
                if (timer != null)
                    timer.Dispose();
            }
            catch { }
        }

        public void Dispose()
        {
            respostaBroadcasting.Dispose();
            broadcasting.Dispose();

            timer.Dispose();
            servidorBroadcast.DropMulticastGroup(endMulticast);
            servidorBroadcast.Dispose();
            emissorResposta.Dispose();
            
        }

        //construtor para peers remotos
        public Peer(IPEndPoint ipRemoto)
        {
            if (ipRemoto == null)
                throw new ArgumentNullException("IP remoto nulo");
            this.IP = ipRemoto.Address;
        }
        //construtor para próprio peer(localhost)
        public Peer(IPAddress meuIP) {
            if (meuIP != null)
                this.IP = meuIP;
            else
                throw new ArgumentNullException("IP local nulo");

        }

    }
    
}
