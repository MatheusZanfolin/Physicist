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
        private void tratarBroadcast() { }
        private void responderPeers() { }
        public void iniciar() {
            servidorBroadcast = new UdpClient((AddressFamily)IP);
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
        }
        
    }
}
