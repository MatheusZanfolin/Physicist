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
        Task<UdpReceiveResult> broadcasting;
        const int portaBroadcast = 1729;
        const double intervaloSinal = 1000;
        
        UdpClient servidorBroadcast;
        String nome;
        bool conectadoBroadcast;
        
        public IPEndPoint tratarBroadcast() {
            IPEndPoint IPQuemMandou;
            UdpReceiveResult req;
            //CancellationToken token = new CancellationToken(true);

            //receber();
            //byte[] datagrama = servidorBroadcast.ReceiveAsync(/*ref IPQuemMandou*/);
            req = broadcasting.Result;
            string requisição = Encoding.ASCII.GetString(req.Buffer);
            IPQuemMandou = req.RemoteEndPoint;
            if (!requisição.Equals("Requisitando"))
            {
                //throw new SocketException("Ataque vírus!!!");
                throw new Exception("Esperava por requisição \" Requisitando\", porém achou \" "+ requisição +" \" !");
            }
            return IPQuemMandou;
        }
        public async void receber()
        {
            try
            {
                broadcasting = (servidorBroadcast.ReceiveAsync());
            }
            catch (ObjectDisposedException obDispEx)
            {
                broadcasting.Dispose();
            }
        }
        public TaskStatus estadoBroadcasting()
        {
            return broadcasting.Status;
        }
        public void responderPeers() {
            var infoResp = Encoding.ASCII.GetBytes("Respondendo");
        }
        
        public void finalizarBroadcasting()
        {
            try
            {
                broadcasting.Dispose();
                servidorBroadcast.Close();
                servidorBroadcast.Dispose();
            }
            catch {}
        }
        private IPAddress IP;
        public Peer(IPEndPoint ipRemoto)
        {
            if (ipRemoto == null)
                throw new ArgumentNullException("IP remoto nulo");
            this.IP = ipRemoto.Address;
        }
        public Peer(IPAddress meuIP) {
            if (meuIP != null)
                this.IP = meuIP;
            else
                throw new ArgumentNullException("IP local nulo");
            servidorBroadcast = new UdpClient(new IPEndPoint(IPAddress.Any, portaBroadcast));
            servidorBroadcast.EnableBroadcast = true;//pode enviar e/ou receber broadcast
            servidorBroadcast.MulticastLoopback = true;
            //uma mensagem será enviada para o dispositivo que fez um multicast

        }

    }
}
