public class Controlador(){
	private ConexaoP2P conexao;
	private Peer peerAchado;
	private byte[] buffer;
	public Controlador(){
		this.conexao = new ConexaoP2P(InetAddress.getLocalHost().getHostAddress());
	}
	public void inicializarBroascasting(){
		this.conexao.inicializarBroadcasting();
	}
	public void enviar(){
		this.conexao.enviar();
	}
	public static void depoisEnviar(){
		//enviado!!
		//dentro daqui vc chamar� um m�todo que acontecer� depois de	enviar dados
		MainTeste.depoisEnviar();
	}
	public void finalizarBroascasting(){
		this.conexao.finalizarBroadcasting();	
	}
	public void inicializarEscuta(){
		this.conexao.inicializarEscuta();
	}
	public void receber(){
		this.conexao.receber();
	}
	public static void depoisEscuta(){
		this.peerAchado = this.conexao.getUltimoPeer();
		//dentro daqui vc chamar� um m�todo que acontecer� depois de receber dados
		MainTeste.depoisEscuta();
	}
	public void finalizarEscuta(){
		this.conexao.finalizarEscuta();
	}
	public Peer getPeerAchado(){
		return this.peerAchado;
	}
	
}