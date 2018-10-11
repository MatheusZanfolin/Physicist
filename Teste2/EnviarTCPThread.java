import java.net.*;
import java.io.*;
public class EnviarTCPThread extends Thread{
	private DataOutputStream streamEnvio;
	private byte[] dados;
//	private Thread blinker;
//https://docs.oracle.com/javase/9/docs/api/java/lang/doc-files/threadPrimitiveDeprecation.html
	public EnviarTCPThread(String nomeThread, byte[] buffer ,DataOutputStream stream){
		super(nomeThread);
		this.dados = buffer;
		this.streamEnvio = stream;
	}
	public byte[] getDados(){
		return this.dados;
	}
	/*public void start(){
		blinker = new Thread(this);
		blinker.start();
	}*/
	public void run(){
		try{

			streamEnvio.write(dados, 0, dados.length);

		}
		catch(Exception ex){
			System.out.println("Erro ao enviar pacote");
		}
	}

}