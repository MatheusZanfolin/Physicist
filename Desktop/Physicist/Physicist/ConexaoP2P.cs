using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Physicist
{
    class ConexaoP2P
    {
        private Peer[] peers;
        const int portaP2P = 1337;
        TcpListener receptorP2P;
        TcpClient emissorP2P;
        bool conectadoP2P;
        private void iniciarConexao() { }
        private void finalizarConexao() { }
        public ConexaoP2P()
        {

        }
    }
}
