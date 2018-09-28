public class ReceberUDPThread extends Thread{
	private MulticastSocket mcastSocket;
	private DatagramPacket pacote;
	public ReceberUDP(String nomeThread, DatagramPacket pacote, MulticastSocket mcastSocket){
		super(nomeThread);
		this.pacote = pacote;
		this.mcastSocket = mcastSocket;
	}
	public void run(){
		this.mcastSocket.receive(this.pacoteBroadcast);
	}
	public DatagramPacket getDados(){
		return this.pacote;
	}
}