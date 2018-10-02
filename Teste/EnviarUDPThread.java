public class EnviarUDPThread extends Thread{
	private MulticastSocket mcastSocket;
	private DatagramPacket pacote;
//	private Thread blinker;
//https://docs.oracle.com/javase/9/docs/api/java/lang/doc-files/threadPrimitiveDeprecation.html
	public ReceberUDP(String nomeThread, DatagramPacket pacote, MulticastSocket mcastSocket){
		super(nomeThread);
		this.pacote = pacote;
		this.mcastSocket = mcastSocket;
	}
	/*public void start(){
		blinker = new Thread(this);
		blinker.start();
	}*/
	public void run(){
		this.mcastSocket.send(this.pacoteBroadcast);
	}
	
}