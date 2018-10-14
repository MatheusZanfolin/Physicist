/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package main;
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
	public byte[] getDados(){
		return this.pacote.getData();
	}
	/*public void start(){
		blinker = new Thread(this);
		blinker.start();
	}*/
	public void run(){
		try{

			this.mcastSocket.send(this.pacote);

		}
		catch(Exception ex){
			System.out.println("Erro ao enviar pacote");
		}
	}

}