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
        private enum Status
        {

            peerDesconectado,
            peerDisponivel,
            peerRequisitandoConexao,
            peerConectado
        }
        const int portaBroadcast = 1729;
        const double intervaloSinal = 1000;
        
        UdpClient servidorBroadcast;
        String nome;
        bool conectadoBroadcast;
        public IPEndPoint tratarBroadcast() {
            IPEndPoint IPQuemMandou = new IPEndPoint(IPAddress.Any,portaBroadcast);
            byte[] datagrama = servidorBroadcast.Receive(ref IPQuemMandou);
            string requisição = Encoding.ASCII.GetString(datagrama);
            if(requisição.Equals("Requisitando"))
            {

            }
            else
            {
                //throw new SocketException("Ataque vírus!!!");
                throw new SocketException("Esperava por requisição \" Requisitando\", porém achou \" "+requisição +" \" !");
            }
            return IPQuemMandou;
        }
        public void responderPeers() {
            var infoResp = Encoding.ASCII.GetBytes("Respondendo");
        }
        
        public void finalizar()
        {

        }
        private IPAddress IP;
        public Peer(IPAddress meuIP) {
            if (meuIP != null)
                this.IP = meuIP;
            else
                throw new ArgumentNullException("IP nulo");
            servidorBroadcast = new UdpClient(new IPEndPoint(IPAddress.Any, portaBroadcast));
            servidorBroadcast.EnableBroadcast = true;//pode enviar e/ou receber broadcast
            servidorBroadcast.MulticastLoopback = true;
            //uma mensagem será enviada para o dispositivo que fez um multicast

        }

    }
}
