public class Peer{
	private MulticastSocket emissor;

	//private static final int portaAppBroadcasting = 1729;
	private static final int portaDesktopBroadcasting = 1639;
	private static final String msgReq = "Requisitando";
	private static final String msgResp = "Respondendo";
	private static final String endGrupo = "236.0.0.0";
	private MulticastSocket mcastSocket;
	private InetAddress meuIP;
	private InetAddress endBroadcast;
	private DatagramPackage pacoteBroadcast;
	private static final byte[] msgReqBytes = msgReq.getBytes(Charset.forName("UTF-8"));
	/*
	alguns m�todos necess�rios
	inicializarTimer
	inicializarBroadcasting
	finalizarBroadcasting
	enviar
	estadoBroadcasting
	construtor
	*/
	public Peer(InetAddress meuIP)throws Exception{
		if(meuIP != null)
			this.meuIP = meuIP;
		else
			throw new Exception("IP local nulo");
	}
	public inicializarBroadcasting(){
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
		this.endBroadcast = InetAddress.getByName(endGrupo);
		//monta o pacote com os dados de quem ir� receber
		this.pacoteBroadcast = new DatagramPacket(
		Peer.msgReqBytes,Peer.msgReqBytes.length, endBroadcast, Peer.portaAppBroadcasting);
		//constr�i o socket com os dados de quem envia
		//nesse caso, ele envia por uma porta aleat�ria
		//pois n�o pus nada no par�metro
		mcastSocket = new MulticastSocket();
		mcastSocket.joinGroup(endBroadcast);
	}
	public void enviar(){
		//por enquanto vou deixzar s�ncrono
		mcastSocket.send(pacoteBroadcast);
	}
	public finalizarBroadcasting(){
		try{
			if(mcastSocket != null){
				this.mcastSocket.leaveGroup(endBroadcast);

			}
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