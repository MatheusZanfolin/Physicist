public class Main{
	private static Controlador meuControlador;
	public static void main(String[] args){
		meuControlador = new Controlador();
		System.out.println("Começando a tratar broadcasting..");
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
		//meuControlador.finalizarEscuta();
		meuContolador.inicializarTCP();
		System.out.println("Enviando TCP!!");
		meuControlador.enviarTCP();

	}
	public static void depoisEnviarTCP(){
		System.out.println("Enviado TCP!!");
	}
}