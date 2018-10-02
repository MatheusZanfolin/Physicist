public class Main{
	private static Controlador meuControlador;
	public static void main(String[] args){
	meuControlador = new Controlador();
	System.out.println("Começando broadcasting..");
	meuControlador.inicializarBroadcasting();
	System.out.println("Enviando broadcasting..");
	meuControlador.enviar();
	}
	public static depoisEnviar(){
		System.out.println("Enviado!!");
		meuControlador.finalizarBroadcasting();
		System.out.println("Esperando resposta!");
		meuControlador.inicializarEscuta();
		System.out.println("Escutando....");
		meuControlador.receber();
	}
	public static depoisEscuta(){
		Peer achado = this.meuControlador.getPeerAchado();
		System.out.println(achado.getIP());
		this.meuControlador.finalizarEscuta();
	}
}