using System;
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
        public static bool flagFimRecebimento = false;
        public static bool flagFimInterpretacao = false;
        public static bool flagFimSimulacao = false;
        private System.Threading.Timer timerDesenhaveis;
        private System.Threading.Timer timerSemaforo;
        private const int intervaloTimer = 17;
        private const double taxaProporcaoAltura = 0.5;
        private const double taxaProporcaoLargura = 0.5;
        private const int espessura = 5;
        private Color corReta = Color.Yellow;
        private Color corCircunferencia = Color.Black;
        Pen caneta;
        Graphics g;
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
            timerDesenhaveis.Dispose();
            timerDesenhaveis = null;
            if (ehInterpretacao)
            {
                if(Form2.estadoInterpretacao() == TaskStatus.RanToCompletion)
                {

                }
                else
                {

                }
            }
            else
            {
                if(Form2.estadoRecebimento() == TaskStatus.RanToCompletion)
                {

                }
                else
                {

                }
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
            g = CreateGraphics();
            Action<object> interpretacao = (object obj) =>
            {
                //bool flagFim = false;
                semaforoDesenhaveis.WaitOne();
                //inicializarTimer(5);
                
                    try
                    {
                        while (!DesenhavelRepositorio.estaVazio())
                        {
                            Desenhavel desenhavel = DesenhavelRepositorio.obter();
                            interpretarDesenhavel(desenhavel);
                        }
                        //achouCon = true;
                    }
                    catch (Exception ex)
                    {
                        //achouCon = false;
                        flagFimInterpretacao = true;
                    }

                semaforoDesenhaveis.Release();
                //semaforoDesenhaveis.Release();

            };
            Action<object> recebimento = (object obj) =>
            {
                //bool flagFim = false;
                semaforoDesenhaveis.WaitOne();
                //inicializarTimer(4);*/
                
                    try
                    {
                        ConexaoP2P.inicializarPeer(4);
                        //esse método já recebe e adiciona na
                        //classe DesenhavelRepositorio
                        
                    }
                    catch (Exception ex)
                    {
                        //achouCon = false;
                        Application.DoEvents();
                    }
                semaforoDesenhaveis.Release();
                
                //semaforoDesenhaveis.Release();
            };
            Action<object> controleSemaforo = (object obj) =>
            {
                administrarSemaforo();
            };
            semaforoDesenhaveis = new Semaphore(0, 1);
            interpretarDesenhaveis = new Task(interpretacao, "interpretarDesenhaveis");
            receberDesenhaveis = new Task(recebimento, "receberDesenhaveis");
            controlarSemaforo = new Task(controleSemaforo, "controlarSemaforo");
            interpretarDesenhaveis.Start();
            //receberDesenhaveis.Start();
            semaforoDesenhaveis.Release();
            controlarSemaforo.Start();
            finalizarTimer(true);
        }
        private void administrarSemaforo()
        {
            //predict de erro aqui
            while (!flagFimSimulacao)
            {
                if (flagFimInterpretacao) {
                    
                    finalizarTimer(false);
                    inicializarTimer(5);
                    receberDesenhaveis.Start();
                    
                }
                if(flagFimRecebimento)
                {
                    
                    finalizarTimer(false);
                    inicializarTimer(4);
                    interpretarDesenhaveis.Start();
                    
                }
                    
                /*0 -   broadcasting
          1 -   respostaBroadcasting
          2 -   escutarPeer
          3 -   escutarCon
          4 -   receberDesenhaveis
          5 -   interpretarDesenhaveis
             */

            }
            
        }
        private void interpretarDesenhavel(Desenhavel aInterpretar)
        {
            //this.width => largura da janela
            //this.length => altura da janela
            //** espessura e cor
            double xC = 0, yC = 0, largura = 0, altura = 0;
            xC = aInterpretar.XRelCentro;
            yC = aInterpretar.YRelCentro;
            largura = aInterpretar.LarguraRel;
            altura = aInterpretar.AlturaRel;
            if(aInterpretar.GetType() == typeof(Imagem))
            {
                Imagem imagemAInterpretar = (Imagem)aInterpretar;
            }
            if(aInterpretar.GetType() == typeof(Forma))
            {
                Forma formaAInterpretar = (Forma)aInterpretar;
                
                switch (formaAInterpretar.Tipo)
                {
                    case Forma.TipoForma.Reta:
                        caneta = new Pen(corReta, espessura);
                        double x1=0, y1=0, x2=0, y2=0;
                        x1 = xC - (largura / 2);
                        y1 = yC - (altura / 2);
                        x2 = x1 + largura;
                        y2 = y1 + altura;

                        break;
                    case Forma.TipoForma.Circunferencia:
                        caneta = new Pen(corCircunferencia, espessura);
                        largura *= taxaProporcaoLargura;
                        altura *= taxaProporcaoAltura;
                        g.DrawEllipse(caneta, )
                        break;
                }
            }
        }
    }
}
