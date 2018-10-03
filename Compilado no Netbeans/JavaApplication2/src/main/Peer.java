import java.net.*;
import java.util.*;
import java.lang.Thread.State;
public class Peer{
	//private MulticastSocket emissor;
	private static Timer temporizador;
	private static final int portaAppBroadcasting = 1729;
	private static final int portaDesktopBroadcasting = 1639;
	private static final String msgReq = "Requisitando";
	private static final String msgResp = "Respondendo";
	private static final String endGrupo = "236.0.0.0";
	private static MulticastSocket mcastEnviar;
	private static MulticastSocket mcastReceber;
	private InetAddress meuIP;
	private static InetAddress endBroadcast;
	private DatagramPacket pacoteEnviar;
	private DatagramPacket pacoteReceber;
	private static final byte[] msgReqBytes = msgReq.getBytes(/*Charset.forName("UTF-8")*/);
	private static final int ttlEnviar = 20;//passar por 20 dispostivos no máximo
	private static final int ttlReceber = 20;//passar por 20 dispositivos no máximo
	private static int ttlPadrao;
	public static ReceberUDPThread escuta;
	public static EnviarUDPThread broadcasting;
	private static ChecadorStatus checador;
	private static InetAddress iPConectando;
	public InetAddress getIPConectando(){
		return iPConectando;
	}
	public InetAddress getIP(){
		return meuIP;
	}
	/*
	alguns métodos necessários
	inicializarTimer
	inicializarBroadcasting
	finalizarBroadcasting
	enviar
	estadoBroadcasting
	construtor
	*/
	/*
https://docs.oracle.com/javase/9/docs/api/java/lang/doc-files/threadPrimitiveDeprecation.html*/
	public Peer(InetAddress ip)throws Exception{
		if(ip != null)
			meuIP = ip;
		else
			throw new Exception("IP local nulo");

		endBroadcast = InetAddress.getByName(endGrupo);
		//constrói o socket com os dados de quem envia
		//nesse caso, ele envia por uma porta aleatória
		//pois não pus nada no parâmetro
		mcastEnviar = new MulticastSocket();

		mcastReceber = new MulticastSocket(Peer.portaAppBroadcasting);

		//monta o pacote com os dados de quem irá receber
		pacoteEnviar = new DatagramPacket(
		Peer.msgReqBytes,Peer.msgReqBytes.length, endBroadcast, Peer.portaDesktopBroadcasting);
		byte[] buffer = new byte[1024];
		pacoteReceber = new DatagramPacket(buffer, buffer.length);

	}
	public void inicializarBroadcasting(){
		//endereço do grupo multicast(quem espera a requisição)
		//escolher um endereço aleatório de 225.0.0.0 a 238.255.255.255
		/*Endereços multicast de link-local começam com 224.0.0
		(ou seja, os endereços de 224.0.0.0 a 224.0.0.255)
		e estão reservados para protocolos de roteamento
		e outras atividades de baixo nível,
		como descoberta de gateway
		e relatórios de participação em grupo*/
		//Roteadores multicast nunca encaminham
		//datagramas com destinos nesse intervalo.
		try{
			mcastEnviar.joinGroup(endBroadcast);
			Peer.ttlPadrao= mcastEnviar.getTimeToLive();
			mcastEnviar.setTimeToLive(Peer.ttlEnviar);
		}
		catch(Exception ex){
			System.out.println("Erro ao conectar no grupo para enviar");
		}
		broadcasting = new EnviarUDPThread(
		"broadcasting",pacoteEnviar, mcastEnviar);

		temporizador = new Timer("broadcasting", false);
		//não é uma task daemon, por isso é false
		checador = new ChecadorStatus(broadcasting);


	}
	public void enviar(){
		/*//por enquanto vou deixar síncrono
		mcastEnviar.send(pacoteBroadcast);*/
		broadcasting.start();
		temporizador.schedule(checador, 0, 1000);
		//      		    inicial delay periodo
	}
	public static void depoisEnviar()throws Exception{
	//aqui temos certeza que a thread de enviar acabou
		State comoAcabou = checador.getStatus();
		System.out.println(new String(broadcasting.getDados()));
		try{
			if(ttlPadrao>0 && ttlPadrao<255)
				mcastEnviar.setTimeToLive(Peer.ttlPadrao);
		}
		catch(Exception ex){
				System.out.println("Erro no ttl: " + ex.getMessage() );
				ex.printStackTrace();
		}

		if(!ChecadorStatus.ehParaParar(comoAcabou)){

			finalizarBroadcasting();
			throw new Exception("Conexão com o dispositivo mal estabelecida! Verifique se estão na mesma rede e tente novamente!");
		}
		else{
			Peer.finalizarBroadcasting();

			//deu certo!
			ConexaoP2P.depoisEnviar();
		}
	}
	public static void finalizarBroadcasting(){
		try{
			if(mcastEnviar != null){
				mcastEnviar.leaveGroup(endBroadcast);
				mcastEnviar.close();
				mcastEnviar.disconnect();
				mcastEnviar = null;
				mcastEnviar = new MulticastSocket();

			}

			if(temporizador!=null){
				temporizador.purge();
				temporizador.cancel();
				temporizador = null;

			}
			checador=null;
			broadcasting=null;
		}
		catch(Exception ex){}
	}
	public void inicializarEscuta(){
		//endereço do grupo multicast(quem espera a requisição)
		//escolher um endereço aleatório de 225.0.0.0 a 238.255.255.255
		/*Endereços multicast de link-local começam com 224.0.0
		(ou seja, os endereços de 224.0.0.0 a 224.0.0.255)
		e estão reservados para protocolos de roteamento
		e outras atividades de baixo nível,
		como descoberta de gateway
		e relatórios de participação em grupo*/
		//Roteadores multicast nunca encaminham
		//datagramas com destinos nesse intervalo.
		//monta o pacote com os dados de quem irá receber
		//constrói o socket com os dados de quem envia
		//nesse caso, ele envia por uma porta aleatória
		//pois não pus nada no parâmetro
		try{
			mcastReceber.joinGroup(endBroadcast);
			ttlPadrao= mcastReceber.getTimeToLive();
			mcastReceber.setTimeToLive(Peer.ttlReceber);
		}
		catch(Exception ex){
			System.out.println("Erro ao conectar no grupo para receber" + ex.getMessage());
		}
		escuta = new ReceberUDPThread(
		"escutaUDP",pacoteReceber, mcastReceber);

		temporizador = new Timer("escutaUDP", false);
		checador = null;
		checador = new ChecadorStatus(escuta);
		/*
		int ttl = mcastSocket.getTimeToLive(); mcastSocket.setTimeToLive(newttl); mcastSocket.send(p); mcastSocket.setTimeToLive(ttl);
		*/
	}
	public void receber(){
		//código rodando assincronamente
		escuta.start();
		temporizador.schedule(checador, 0, 1000);
	}
		public static boolean ehParaParar(Thread.State estadoAtual){
			//status = estadoAtual;
			switch(estadoAtual){
				case NEW:
				//A thread não começou ainda
					return false;
				case RUNNABLE:
				//se está executando nesse momento na máquina virtual Java
					return false;
				case BLOCKED:
				//está bloqueada esperando por um "monitor lock "
					return false;
				case WAITING:
				//esperando indefinitivamente por outra thread que está fazendo uma ação determinada
					return false;
				case TIMED_WAITING:
				//esperando outra thread fazer uma
				//"action for up to a specified waiting time"
					return false;
				case TERMINATED:
				//morreu
					return true;
				default:
					return true;
			}
	}
	public static void depoisEscuta()throws Exception{
		State comoAcabou = checador.getStatus();
		try{
			if(ttlPadrao>0 && ttlPadrao<255 && mcastEnviar!=null)
				mcastReceber.setTimeToLive(ttlPadrao);
		}
		catch(Exception ex){
				System.out.println("Erro no ttl: " + ex.getMessage());

				ex.printStackTrace();
		}
		if(!ehParaParar(comoAcabou)){

			finalizarEscuta();
			throw new Exception("Conexão com o dispositivo mal estabelecida! Verifique se estão na mesma rede e tente novamente!");
		}
		else{
			//deu certo!
			String buffer = new String(escuta.getDados());
			buffer = buffer.trim();
			InetAddress ipQuemEnviou = escuta.getIPQuemEnviou();
			//byte[] requerido = Peer.msgResp.getBytes(/*Charset.forName("UTF-8")*/);
			boolean iguais = true;
			if(buffer.length() == Peer.msgResp.length()){
				for(int i=0;i<buffer.length();i++){
					if(buffer.charAt(i) != Peer.msgResp.charAt(i))
						iguais = false;
				}
			}
			if(buffer.length()!=Peer.msgResp.length() || !iguais){
				//ataque vírus!!!
				throw new Exception("Ataque vírus!!");
			}
			else{
				//InetAddress ipQuemEnviou = escuta.getIPQuemEnviou();
				iPConectando = ipQuemEnviou;
				Peer.finalizarEscuta();
				ConexaoP2P.depoisEscuta();
			}


		}
	}
	public static void finalizarEscuta(){
		try{
			if(mcastReceber != null){
				mcastReceber.leaveGroup(endBroadcast);
				mcastReceber.close();
				mcastReceber.disconnect();
				mcastReceber = null;
				mcastReceber = new MulticastSocket(Peer.portaAppBroadcasting);
			}


			if(temporizador!=null){
				temporizador.purge();
				temporizador.cancel();
				temporizador = null;

			}
			checador=null;
			escuta=null;
		}
		catch(Exception ex){}
	}
/*MulticastSocket ms = new MulticastSocket(2300);
InetAddress group = InetAddress.getByName("224.2.2.2");
ms.joinGroup(group);
byte[] buffer = new byte[8192];
DatagramPacket dp = new DatagramPacket(buffer, buffer.length);
ms.receive(dp);
ms.leaveGroup(group);
ms.close(););*/


}