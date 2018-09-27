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
        
        Peer peerAchado;
        //tempo máximo = 2min 30 seg
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
        public void inicializarBroadcasting()
        {
            conexao.inicializarBroadcasting();
        }
        public void finalizarRespostaBroadcasting()
        {
            conexao.finalizarRespostaBroadcasting();
        }
        public void inicializarRespostaBroadcasting()
        {
            conexao.inicializarRespostaBroadcasting();
        }
        public async void testarBroadcasting()
        {
		    try{
                conexao.testarBroadcasting();
        	}
		    catch(Exception ex){
			    if(ex.Message == "A"){
				    try{
					    this.tratarBroadcast();
				    }
				    catch(Exception e){
					    if(e.Message == "B"){
						    Peer ultimo = this.conexao.ultimoPeer();
						    this.peerAchado = ultimo;
						    throw new Exception("B");
					    }
					    else{
						    throw new Exception("Caso esperado de ser impossível!");	
					    }
				    }
			    }
			    else{
				    throw new Exception("Busca por dispositivos não deu certo!");
			    }
		    }
	    }
        public async void responderBroadcasting()
        {
            try
            {
                conexao.responderBroadcasting();
            }
            catch (Exception ex)
            {
                if (ex.Message == "C")
                {
                    try
                    {
                        //ainda não programei, aqui significa que a resposta foi enviada 
                        // devo programar para esperar outra requisição com os dados
                    }
                    catch (Exception e)
                    {
                        if (e.Message == "D")
                        {
                            //aqui devo programar para tratar os dados recebidos
                        }
                        else
                        {
                            throw new Exception("Caso esperado de ser impossível!");
                        }
                    }
                }
                else
                {
                    throw new Exception("Mensagem não foi enviada pro dispositivo!");
                }
            }
        }
        public Peer PeerAchado {
            get{
                return this.peerAchado;
            }
        }
        private async void tratarBroadcast()
        {
            conexao.tratarBroadcast();
            //cuidado, lança exceção
            peerAchado = conexao.ultimoPeer();
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