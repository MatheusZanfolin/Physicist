import java.util.*;
import java.lang.Thread.State;
public class ChecadorStatus extends TimerTask{
	private Thread threadControlada;
	private static Thread.State status;
	public Thread.State getStatus(){
		return status;
	}
	public ChecadorStatus(Thread threadControlada){
		this.threadControlada = threadControlada;
	}
	public void run(){
		if(ehParaParar(threadControlada.getState())){
			if(threadControlada.equals(Peer.escuta)){
				try{
					Peer.depoisEscuta();
				}
				catch(Exception ex){
					System.out.println("Erro depois de escutar");
				}
			}
			if(threadControlada.equals(Peer.broadcasting)){
				try{
					Peer.depoisEnviar();
				}
				catch(Exception ex){
					System.out.println("Erro depois de enviar");
				}
			}
			if(threadControlada.equals(ConexaoP2P.emissao)){
				try{
					ConexaoP2P.depoisEnviarTCP():
				}
				catch(Exception ex){
					System.out.println("Erro depois de enviar TCP");
				}
			}
		}
	}
	public static boolean ehParaParar(Thread.State estadoAtual){
		status = estadoAtual;
		switch(estadoAtual){
			case NEW:
			//A thread n�o come�ou ainda
				return false;
			case RUNNABLE:
			//se est� executando nesse momento na m�quina virtual Java
				return false;
			case BLOCKED:
			//est� bloqueada esperando por um "monitor lock "
				return false;
			case WAITING:
			//esperando indefinitivamente por outra thread que est� fazendo uma a��o determinada
				return false;
			case TIMED_WAITING:
			//esperando outra thread fazer uma
			//"action for up to a specified waiting time"
				return false;
			case TERMINATED:
			//morreu
				return true;
			default:
				return true;
		}
	}
}