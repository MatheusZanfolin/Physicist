import java.net.*;
public class Controlador{
	private static ConexaoP2P conexao;
	private static Peer peerAchado;
	private byte[] buffer;
	public Controlador(){
		try{
			this.conexao = new ConexaoP2P(InetAddress.getByName(InetAddress.getLocalHost().getHostAddress()));
		}
		catch(Exception ex){
			System.out.println("Placa de rede do pr�prio dispositivo desconhecida");
		}
	}
	public void inicializarBroadcasting(){
		this.conexao.inicializarBroadcasting();
	}
	public void enviar(){
		this.conexao.enviar();
	}
	public static void depoisEnviar(){
		//enviado!!
		//dentro daqui vc chamar� um m�todo que acontecer� depois de	enviar dados
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
		//dentro daqui vc chamar� um m�todo que acontecer� depois de receber dados
		Main.depoisEscuta();
	}
	public void finalizarEscuta(){
		this.conexao.finalizarEscuta();
	}
	public Peer getPeerAchado(){
		return this.peerAchado;
	}
	public void inicializarTCP(){
		this.conexao.inicializarPeer();
	}
	public void enviarTCP(){
		this.conexao.enviarTCP();
	}
	public static void depoisEnviarTCP(){
		Main.depoisEnviarTCP();
	}

}