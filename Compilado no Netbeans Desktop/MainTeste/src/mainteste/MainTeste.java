/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mainteste;

/**
 *
 * @author rafae
 */public class MainTeste{
	private static Controlador meuControlador;
	public static void main(String[] args){
	meuControlador = new Controlador();
	System.out.println("Começando a tratar broadcasting..");
	meuControlador.inicializarEscuta();
	System.out.println("Escutando broadcasting..");
	meuControlador.receber();
	}
	public static void depoisEnviar(){
		System.out.println("Enviado!!");
		//meuControlador.finalizarBroadcasting();
	}
	public static void depoisEscuta(){
		PeerTeste achado = meuControlador.getPeerAchado();
		System.out.println(achado.getIP());
		System.out.println("Recebido!!");
		//meuControlador.finalizarEscuta();
		try{
			Thread.sleep(1000);
		}
		catch(Exception ex){
			System.out.println("Erro ao fazer a Thread dormir");
		}
		System.out.println("Enviando resposta!");
		meuControlador.inicializarBroadcasting();
		System.out.println("Enviando....");
		meuControlador.enviar();
	}
}