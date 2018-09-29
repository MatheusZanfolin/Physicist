using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Physicist
{
    class ChecadorStatus
    {
        int indTarefa;
        Task tarefaAnalisar;
        /*0 -   broadcasting
          1 -   respostaBroadcasting
          2 -   escutarPeer
         // 3 -   lerStream
             */
        public ChecadorStatus(int indTarefa)
        {
            switch (indTarefa)
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
               /* case 3:
                    this.tarefaAnalisar = ConexaoP2P.lerStream;
                    break;*/
            }   
        }
        //esse método é chamado pelo delegado do timer
        public void CheckStatus(Object infoStatus)
        {
            AutoResetEvent autoEvento = (AutoResetEvent)infoStatus;
            if (ehParaParar(tarefaAnalisar.Status))
            {
                autoEvento.Set();
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
