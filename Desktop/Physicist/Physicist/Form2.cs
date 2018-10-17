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
        private const int intervaloTimer = 17;
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
        private void finalizarTimer(bool ehInterpretacao)
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
            Action<object> interpretacao = (object obj) =>
            {
                //bool flagFim = false;
                semaforoDesenhaveis.WaitOne();
                //inicializarTimer(5);
                
                    try
                    {
                        Desenhavel desenhavel = DesenhavelRepositorio.obter();
                        interpretarDesenhavel(desenhavel);

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
                        //Desenhavel desenhavel = DesenhavelRepositorio.obter();
                        //interpretarDesenhavel(desenhavel);

                        //achouCon = true;
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
            
        }
        private void administrarSemaforo()
        {
            //predict de erro aqui
            while (!flagFimSimulacao)
            {
                if (flagFimInterpretacao) {
                    finalizarTimer(true);
                    receberDesenhaveis.Start();
                }
                if(flagFimRecebimento)
                {
                    finalizarTimer(false);
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

        }
    }
}
