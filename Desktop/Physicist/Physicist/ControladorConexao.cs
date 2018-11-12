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
    class ControladorConexao
    {
        ConexaoP2P conexao;
        byte[] buffer;
        public byte[] Buffer
        {
            get
            {
                return this.buffer;
            }
        }
        private static Peer peerAchado;
        //tempo máximo = 2min 30 seg
        /*public enum Status
        {
            Desconectado,
            EsperandoRequisicao,
            Escolhendo,
            EsperandoResposta,
            Conectado
        }*/
        public ControladorConexao() {
            conexao = new ConexaoP2P(getIpLocal());
        }
        
        public void finalizarConexao()
        {

        }
        public void finalizarMulticasting()
        {
            conexao.finalizarMulticasting();
        }
        public void inicializarMulticasting()
        {
            conexao.inicializarMulticasting();
        }
        public void finalizarRespostaMulticasting()
        {
            conexao.finalizarRespostaMulticasting();
        }
        public void inicializarRespostaMulticasting(int indice)
        {
            conexao.inicializarRespostaMulticasting(indice);
        }
        public async void testarMulticasting()
        {
                conexao.testarMulticasting();
        	
	    }
        public async void responderMulticasting()
        {
            conexao.responderMulticasting();                              
        }
        public static Peer PeerAchado {
            get{
                return peerAchado;
            }
        }
        public static void tratarMulticast() {
            //conexao.tratarMulticast();
            peerAchado = ConexaoP2P.ultimoPeer();
            frmPrincipal.novoPeer = true;
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