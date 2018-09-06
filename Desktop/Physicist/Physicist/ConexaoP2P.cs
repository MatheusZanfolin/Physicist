using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Physicist
{
    class ConexaoP2P
    {
        private Peer[] peers;
        public const int portaP2P = 1337;
        TcpListener receptorP2P;
        TcpClient emissorP2P;
        bool conectadoP2P;
        IPAddress meuIP;
        public void iniciarConexao() {

        }
        public void finalizarConexao() {
        }
        public ConexaoP2P(IPAddress meuIP)
        {
            if (meuIP == null)
                throw new ArgumentNullException("ConexãoP2P: IP nulo");
            this.meuIP = meuIP;
            this.peers = new Peer[2];
            this.peers[0] = new Peer(meuIP);
            this.receptorP2P = new TcpListener(IPAddress.Any, ConexaoP2P.portaP2P);
            this.conectadoP2P = false;

        }
    }
}
