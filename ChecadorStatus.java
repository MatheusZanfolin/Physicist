public class ChecadorStatus extends TimerTask{
	private Thread threadControlada;
	private Thread.Status status;
	public getStatus(){
		return this.status;
	}
	public ChecadorStatus(Thread threadControlada){
		this.threadControlada = threadControlada;
	}
	public void run(){
		if(ehParaParar(threadControlada.getState())){
			if(threadControlada.equals(escuta))
				depoisEscuta();
			if(threadControlada.equals(broadcasting))	
				depoisEnviar();
		}
	}
	private boolean ehParaParar(Thread.State estadoAtual){
		this.status = estadoAtual;
		switch(estadoAtual){
			case Thread.State.NEW:
			//A thread não começou ainda
				return false;
			break;
			case Thread.State.RUNNABLE:
			//se está executando nesse momento na máquina virtual Java
				return false;
			break;
			case Thread.State.BLOCKED:
			//está bloqueada esperando por um "monitor lock "
				return false;
			break;
			case Thread.State.WAITING:
			//esperando indefinitivamente por outra thread que está fazendo uma ação determinada
				return false;
			break;
			case Thread.State.TIMED_WAITING:
			//esperando outra thread fazer uma 
			//"action for up to a specified waiting time"
				return false;
			break;
			case Thread.State.TERMINATED:
			//morreu
				return true;
			break;
		}
	}
}