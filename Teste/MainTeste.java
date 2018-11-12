public class MainTeste{
	private static Controlador meuControlador;
	public static void main(String[] args){
		System.out.println("Peer 1: Aguardando requisição...");

		meuControlador = new Controlador();
		meuControlador.inicializarEscuta();
		meuControlador.receber();
	}
	public static void depoisEnviar(){

		//meuControlador.finalizarBroadcasting();
	}
	public static void depoisEscuta(){
		PeerTeste achado = meuControlador.getPeerAchado();

		System.out.println("Peer 1: Requisição recebida!");

		try{
			Thread.sleep(1000);
		}
		catch(Exception ex){
			System.out.println("Peer 1: Erro inesperado :(");
		}

		System.out.println("Peer 1: Enviando resposta ao peer com IP " + achado.getIP().getHostAddress());
		meuControlador.inicializarBroadcasting();
		meuControlador.enviar();
	}
}