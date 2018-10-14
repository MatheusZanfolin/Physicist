/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package main;import java.net.*;
public class ReceberUDPThread extends Thread{
	private MulticastSocket mcastSocket;
	private DatagramPacket pacote;
	public ReceberUDPThread(String nomeThread, DatagramPacket pacote, MulticastSocket mcastSocket){
		super(nomeThread);
		this.pacote = pacote;
		this.mcastSocket = mcastSocket;
	}
	public void run(){
		try{
			this.mcastSocket.receive(this.pacote);
		}
		catch(Exception ex){
			System.out.println("Erro ao receber");
		}
	}
	public byte[] getDados(){
		return this.pacote.getData();
	}
	public InetAddress getIPQuemEnviou(){
		return this.pacote.getAddress();
	}
}