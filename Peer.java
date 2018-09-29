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
	public EnviarUDPThread broadcasting;
	private ChecadorStatus checador;
	private InetAddress iPConectando;
	public InetAddress getIPConectando(){
		return this.iPConectando;
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
		this.broadcasting = new EnviarUDPThread(
		"broadcasting",pacoteBroadcast, mcastSocket);

		this.temporizador = new Timer("broadcasting", false);
		//não é uma task daemon, por isso é false
		this.temporizador.schedule()
	}
	public void enviar(){
		/*//por enquanto vou deixar síncrono
		mcastSocket.send(pacoteBroadcast);*/
		this.broadcasting.start();
		this.temporizador.schedule(this.checador, 0, 1000);
	}
	public void depoisEnviar(){
		Thread.Status comoAcabou = this.checador.getStatus();
		if(comoAcabou != Thread.State.TERMINATED){
			
			this.finalizarBroadcasting();
			throw new Exception("Conexão com o dispositivo mal estabelecida! Verifique se estão na mesma rede e tente novamente!");
		}
		else{
			
			//deu certo!
		}		
	}
	public finalizarBroadcasting(){
		try{
			if(this.mcastSocket != null){
				this.mcastSocket.setTimeToLive(ttlPadrao);
				this.mcastSocket.leaveGroup(endBroadcast);
				this.mcastSocket.close();
				this.mcastSocket.disconnect();
				this.mcastSocket = null;
			}
			
			if(this.temporizador!=null){
				this.temporizador.purge();
				this.temporizador.cancel();
				this.temporizador = null;

			}
			this.checador=null;
			this.broadcasting=null;
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
		this.escuta = new ReceberUDPThread(
		"escutaUDP",pacoteBroadcast, mcastSocket);

		this.temporizador = new Timer("escutaUDP", false);
		this.checador = null;
		this.checador = new ChecadorStatus(this.escuta);
		/*
		int ttl = mcastSocket.getTimeToLive(); mcastSocket.setTimeToLive(newttl); mcastSocket.send(p); mcastSocket.setTimeToLive(ttl);
		*/
	}
	public void receber(){
		//código rodando assincronamente
		this.escuta.start();
		this.temporizador.schedule(this.checador, 0, 1000);
	}
	public void depoisEscuta(){
		Thread.Status comoAcabou = this.checador.getStatus();
		if(comoAcabou != Thread.State.TERMINATED){
			
			this.finalizarEscuta();
			throw new Exception("Conexão com o dispositivo mal estabelecida! Verifique se estão na mesma rede e tente novamente!");
		}
		else{
			//deu certo!
			byte[] buffer = this.escuta.getDados();
			InetAddress ipQuemEnviou = this.escuta.getIPQuemEnviou();
			byte[] requerido = this.Peer.msgResp.getBytes(Charset.forName("UTF-8"));
			boolean iguais = true;
			if(buffer.length == requerido.length){
				for(int i=0;i<buffer.length;i++){
					if(buffer[i] != requerido[i])
						iguais = false;
				}
			}
			if(buffer.length!=requerido.length || !iguais){
				//ataque vírus!!!
			}
			else{
				this.iPConectando = ipQuemEnviou;
			}


	
		}
	}
	public finalizarEscuta(){
		try{
			if(this.mcastSocket != null){
				this.mcastSocket.setTimeToLive(ttlPadrao);
				this.mcastSocket.leaveGroup(endBroadcast);
				this.mcastSocket.close();
				this.mcastSocket.disconnect();
				this.mcastSocket = null;
			}
			
			if(this.temporizador!=null){
				this.temporizador.purge();
				this.temporizador.cancel();
				this.temporizador = null;

			}
			this.checador=null;
			this.escuta=null;
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