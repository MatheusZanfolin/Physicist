/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mainteste;

/**
 *
 * @author rafae
 */import java.util.*;
import java.net.*;

public class ConexaoP2P{
	private static ArrayList<PeerTeste> listaPeers;
	private final int portaP2PDesktop = 1337;
	private InetAddress ipLocal;
	public ConexaoP2P(InetAddress ipLocal){
		ipLocal = ipLocal;
		listaPeers = new ArrayList<PeerTeste>();
		try{
			listaPeers.add(new PeerTeste(ipLocal));
		}
		catch(Exception ex){}
	}
	public void inicializarBroadcasting(){
		listaPeers.get(0).inicializarBroadcasting();
	}
	public void enviar(){
		listaPeers.get(0).enviar();
	}
	public static void depoisEnviar(){
		Controlador.depoisEnviar();
	}
	public void finalizarBroadcasting(){
		listaPeers.get(0).finalizarBroadcasting();
	}
	public void inicializarEscuta(){
		listaPeers.get(0).inicializarEscuta();
	}
	public void receber(){
		listaPeers.get(0).receber();
	}
	public static void depoisEscuta(){
		//aqui temos certeza que a thread
		//de escutar acabou
		InetAddress IPQuemMandouMsg = listaPeers.get(0).getIPConectando();
		try{
			listaPeers.add(new PeerTeste(IPQuemMandouMsg));
		}
		catch(Exception ex){}
		Controlador.depoisEscuta();
	}
	public void finalizarEscuta(){
		listaPeers.get(0).finalizarEscuta();
	}
	public PeerTeste getUltimoPeer(){
		return listaPeers.get(listaPeers.size()-1);
	}
}