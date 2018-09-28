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
	alguns métodos necessários
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
		this.endBroadcast = InetAddress.getByName(endGrupo);
		//monta o pacote com os dados de quem irá receber
		this.pacoteBroadcast = new DatagramPacket(
		Peer.msgReqBytes,Peer.msgReqBytes.length, endBroadcast, Peer.portaAppBroadcasting);
		//constrói o socket com os dados de quem envia
		//nesse caso, ele envia por uma porta aleatória
		//pois não pus nada no parâmetro
		mcastSocket = new MulticastSocket();
		mcastSocket.joinGroup(endBroadcast);
	}
	public void enviar(){
		//por enquanto vou deixzar síncrono
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