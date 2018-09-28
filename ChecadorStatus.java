public class ChecadorStatus extends TimerTask{
	private Thread threadControlada;
	public ChecadorStatus(Thread threadControlada){
		this.threadControlada = threadControlada;
	}
	public void run(){
		if(ehParaParar(escuta.getState())){

		}
		else{
		
		}
		
	}
	private boolean ehParaParar(Thread.State estadoAtual){
		switch(estadoAtual){
			case Thread.State.NEW:
			//A thread não começou ainda
			break;
			case Thread.State.RUNNABLE:
			//se está executando nesse momento na máquina virtual Java
			break;
			case Thread.State.BLOCKED:
			//está bloqueada esperando por um "monitor lock "
			break;
			case Thread.State.WAITING:
			//esperando indefinitivamente por outra thread que está fazendo uma ação determinada
			break;
			case Thread.State.TIMED_WAITING:
			//esperando outra thread fazer uma 
			//"action for up to a specified waiting time"
			break;
			case Thread.State.TERMINATED:
			//morreu
			break;
		}
	}
}