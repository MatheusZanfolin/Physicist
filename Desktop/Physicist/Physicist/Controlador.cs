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
    class Controlador
    {
        ConexaoP2P conexao;
        Timer timer = new Timer()
        public enum Status
        {
            Desconectado,
            EsperandoRequisicao,
            Escolhendo,
            EsperandoResposta,
            Conectado
        }
        public Controlador() {
            conexao = new ConexaoP2P(getIpLocal());
           
        }
        public void finalizarConexao()
        {
            
        }
        public void finalizarBroadcasting()
        {
            conexao.finalizarBroadcasting();
        }
        public async void testarBroadcasting()
        {
            conexao.testarBroadcasting();
        }
        public void tratarBroadcast()
        {
            //cuidado, lança exceção
            conexao.tratarBroadcast();
            
            

            
        }
        private IPAddress getIpLocal()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("Não há adaptadores de rede com um endereço IPv4 compatível no sistema");
        }
    }
}
