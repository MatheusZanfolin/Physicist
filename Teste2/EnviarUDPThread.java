import java.net.*;
public class EnviarUDPThread extends Thread{
	private MulticastSocket mcastSocket;
	private DatagramPacket pacote;
//	private Thread blinker;
//https://docs.oracle.com/javase/9/docs/api/java/lang/doc-files/threadPrimitiveDeprecation.html
	public EnviarUDPThread(String nomeThread, DatagramPacket pacote, MulticastSocket mcastSocket){
		super(nomeThread);
		this.pacote = pacote;
		this.mcastSocket = mcastSocket;
	}
	/*public void start(){
		blinker = new Thread(this);
		blinker.start();
	}*/
	public void run(){
		this.mcastSocket.send(this.pacote);
	}

}