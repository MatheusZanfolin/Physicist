public class MainTeste{
	private static Controlador meuControlador;
	public static void main(String[] args){
	meuControlador = new Controlador();
	System.out.println("Começando a tratar broadcasting..");
	meuControlador.inicializarEscuta();
	System.out.println("Escutando broadcasting..");
	meuControlador.receber();
	}
	public static depoisEnviar(){
		System.out.println("Enviado!!");
		meuControlador.finalizarBroadcasting();
	}
	public static depoisEscuta(){
		Peer achado = this.meuControlador.getPeerAchado();
		System.out.println(achado.getIP());
		System.out.println("Recebido!!");
		meuControlador.finalizarEscuta();
		System.out.println("Enviando resposta!");
		meuControlador.inicializarBroadcasting();
		System.out.println("Enviando....");
		meuControlador.enviar();	
	}
}