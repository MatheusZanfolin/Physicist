import java.net.*;
public class Controlador{
	private static ConexaoP2P conexao;
	private static PeerTeste peerAchado;
	private byte[] buffer;
	public Controlador(){
		this.conexao = new ConexaoP2P(InetAddress.getByName(InetAddress.getLocalHost().getHostAddress()));
	}
	public void inicializarBroadcasting(){
		this.conexao.inicializarBroadcasting();
	}
	public void enviar(){
		this.conexao.enviar();
	}
	public static void depoisEnviar(){
		//enviado!!
		//dentro daqui vc chamará um método que acontecerá depois de	enviar dados
		Main.depoisEnviar();
	}
	public void finalizarBroadcasting(){
		this.conexao.finalizarBroadcasting();
	}
	public void inicializarEscuta(){
		this.conexao.inicializarEscuta();
	}
	public void receber(){
		this.conexao.receber();
	}
	public static void depoisEscuta(){
		peerAchado = conexao.getUltimoPeer();
		//dentro daqui vc chamará um método que acontecerá depois de receber dados
		Main.depoisEscuta();
	}
	public void finalizarEscuta(){
		this.conexao.finalizarEscuta();
	}
	public PeerTeste getPeerAchado(){
		return this.peerAchado;
	}

}