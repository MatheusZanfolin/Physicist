using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Physicist
{
    class ChecadorStatus
    {
        int indTarefa;
        Task tarefaAnalisar;
        /*0 -   broadcasting
          1 -   respostaBroadcasting
          2 -   escutarPeer
          3 -   escutarCon
          4 -   receberDesenhaveis
          5 -   interpretarDesenhaveis
          6 -   semaforoDesenhaveis
             */
        public ChecadorStatus(int ind)
        {
            this.indTarefa = ind;
            switch (ind)
            {
                case 0:
                    this.tarefaAnalisar = Peer.broadcasting;
                    break;
                case 1:
                    this.tarefaAnalisar = Peer.respostaBroadcasting;
                    break;
                case 2:
                    this.tarefaAnalisar = ConexaoP2P.escutarPeer;
                    break;
                case 3:
                    this.tarefaAnalisar = ConexaoP2P.escutarConexao;
                    break;
                case 4:
                    this.tarefaAnalisar = Form2.receberDesenhaveis;
                    break;
                case 5:
                    this.tarefaAnalisar = Form2.interpretarDesenhaveis;
                    break;
                case 6:
                    this.tarefaAnalisar = Form2.controlarSemaforo;
                    break;
            }   
        }
        //esse método é chamado pelo delegado do timer
        public void CheckStatus(Object infoStatus)
        {
            AutoResetEvent autoEvento = (AutoResetEvent)infoStatus;
            
            if (ehParaParar(tarefaAnalisar.Status))
            {
                MessageBox.Show("Task parou!");
                if (indTarefa < 2)
                    Peer.finalizarTimer();
                if(indTarefa>=2 && indTarefa < 4)
                    ConexaoP2P.finalizarTimer();
                if (indTarefa == 4)
                {
                    Form2.flagFimRecebimento = true;
                    ConexaoP2P.finalizarTimer();
                }
                if (indTarefa == 5)
                    Form2.flagFimInterpretacao = true;
                if (indTarefa == 6)
                    Form2.flagFimSimulacao = true;
            }

        }
        private bool ehParaParar(TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.Canceled:
                    return true;
                    break;
                case TaskStatus.Faulted:
                    return true;
                    break;
                case TaskStatus.RanToCompletion:

                    return true;
                    break;
                case TaskStatus.WaitingForChildrenToComplete:
                    return true;
                    break;
                default:
                    return false;
            }
        }
    }
}
