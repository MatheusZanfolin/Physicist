import java.net.*;
import java.util.*;
import java.lang.Thread.State;
public class PeerTeste{
	//private MulticastSocket emissor;
	private static Timer temporizador;
	private static final int portaDesktopBroadcasting = 1729;
	private static final int portaAppBroadcasting = 1639;
	private static final String msgReq = "Respondendo";
	private static final String msgResp = "Requisitando";
	private static final String endGrupo = "236.0.0.0";
	private static MulticastSocket mcastEnviar;
	private static MulticastSocket mcastReceber;
	private InetAddress meuIP;
	private static InetAddress endBroadcast;
	private DatagramPacket pacoteEnviar;
	private DatagramPacket pacoteReceber;
	private static final byte[] msgReqBytes = msgReq.getBytes(/*Charset.forName("UTF-8")*/);
	private static final int ttlEnviar = 20;//passar por 20 dispostivos no m�ximo
	private static final int ttlReceber = 20;//passar por 20 dispositivos no m�ximo
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
	alguns m�todos necess�rios
	inicializarTimer
	inicializarBroadcasting
	finalizarBroadcasting
	enviar
	estadoBroadcasting
	construtor
	*/
	/*
https://docs.oracle.com/javase/9/docs/api/java/lang/doc-files/threadPrimitiveDeprecation.html*/
	public PeerTeste(InetAddress ip)throws Exception{
		if(ip != null)
			meuIP = ip;
		else
			throw new Exception("IP local nulo");

		endBroadcast = InetAddress.getByName(endGrupo);
		//constr�i o socket com os dados de quem envia
		//nesse caso, ele envia por uma porta aleat�ria
		//pois n�o pus nada no par�metro
		mcastEnviar = new MulticastSocket();

		mcastReceber = new MulticastSocket(PeerTeste.portaAppBroadcasting);

		//monta o pacote com os dados de quem ir� receber
		pacoteEnviar = new DatagramPacket(
		PeerTeste.msgReqBytes,PeerTeste.msgReqBytes.length, endBroadcast, PeerTeste.portaDesktopBroadcasting);
		byte[] buffer = new byte[1024];
		pacoteReceber = new DatagramPacket(buffer, buffer.length);

	}
	public void inicializarBroadcasting(){
		//endere�o do grupo multicast(quem espera a requisi��o)
		//escolher um endere�o aleat�rio de 225.0.0.0 a 238.255.255.255
		/*Endere�os multicast de link-local come�am com 224.0.0
		(ou seja, os endere�os de 224.0.0.0 a 224.0.0.255)
		e est�o reservados para protocolos de roteamento
		e outras atividades de baixo n�vel,
		como descoberta de gateway
		e relat�rios de participa��o em grupo*/
		//Roteadores multicast nunca encaminham
		//datagramas com destinos nesse intervalo.
		try{
			mcastEnviar.joinGroup(endBroadcast);
			PeerTeste.ttlPadrao= mcastEnviar.getTimeToLive();
			mcastEnviar.setTimeToLive(PeerTeste.ttlEnviar);
		}
		catch(Exception ex){
			System.out.println("Erro ao conectar no grupo para enviar");
		}
		broadcasting = new EnviarUDPThread(
		"broadcasting",pacoteEnviar, mcastEnviar);

		temporizador = new Timer("broadcasting", false);
		//n�o � uma task daemon, por isso � false
		checador = new ChecadorStatus(broadcasting);


	}
	public void enviar(){
		/*//por enquanto vou deixar s�ncrono
		mcastEnviar.send(pacoteBroadcast);*/
		//System.out.println("Thread de enviar come�ando");
		//System.out.println(""+new String(this.pacoteEnviar.getData()));

		//System.out.println("IP: "+this.pacoteEnviar.getAddress());
		System.out.println("Peer 1: Redirecionando pacote pela porta " + this.pacoteEnviar.getPort());
		broadcasting.start();
		temporizador.schedule(checador, 0, 1000);
		//      		     			inicial delay periodo
	}
	public static void depoisEnviar()throws Exception{
	//aqui temos certeza que a thread de enviar acabou
		State comoAcabou = checador.getStatus();
		try{
			mcastEnviar.setTimeToLive(PeerTeste.ttlPadrao);
		}
		catch(Exception ex){
					System.out.println("Erro no ttl");
		}
		if(!ChecadorStatus.ehParaParar(comoAcabou)){

			finalizarBroadcasting();
			throw new Exception("Conex�o com o dispositivo mal estabelecida! Verifique se est�o na mesma rede e tente novamente!");
		}
		else{

			//deu certo!
			PeerTeste.finalizarBroadcasting();
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
		//endere�o do grupo multicast(quem espera a requisi��o)
		//escolher um endere�o aleat�rio de 225.0.0.0 a 238.255.255.255
		/*Endere�os multicast de link-local come�am com 224.0.0
		(ou seja, os endere�os de 224.0.0.0 a 224.0.0.255)
		e est�o reservados para protocolos de roteamento
		e outras atividades de baixo n�vel,
		como descoberta de gateway
		e relat�rios de participa��o em grupo*/
		//Roteadores multicast nunca encaminham
		//datagramas com destinos nesse intervalo.
		//monta o pacote com os dados de quem ir� receber
		//constr�i o socket com os dados de quem envia
		//nesse caso, ele envia por uma porta aleat�ria
		//pois n�o pus nada no par�metro
		try{
			mcastReceber.joinGroup(endBroadcast);
			PeerTeste.ttlPadrao= mcastReceber.getTimeToLive();
			mcastReceber.setTimeToLive(PeerTeste.ttlReceber);
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
		//c�digo rodando assincronamente
		escuta.start();
		temporizador.schedule(checador, 0, 1000);
	}
	public static boolean ehParaParar(Thread.State estadoAtual){
			switch(estadoAtual){
				case NEW:
				//A thread n�o come�ou ainda
					return false;
				case RUNNABLE:
				//se est� executando nesse momento na m�quina virtual Java
					return false;
				case BLOCKED:
				//est� bloqueada esperando por um "monitor lock "
					return false;
				case WAITING:
				//esperando indefinitivamente por outra thread que est� fazendo uma a��o determinada
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
		//System.out.println(comoAcabou.toString());
		try{
			mcastReceber.setTimeToLive(PeerTeste.ttlPadrao);
		}
		catch(Exception ex){
			System.out.println("Erro no ttl");
		}
		if(!ehParaParar(comoAcabou)){
			finalizarEscuta();
			throw new Exception("Conex�o com o dispositivo mal estabelecida! Verifique se est�o na mesma rede e tente novamente!");
		}
		else{
			//deu certo!
			String buffer = new String(escuta.getDados());
			buffer = buffer.trim();
			InetAddress ipQuemEnviou = escuta.getIPQuemEnviou();
			//byte[] requerido = PeerTeste.msgResp.getBytes(/*Charset.forName("UTF-8")*/);
			boolean iguais = true;
			if(buffer.length() == PeerTeste.msgResp.length()){
				for(int i=0;i<buffer.length();i++){
					if(buffer.charAt(i) != PeerTeste.msgResp.charAt(i))
						iguais = false;
				}
			}
			if(buffer.length()!=PeerTeste.msgResp.length() || !iguais){
				//ataque v�rus!!!
				System.out.println("Buffer recebido: "+ new String(buffer));
				System.out.println("O que era requerido: "+ new String(PeerTeste.msgResp));
				throw new Exception("Ataque v�rus!!");
			}
			else{

				iPConectando = ipQuemEnviou;
				PeerTeste.finalizarEscuta();
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