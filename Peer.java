public class Peer{
	private MulticastSocket emissor;
	private Timer temporizador;
	private static final int portaAppBroadcasting = 1729;
	private static final int portaDesktopBroadcasting = 1639;
	private static final String msgReq = "Requisitando";
	private static final String msgResp = "Respondendo";
	private static final String endGrupo = "236.0.0.0";
	private MulticastSocket mcastSocket;
	private InetAddress meuIP;
	private InetAddress endBroadcast;
	private DatagramPacket pacoteBroadcast;
	private static final byte[] msgReqBytes = msgReq.getBytes(Charset.forName("UTF-8"));
	private static final int ttlEnviar = 1000;//1 seg
	private static final int ttlReceber = 10000;//10 seg
	private int ttlPadrao;
	public ReceberUDPThread escuta;
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
		this.mcastSocket = new MulticastSocket();
		this.mcastSocket.joinGroup(endBroadcast);
		this.ttlPadrao= mcastSocket.getTimeToLive();
		this.mcastSocket.setTimeToLive(Peer.ttlEnviar);
	}
	public void enviar(){
		//por enquanto vou deixar síncrono
		mcastSocket.send(pacoteBroadcast);
	}
	public finalizarBroadcasting(){
		try{
			if(mcastSocket != null){
				this.mcastSocket.setTimeToLive(ttlPadrao);
				this.mcastSocket.leaveGroup(endBroadcast);
				this.mcastSocket.close();
				this.mcastSocket.disconnect();
				this.mcastSocket = null;
			}
		}
		catch(Exception ex){}
	}
	public inicializarEscuta(){
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
		byte[] buffer = new byte[1024];
		this.pacoteBroadcast = new DatagramPacket(buffer, buffer.length);
		//constrói o socket com os dados de quem envia
		//nesse caso, ele envia por uma porta aleatória
		//pois não pus nada no parâmetro
		mcastSocket = new MulticastSocket(portaAppBroadcasting);
		mcastSocket.joinGroup(endBroadcast);
		this.ttlPadrao= mcastSocket.getTimeToLive();
		mcastSocket.setTimeToLive(Peer.ttlReceber);
		escuta = new ReceberUDPThread(
		"escutaUDP",pacoteBroadcast, mcastSocket);
		/*
		int ttl = mcastSocket.getTimeToLive(); mcastSocket.setTimeToLive(newttl); mcastSocket.send(p); mcastSocket.setTimeToLive(ttl);
		*/
	}
	public void receber(){
		//código rodando assincronamente
		escuta.run();
		this.temporizador = new Timer("escutaUDP", false);
	}
	public void timerFinalizou
	public finalizarEscuta(){
		try{
			if(mcastSocket != null){
				mcastSocket.setTimeToLive(ttlPadrao);
				this.mcastSocket.leaveGroup(endBroadcast);
				this.mcastSocket.close();
				this.mcastSocket.disconnect();
				this.mcastSocket = null;
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