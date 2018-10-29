import java.lang.Thread;
import java.io.*;
public class Main{
	private static Controlador meuControlador;
	private static BufferedReader teclado = new BufferedReader(new InputStreamReader(System.in));
	public static void main(String[] args){
		meuControlador = new Controlador();
		System.out.println("Começando a fazer broadcasting..");
		meuControlador.inicializarBroadcasting();
		System.out.println("Enviando broadcasting..");
		meuControlador.enviar();
	}
	public static void depoisEnviar(){
		System.out.println("Enviado!!");
		//meuControlador.finalizarBroadcasting();
		System.out.println("Escutando resposta!");
		meuControlador.inicializarEscuta();
		System.out.println("Escutando....");
		meuControlador.receber();
	}
	public static void depoisEscuta(){
		Peer achado = meuControlador.getPeerAchado();
		System.out.println(achado.getIP());
		System.out.println("Recebido!!");
		try{
			String x = teclado.readLine();
		}
		catch(Exception e){
			System.out.println(e.getMessage());
		}
        Desenhavel[] dados;
        //se deseja parar a simulação, 
        //não passe parâmetros no
        //método inicializarTCP
        meuControlador.inicializarTCP(dados);
		System.out.println("Enviando TCP!!");
		meuControlador.enviarTCP();
	}
	public static void depoisEnviarTCP(){
		System.out.println("Enviado TCP!!");
	}
}