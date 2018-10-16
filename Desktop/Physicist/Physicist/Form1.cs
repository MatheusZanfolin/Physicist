﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Physicist
{
    public partial class frmPrincipal : Form
    {
        bool ehPossivelCancelar = false;
        bool ehPossivelCancelarResposta= false;
        private static Controlador meuControlador;
        private static List<Peer> listaDispositivos = new List<Peer>();
        private static byte[] buffer;
        private static int numElementosListaForm = 0;
        private static System.Windows.Forms.Timer timerLista;
        private static bool flagSairTimer;
        private static List<Desenhavel> listaDesenhaveis;
        public frmPrincipal()
        {
            InitializeComponent();
            
        }
        private static void TimerEventProcessor(Object myObject,
                                            EventArgs myEventArgs)
        {
            timerLista.Stop();

            // Displays a message box asking whether to continue running the timer.
            if (listaDispositivos.Count != numElementosListaForm)
            {
                // Restarts the timer and increments the counter.
                
                timerLista.Enabled = true;
            }
            else
            {
                // Stops the timer.
                flagSairTimer = true;
            }
        }
        private void inicializarTimer()
        {
            timerLista = new System.Windows.Forms.Timer();
            timerLista.Tick += new EventHandler(TimerEventProcessor);
            timerLista.Interval = 1000;
            timerLista.Start();
        }
        private void btnListar_Click(object sender, EventArgs e)
        {
            if (ehPossivelCancelar)
            {
                inserirNaLista();
                meuControlador.finalizarBroadcasting();
                ehPossivelCancelar = false;
                btnListar.Text = "Buscar Dispositivos";
            }
            else {
                ehPossivelCancelar = true;
                btnListar.Text = "Verificar Busca";
                meuControlador.inicializarBroadcasting();
                procurarDispositivos();
                meuControlador.finalizarBroadcasting();
                /*inicializarTimer();
                while (!flagSairTimer)
                    Application.DoEvents();
                if (flagSairTimer)
                    inserirNaLista();*/
                //procurarDispositivos é async
                //inserirNaLista();
                //Thread.Sleep(1000);
                

            }
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            if (ehPossivelCancelarResposta)
            {
                atualizarForm();
                meuControlador.finalizarRespostaBroadcasting();
                ehPossivelCancelar = false;
                btnConectar.Text = "Conectar com esse dispositivo";
            }
            else
            {
                ehPossivelCancelar = true;
                btnConectar.Text = "Verificar resposta";
                meuControlador.inicializarRespostaBroadcasting();
                responderPeer(lsbDispositivos.SelectedIndex);
                meuControlador.finalizarRespostaBroadcasting();
                //.. esperar a resposta
                //responder peer é async
                
            }

        }
        private async void responderPeer(int indice)
        {
           /* try
            {*/
                meuControlador.responderBroadcasting();
           
            /*}
            catch(Exception ex)
            {
                if(ex.Message == "D")
                {
                    this.interpretarBuffer(meuControlador.Buffer);
                    //interpretar o buffer
                }
            }*/
        }
        public static void interpretarBuffer(byte[] buf)
        {
            // enfileirar dados
            buffer = new byte[buf.Length];
            for (int i = 0; i < buf.Length; i++)
                buffer[i] = buf[i];
            MessageBox.Show("Tratando dados");
            //pegar conjuntos de 33 em 33 bytes
            listaDesenhaveis = new List<Desenhavel>();
            for(int i=0; i< buf.length; i = i = i+33){
                Desenhavel itemBuffer;
                /*byte[] xRel        = new byte[8];
                for(i=1;i<xRel.Length;i++){
                    xRel[i] = buf[i];
                }
                byte[] yRel        = new byte[8];
                for(;i<yRel.Length;i++){
                    yRel[i] = buf[i];
                }
                byte[] larguraRel  = new byte[8];
                for(;i<larguraRel.Length;i++){
                    larguraRel[i] = buf[i];
                }
                byte[] alturaRel   = new byte[8];
                for(;i<alturaRel.Length;i++){
                    alturaRel[i] = buf[i];
                }
                i = 0;*/
                double xRel = BitConverter.ToDouble(buf, 1);
                double yRel = BitConverter.ToDouble(buf, 9);
                double larguraRel = BitConverter.ToDouble(buf, 17);
                double alturaRel = BitConverter.ToDouble(buf, 25);
                if(buf[i] == 0x00){
                    //flag para imagem = 00
                    itemBuffer = new Imagem(xRel, yRel, larguraRel, alturaRel);
                }
                if(buf[i] == 0xFF){
                    //flag para FIM = 255
                    return;
                }
                if(buf[i] == 0x7F){
                    //flag para reta = 127
                    itemBuffer = new Reta(xRel, yRel, larguraRel, alturaRel);
                }
                listaDesenhaveis.add(itemBuffer);
            }
            
        }
        private async void procurarDispositivos()
        {
		    //try{
                meuControlador.testarBroadcasting();
		    /*}
		    catch(Exception ex){
			    if(ex.Message == "B"){
				    
			    }
			    else{
				    MessageBox.Show("Busca cancelada!");
			    }	
		    }*/

        }
        private void atualizarForm()
        {

        }
        private void inserirNaLista()
        {
            if (numElementosListaForm != listaDispositivos.Count)
            
            {
                lsbDispositivos.Items.Add(listaDispositivos.Last().ToString());
                numElementosListaForm++;
            }
        }
        public static void listarDispositivos() {
            Peer achado = Controlador.PeerAchado;
            listaDispositivos.Add(achado);
            
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            
            meuControlador = new Controlador();

        }
    }
}
