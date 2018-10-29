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
			System.out.println("Placa de rede do próprio dispositivo desconhecida");
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
	public Peer getPeerAchado(){
		return this.peerAchado;
	}
	public void inicializarTCP(){
        byte[] dados = new byte[1];
        dados[0] = 0xFF;//flag para parar
        //a simulação
		this.conexao.inicializarPeer(dados);
	}
    public void inicializarTCP(Desenhavel[] infos){
        int bytesDesenhavel = 41;
        byte[] dados = new byte[infos.length * bytesDesenhavel];
        for(int i=0;i<infos.length;i++){
            byte[] paraBytes = infos[i].converteBytes();
            for(int j=0;j<paraBytes.length;j++){
                dados[i+j] = paraBytes[j];
            }
        }
        this.conexao.inicializarPeer(dados);
    }
	public void enviarTCP(){
        
		this.conexao.enviarTCP();
	}
	public static void depoisEnviarTCP(){
		Main.depoisEnviarTCP();
	}

}