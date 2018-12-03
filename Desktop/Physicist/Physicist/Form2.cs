﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace Physicist
{
    public partial class Form2 : Form
    {
        public static Task interpretarDesenhaveis;
        public static Task receberDesenhaveis;
        public static Task controlarSemaforo;
        private static Action<object> recebimento;
        private static Action<object> interpretacao;
        public static bool flagFimRecebimento = false;
        public static bool flagFimInterpretacao = false;
        public static bool flagFimSimulacao = false;
        private System.Threading.Timer timerDesenhaveis;
        private System.Threading.Timer timerSemaforo;
        private const int intervaloTimer = 17;
        private const double taxaProporcaoAltura = 1;
        private const double taxaProporcaoLargura = 0.5;
        private const int espessura = 5;
        private static float alturaTotal;
        private static float larguraTotal;
        private Color corReta = Color.Yellow;
        private Color corElipse = Color.Black;
        private Color corTracejado = Color.Gray;
        Pen caneta;
        Graphics g = null;
        public Semaphore semaforoDesenhaveis;
        public Form2()
        {
            InitializeComponent();
            
        }
        private void inicializarTimer(int indTarefa)
        {//não tenho ctz se ele é async ou não

            var autoEvento = new AutoResetEvent(false);
            var checador = new ChecadorStatus(indTarefa);
            timerDesenhaveis = new System.Threading.Timer(checador.CheckStatus, autoEvento, intervaloTimer, intervaloTimer);
            //autoEvento.WaitOne();
            //liberar e reiniciar o timer
            //"flag" autoevento foi alterada
            //timerP2P.Dispose();
            //throw new Exception("Timer Peer to Peer acabou!");

        }
        private void finalizarTimer(bool ehSemaforo)
        {
            if(timerDesenhaveis!=null)
                timerDesenhaveis.Dispose();
            timerDesenhaveis = null;
            if (ehSemaforo)
            {
                timerSemaforo.Dispose();
                timerSemaforo = null;
                
            }
        }
        private static TaskStatus estadoInterpretacao()
        {
            return interpretarDesenhaveis.Status;
        }
        private static TaskStatus estadoRecebimento()
        {
            return receberDesenhaveis.Status;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            
            interpretacao = (object obj) =>
            {
                //bool flagFim = false;
                semaforoDesenhaveis.WaitOne();
                //inicializarTimer(5);

                //MessageBox.Show("Interpretando");
                    while (!DesenhavelRepositorio.estaVazio())
                    {
                        try
                        {
                            g = CreateGraphics();//resetar os gráficos

                        }
                        catch
                        {

                        } 
                        
                        Desenhavel desenhavel = DesenhavelRepositorio.obter();
                        if(desenhavel!=null)
                            interpretarDesenhavel(desenhavel);
                        while (!DesenhavelRepositorio.Primeiro)//significa que esse é o primeiro desenhável
                        {
                            desenhavel = DesenhavelRepositorio.obter();

                            if (desenhavel != null)
                                interpretarDesenhavel(desenhavel);
                        }
                        Invalidate();

                }
               semaforoDesenhaveis.Release();
                //semaforoDesenhaveis.Release();

            };
            recebimento = (object obj) =>
            {
                bool estaEscutando = false;
                //bool flagFim = false;
                semaforoDesenhaveis.WaitOne();
                //inicializarTimer(4);*/
                //MessageBox.Show("Escutando");
                        
                if (!ControladorDesenhavel.Interpretando)
                {
                    ConexaoP2P.tratarDados();
                    estaEscutando = true;
                }
                        
                        
                //esse método já recebe e adiciona na
                //classe DesenhavelRepositorio

                   
                 semaforoDesenhaveis.Release();
                
                //semaforoDesenhaveis.Release();
            };
            Action<object> controleSemaforo = (object obj) =>
            {
                administrarSemaforo();
            };
            semaforoDesenhaveis = new Semaphore(0, 1);
            controlarSemaforo = new Task(controleSemaforo, "controlarSemaforo");
            //   receberDesenhaveis.Start();
            flagFimInterpretacao = true;
            //receberDesenhaveis.Start();
            semaforoDesenhaveis.Release();
            controlarSemaforo.Start();
            //finalizarTimer(true);
        }
        private void administrarSemaforo()
        {
            //predict de erro aqui
            while (!flagFimSimulacao)
            {
                //receberDesenhaveis = null;
                receberDesenhaveis = new Task(recebimento, "receberDesenhaveis");

                //inicializarTimer(4);
                receberDesenhaveis.Start();
                interpretarDesenhaveis = new Task(interpretacao, "interpretarDesenhaveis");

                //inicializarTimer(5);
                interpretarDesenhaveis.Start();
                //Thread.Sleep(17);   
            }
                    
                /*0 -   Multicasting
          1 -   respostaMulticasting
          2 -   escutarPeer
          3 -   escutarCon
          4 -   receberDesenhaveis
          5 -   interpretarDesenhaveis
          6 -   semaforoDesenhaveis
             */

            
            
        }
        private void interpretarDesenhavel(Desenhavel aInterpretar)
        {
            //this.width => largura da janela
            //this.length => altura da janela
            //** espessura e cor
            

            alturaTotal = this.Height;
            larguraTotal = this.Width;
            double xC = 0, yC = 0, largura = 0, altura = 0;
            xC = aInterpretar.XRelCentro*larguraTotal;
            yC = aInterpretar.YRelCentro*alturaTotal;
            largura = aInterpretar.LarguraRel*larguraTotal;
            altura = aInterpretar.AlturaRel*alturaTotal;
            if(aInterpretar.GetType() == typeof(Imagem))
            {
                Imagem imagemAInterpretar = (Imagem)aInterpretar;
                Image imagemInterpretada = imagemAInterpretar.imagem();
                if (imagemInterpretada != null)
                {
                    double x1 = 0, y1 = 0;
                    largura *= taxaProporcaoLargura;
                    altura *= taxaProporcaoAltura;
                    x1 = xC - (largura / 2);
                    y1 = yC - (altura / 2);
                    g.DrawImage(imagemInterpretada, Convert.ToInt32(x1), Convert.ToInt32(y1),
                        Convert.ToInt32(largura), Convert.ToInt32(altura));
                }
            }
            if(aInterpretar.GetType() == typeof(Forma))
            {
                Forma formaAInterpretar = (Forma)aInterpretar;
                double x1 = 0, y1 = 0, x2 = 0, y2 = 0;

                switch (formaAInterpretar.Tipo)
                {
                    case Forma.TipoForma.Reta:
                        try
                        {
                            caneta = new Pen(corReta, espessura);
                            x1 = xC - (largura / 2);
                            y1 = yC - (altura / 2);
                            x2 = x1 + largura;
                            y2 = y1 + altura;
                            g.DrawLine(caneta, (float)x1, (float)y1, (float)x2, (float)y2);
                        }
                        catch
                        {

                        }
                            break;
                    case Forma.TipoForma.Elipse:
                        caneta = new Pen(corElipse, espessura);
                        largura *= taxaProporcaoLargura;
                        altura *= taxaProporcaoAltura;
                        g.DrawEllipse(caneta, (float)xC, (float)yC, (float)largura, (float)altura);
                        break;
                    case Forma.TipoForma.Tracejado:
                        caneta = new Pen(corTracejado, espessura);
                        float[] padraoTracos = {20,40};//tem que ser múltiplo de 1,2
                        caneta.DashPattern = padraoTracos;
                        //double x1=0, y1=0, x2=0, y2=0;
                        x1 = xC - (largura / 2);
                        y1 = yC - (altura / 2);
                        x2 = x1 + largura;
                        y2 = y1 + altura;
                        g.DrawLine(caneta, (float)x1, (float)y1,(float) x2,(float) y2);
                        break;
                }
            }
        }
    }
}
