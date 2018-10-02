public class ConexaoP2P{
	private ArrayList<Peer> listaPeers;
	const int portaP2PDesktop = 1337;
	private InetAddress ipLocal;
	public ConexaoP2P(InetAddress ipLocal){
		this.ipLocal = ipLocal;
		this.listaPeers = new ArrayList<Peer>();
		this.listaPeers.add(new Peer(this.ipLocal));
	}
	public void inicializarBroadcasting(){
		this.listaPeers[0].inicializarBroadcasting();
	}
	public void enviar(){
		this.listaPeers[0].enviar();
	}
	public static void depoisEnviar(){
		Controlador.depoisEnviar();
	}
	public void finalizarBroadcasting(){
		this.listaPeers[0].finalizarBroadcasting();
	}
	public void inicializarEscuta(){
		this.listaPeers[0].inicializarEscuta();
	}
	public void receber(){
		this.listaPeers[0].receber();
	}
	public static void depoisEscuta(){
		//aqui temos certeza que a thread
		//de escutar acabou
		InetAddress IPQuemMandouMsg = this.listaPeers[0].getIPConectando();
		this.listaPeers.add(new Peer(IPQuemMandouMsg));
		Controlador.depoisEscuta();
	}
	public void finalizarEscuta(){
		this.listaPeers[0].finalizarEscuta();
	}
	public Peer getUltimoPeer(){
		return this.listaPeers[this.listaPeers.size()-1];
	}
}