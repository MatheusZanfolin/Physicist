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
        const int intervaloTimer = 3000;
        private static System.Threading.Timer timer;
        public static Task<UdpReceiveResult> broadcasting;
        const int portaBroadcast = 1729;
        private IPAddress IP;
        UdpClient servidorBroadcast;
        String nome;
        bool conectadoBroadcast;
        private IPEndPoint iPConectando;
        public String ToString()
        {
            return this.IP.ToString();
        }
        public IPEndPoint IPConectando{
            get
            {
                return this.iPConectando;
            }
            
        }
        private void inicializarTimer()
        {
            //estado inicial falso, ficará true caso chegar no estado callback
            var autoEvento = new AutoResetEvent(false);
            var checador = new ChecadorStatus();
            timer = new Timer(checador.checarStatus, autoEvento, intervaloTimer, intervaloTimer);
            autoEvento.WaitOne();
            //liberar e reiniciar o timer
            //"flag" autoevento foi alterada
            timer.Dispose();
	    throw new Exception("Timer acabou!");
        }

        public async void tratarBroadcast() {
            IPEndPoint IPQuemMandou;
            UdpReceiveResult req;
            //CancellationToken token = new CancellationToken(true);

            //receber();
            //byte[] datagrama = servidorBroadcast.ReceiveAsync(/*ref IPQuemMandou*/);
            req = broadcasting.Result;
            string requisição = Encoding.ASCII.GetString(req.Buffer);
            IPQuemMandou = req.RemoteEndPoint;

            this.finalizarBroadcasting();
            if (!requisição.Equals("Requisitando"))
            {
                //throw new SocketException("Ataque vírus!!!");
                throw new Exception("Esperava por requisição \" Requisitando\", porém achou \" "+ requisição +" \" !");
            }
            this.iPConectando = IPQuemMandou;
	        throw new Exception("B");
        }
        public async void receber()
        {
            try
            {
                broadcasting = (servidorBroadcast.ReceiveAsync());
		        inicializarTimer();
            }
            catch (Exception ex)//ObjectDisposedException pode ser tbm
            {
		            switch(estadoBroadcasting()){
                	    case TaskStatus.Faulted:
				            this.finalizarBroadcasting();
				            throw new Exception("Caso aparentemente impossível!");
			            break;
			            case TaskStatus.RanToCompletion:
				            //deu tudo certo!!!
				            //não dar dispose!!
				            //tratarBroadcast!!
				            throw new Exception("A");
			            break;
			            case TaskStatus.WaitingForChildrenToComplete:
				            this.finalizarBroadcasting();
				            throw new Exception("Comportamento inesperado da Task");
			            break;
			            case TaskStatus.Canceled:
				
				            this.finalizarBroadcasting();
				            throw new Exception("Task mal inicializada!");
			            break;
			            default:
				            this.finalizarBroadcasting();
				            throw new Exception("Comportamento insesperado do Timer");
				
		            }

            }
        }
        public TaskStatus estadoBroadcasting()
        {
            return broadcasting.Status;
        }
        public void responderPeers() {
            var infoResp = Encoding.ASCII.GetBytes("Respondendo");
            //Não sei se vou usar Send ou SendAsync

        }
        public void inicializarBroadcasting()
        {
            servidorBroadcast = new UdpClient(new IPEndPoint(IPAddress.Any, portaBroadcast));
            servidorBroadcast.EnableBroadcast = true;//pode enviar e/ou receber broadcast
            servidorBroadcast.MulticastLoopback = true;
            //uma mensagem será enviada para o dispositivo que fez um multicast

        }
        public void finalizarBroadcasting()
        {
            try
            {
                if(broadcasting!=null)
                    broadcasting.Dispose();
                if (servidorBroadcast != null)
                {
                    servidorBroadcast.Close();
                    servidorBroadcast.Dispose();
                }
		        if(timer != null)
			        timer.Dispose();
                 }
            catch {}
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
    class ChecadorStatus
    {
        //Task<UdpReceiveResult> tarefaAnalisar;
        public ChecadorStatus(/*Task<UdpReceiveResult> tarefa*/)
        {
          //  this.tarefaAnalisar = tarefa;
        }
        public void checarStatus(Object infoStatus)
        {
            AutoResetEvent autoEvento = (AutoResetEvent)infoStatus;
            if (ehParaParar(Peer.broadcasting.Status))
            {
                autoEvento.Set();
            }

        }
        private bool ehParaParar(TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.Canceled:
                    return true;
                    break;
                case TaskStatus.Faulted:
                    return true;
                    break;
                case TaskStatus.RanToCompletion:
		    
                    return true;
                    break;
                case TaskStatus.WaitingForChildrenToComplete:
                    return true;
                    break;
                default:
                    return false;
            }
        }
    }
}
