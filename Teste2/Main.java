import java.lang.Thread;
import java.io.*;
public class Main{
	private static Controlador meuControlador;
	private static BufferedReader teclado = new BufferedReader(new InputStreamReader(System.in));
	public static void main(String[] args){
		meuControlador = new Controlador();
		//System.out.println("Começando a tratar broadcasting..");
		meuControlador.inicializarBroadcasting();
		System.out.println("Peer 2: Enviando requisição...");
		meuControlador.enviar();
	}
	public static void depoisEnviar(){
		//System.out.println("Enviado!!");
		//meuControlador.finalizarBroadcasting();
		System.out.println("Peer 2: Aguardando resposta...");
		meuControlador.inicializarEscuta();
		//System.out.println("Escutando....");
		meuControlador.receber();
	}
	public static void depoisEscuta(){
		Peer achado = meuControlador.getPeerAchado();

		System.out.println("Peer 2: Resposta recebida!");
		System.out.println("Peer 2: Requisição recebida pelo peer de IP " + achado.getIP().getHostAddress());

		try {
			System.out.print("Mensagem: ");

			String x = teclado.readLine();

			System.out.println();
		}
		catch(Exception e){
			System.out.println(e.getMessage());
		}
		meuControlador.inicializarTCP();
		//System.out.println("Enviando TCP!!");
		meuControlador.enviarTCP();

	}
	public static void depoisEnviarTCP(){
		System.out.println("Mensagem enviada!");
	}
}