public class ReceberUDPThread extends Thread{
	private MulticastSocket mcastSocket;
	private DatagramPacket pacote;
	public ReceberUDP(DatagramPacket pacote, MulticastSocket mcastSocket){
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