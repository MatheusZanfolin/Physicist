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
        private List<Peer> peers;
        public const int portaP2P = 1337;
        TcpListener receptorP2P;
        TcpClient emissorP2P;
        bool conectadoP2P;
        IPAddress meuIP;
        public async void testarBroadcasting() {
            	try{
			peers[0].receber();
        	}
		catch(Exception ex){
			if(ex.Message == "A")
				throw new Exception("A");
			else
				throw new Exception("Broadcasting falhou!");	
		}
	}
	public Peer ultimoPeer(){
		if(this.peers.Count != 0)
			return this.peers.Last;
		else
			throw new Exception("Não foi achado nenhum outro peer ainda!");
	}
        public void finalizarConexao() {

        }
        public void finalizarBroadcasting()
        {
            this.peers[0].finalizarBroadcasting();
            this.receptorP2P.Stop();
            
        }
        public async Peer tratarBroadcast()
        {
	    this.peers[0].tratarBroadcast()
            IPEndPoint ipAchado = this.peers[0].IPConectando;
            Peer novoPeer = new Peer(ipAchado);
            peers.Add(novoPeer);
        } 
        public TaskStatus estadoBroadcasting()
        {
            return this.peers[0].estadoBroadcasting();
        }
        public ConexaoP2P(IPAddress meuIP)
        {
            if (meuIP == null)
                throw new ArgumentNullException("ConexãoP2P: IP nulo");
            this.meuIP = meuIP;
            this.peers = new List<Peer>();
            this.peers.Insert(0, new Peer(meuIP));
            this.receptorP2P = new TcpListener(IPAddress.Any, ConexaoP2P.portaP2P);
            this.conectadoP2P = false;

        }
        public int acharIndicePeer(String ip)
        {

            IPAddress esseIp = new IPEndPoint()
        }
    }
}
