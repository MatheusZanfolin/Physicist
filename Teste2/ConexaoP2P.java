import java.util.*;
import java.net.*;
import java.lang.Thread.State;
public class ConexaoP2P{
	private static ArrayList<Peer> listaPeers;
	private final int portaP2PDesktop = 1337;
	private InetAddress ipLocal;
	private Socket emissorP2P;
	private DataOutputStream streamEmissao;
	private static EnviarTCPThread emissao;
	private static Timer temporizadorP2P;
	private static ChecadorStatus checador;
	public ConexaoP2P(InetAddress ipLocal){
		ipLocal = ipLocal;
		listaPeers = new ArrayList<Peer>();
		try{
			listaPeers.add(new Peer(ipLocal));
		}
		catch(Exception ex){}
	}
	public void inicializarBroadcasting(){
		listaPeers.get(0).inicializarBroadcasting();
	}
	public void enviar(){
		listaPeers.get(0).enviar();
	}
	public static void depoisEnviar(){
		Controlador.depoisEnviar();
	}
	public void finalizarBroadcasting(){
		listaPeers.get(0).finalizarBroadcasting();
	}
	public void inicializarEscuta(){
		listaPeers.get(0).inicializarEscuta();
	}
	public void receber(){
		listaPeers.get(0).receber();
	}
	public static void depoisEscuta(){
		//aqui temos certeza que a thread
		//de escutar acabou
		InetAddress IPQuemMandouMsg = listaPeers.get(0).getIPConectando();
		try{
			listaPeers.add(new Peer(IPQuemMandouMsg));
		}
		catch(Exception ex){}
		Controlador.depoisEscuta();
	}
	public void finalizarEscuta(){
		listaPeers.get(0).finalizarEscuta();
	}
	public static Peer getUltimoPeer(){
		return listaPeers.get(listaPeers.size()-1);
	}

	public static void inicializarPeer(){
		emissorP2P = new Socket(getUltimoPeer().getIP(), portaP2PDesktop);
		streamEmissao = new DataOutputStream(emissorP2P.getOutputStream());
		byte[] dados = new byte[1024];

		emissao = new EnviarTCPThread("emissao", , streamEmissao);
		temporizadorP2P = new Timer("emissao", false);
		checador = null;
		checador = new ChecadorStatus(emissao);
	}
	public void enviarTCP(){
		emissao.start();
		temporizadorP2P.schedule(checador, 0, 1000);
	}
	public static void depoisEnviarTCP(){
		State comoAcabou = checador.getStatus();
		if(!Peer.ehParaParar(comoAcabou)){
			finalizarEnvio();
			throw new Exception("Conexão com o dispositivo mal estabelecida! Verifique se estão na mesma rede e tente novamente!");

		}
		else{
		//deu certo!
			finalizarEnvio();
			Controlador.depoisEnviarTCP();
		}
	}
	public static finalizarEnvio(){
		if(emissorP2P != null){
			emissorP2P.close();
			emissorP2P = null;
		}
		if(streamEmissao != null){
			streamEmissao.close();
			streamEmissao = null;
		}
		if(temporizadorP2P != null){
			temporizadorP2P.purge();
			temporizadorP2P.cancel();
			temporizador = null;
		}
		checador = null;
		emissao = null;
	}
}