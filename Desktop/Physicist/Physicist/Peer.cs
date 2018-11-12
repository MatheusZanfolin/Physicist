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
        public static Task<UdpReceiveResult> Multicasting;
        public static Task<int> respostaMulticasting;
        private const int portaMulticast = 1639;
        private const int portaEnviar = 1729;
        private const string msgReq = "Requisitando";
        private const string msgResp = "Respondendo";
        private static IPAddress endMulticast = IPAddress.Parse("236.0.0.0");
        private bool receptorAtribuido = false;
        private static byte[] msgRespBytes = (byte[])Encoding.ASCII.GetBytes(msgResp);
        private IPAddress IP;
        private IPEndPoint meuEnd;
        static UdpClient servidorMulticast;
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
            //indTarefa = 0 => Multicasting
            //indTarefa = 1 => respostaMulticasting
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
            if (Multicasting != null && respostaMulticasting == null)
            {
                if (estadoMulticasting() == TaskStatus.RanToCompletion)
                {
                    tratarMulticast();
                }
                else
                {
                    finalizarMulticasting();
                }
                        
            }
            if(respostaMulticasting != null)
            {
                if (estadoRespostaMulticasting() == TaskStatus.RanToCompletion)
                {
                    finalizarRespostaMulticasting();

                    ConexaoP2P.inicializarPeer(3);
                }
                else
                {
                
                    finalizarRespostaMulticasting();
                }
            }
        }

        private static void tratarMulticast() {
            //timer.Dispose();
            //não sei se isso deve ser realmente assíncrono
            IPEndPoint IPQuemMandou;
            UdpReceiveResult req;
            //CancellationToken token = new CancellationToken(true);

            //receber();
            //byte[] datagrama = servidorMulticast.ReceiveAsync(/*ref IPQuemMandou*/);
            req = Multicasting.Result;
            string requisição = Encoding.ASCII.GetString(req.Buffer);
            IPQuemMandou = req.RemoteEndPoint;

            finalizarMulticasting();
            if (!requisição.Equals("Requisitando"))
            {
                //throw new SocketException("Ataque vírus!!!");
                throw new Exception("Esperava por requisição \" Requisitando\", porém achou \" "+ requisição +" \" !");
            }
            iPConectando = IPQuemMandou;
            ConexaoP2P.tratarMulticast();
	        //throw new Exception("B");
        }
        
        public async void receber()
        {
            /*try
            {*/
                Multicasting = (servidorMulticast.ReceiveAsync());
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
                respostaMulticasting = (emissorResposta.SendAsync(Peer.msgRespBytes, Peer.msgRespBytes.Length));
                inicializarTimer(1);
            /*}
            catch (Exception ex)//ObjectDisposedException pode ser tbm
            {
                switch (estadoRespostaMulticasting())
                {
                    case TaskStatus.Faulted:
                        this.finalizarRespostaMulticasting();
                        throw new Exception("Caso aparentemente impossível!");
                        break;
                    case TaskStatus.RanToCompletion:
                        //deu tudo certo!!!
                        //não dar dispose!!
                        //esperar resposta de conexão!
                        throw new Exception("C");
                        break;
                    case TaskStatus.WaitingForChildrenToComplete:
                        this.finalizarRespostaMulticasting();
                        throw new Exception("Comportamento inesperado da Task");
                        break;
                    case TaskStatus.Canceled:

                        this.finalizarRespostaMulticasting();
                        throw new Exception("Task mal inicializada!");
                        break;
                    default:
                        this.finalizarRespostaMulticasting();
                        throw new Exception("Comportamento insesperado do Timer");

                }

            }*/
        }
        public static TaskStatus estadoMulticasting()
        {
            return Multicasting.Status;
        }
        public static TaskStatus estadoRespostaMulticasting()
        {
            return respostaMulticasting.Status;
        }
        public void inicializarMulticasting()
        {//   236.0.0.0
            

            if (this.receptorAtribuido)
            {
                try
                {

                    servidorMulticast.Close();
                    servidorMulticast.DropMulticastGroup(endMulticast);

                    servidorMulticast.Dispose();
                    servidorMulticast = null;
                }
                catch(Exception ex)
                {
                    servidorMulticast = null;
                    Multicasting = null;
                }
            }
                
            
            servidorMulticast = new UdpClient();
            
            servidorMulticast.DontFragment = true;//não quero que fragmente os pacotes
               //this.meuEnd = new IPEndPoint(IPAddress.Any, portaMulticast);
            this.meuEnd = new IPEndPoint(IP, portaMulticast);
            servidorMulticast.Client.Bind(meuEnd);
            servidorMulticast.EnableBroadcast = true;
            servidorMulticast.JoinMulticastGroup(endMulticast);
            this.receptorAtribuido = true;
            
           

        }
        public static void finalizarMulticasting()
        {
            try
            {
                if (Multicasting != null)
                {
                    Multicasting.Dispose();
                    Multicasting = null;
                }
                if (servidorMulticast != null)
                {
                    servidorMulticast.Close();
                    //servidorMulticast.DropMulticastGroup(endMulticast);
                    // servidorMulticast.Dispose();
                }
                if (timer != null)
			        timer.Dispose();
                 }
            catch {}
        }
        public void inicializarRespostaMulticasting(IPEndPoint aConectar)
        {
            Peer.iPConectando = aConectar;
           /* servidorMulticast.Dispose();
            servidorMulticast = null;*/
            Peer.iPConectando.Port = portaEnviar;
            emissorResposta = new UdpClient();
            emissorResposta.DontFragment = true;
            emissorResposta.Connect(IPConectando);
            emissorResposta.EnableBroadcast = false;//pode enviar e/ou receber Multicast
            emissorResposta.MulticastLoopback = true;
            //uma mensagem será enviada para o dispositivo que fez um multicast

        }
        public static void finalizarRespostaMulticasting()
        {
            try
            {
                if (respostaMulticasting != null)
                    respostaMulticasting.Dispose();
                if (emissorResposta != null)
                {
                    emissorResposta.Close();
                    
                    //servidorMulticast.DropMulticastGroup(endMulticast);
                   // servidorMulticast.Dispose();
                }
                if (servidorMulticast != null)
                {
                    servidorMulticast.DropMulticastGroup(endMulticast);
                    servidorMulticast.Dispose();
                    servidorMulticast = null;
                }
                if (timer != null)
                    timer.Dispose();
            }
            catch { }
        }

        public void Dispose()
        {
            respostaMulticasting.Dispose();
            Multicasting.Dispose();

            timer.Dispose();
            servidorMulticast.DropMulticastGroup(endMulticast);
            servidorMulticast.Dispose();
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
