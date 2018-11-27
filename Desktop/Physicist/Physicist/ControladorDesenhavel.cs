using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Physicist
{
    class ControladorDesenhavel
    {
        private const byte flagForma = 0xFE;
        private const byte flagFim = 0xFF;
        private const byte flagInicio = 0xFD;
        public static void tratarDados()
        {
            //byte[] buffer = ;
            ControladorDesenhavel.interpretarBuffer(ConexaoP2P.Buffer);
        }
        public static void interpretarBuffer(byte[] buf)
        {
            // enfileirar dados
            byte[] buffer = new byte[buf.Length];
            for (int i = 0; i < buf.Length; i++)
                buffer[i] = buf[i];
            //MessageBox.Show("Tratando dados");
            //pegar conjuntos de 41 em 41 bytes
            //repDes = new DesenhavelRepositorio();
            //throw new Exception("a");
            //MessageBox.Show("Recebeu TCP");
            for (int i = 0; i < buf.Length - 40; i = i + 41)
            {
                if (buf[i] == flagFim)
                {
                    //flag para FIM = 255
                    Form2.flagFimSimulacao = true;
                    return;
                }
                if(buf[i] == flagInicio)
                {
                    frmPrincipal.flagAbrirSimulacao = true;
                    return;
                }
                Desenhavel itemBuffer = null;
                int indice = BitConverter.ToInt32(buf, i + 1);
                int qtdDesenhaveis = BitConverter.ToInt32(buf, i + 5);
                double xRel = BitConverter.ToDouble(buf, i + 9);
                double yRel = BitConverter.ToDouble(buf, i + 17);
                double larguraRel = BitConverter.ToDouble(buf, i + 25);
                double alturaRel = BitConverter.ToDouble(buf, i + 33);

                if (buf[i] != flagFim && buf[i] != flagForma)
                {
                    //flag para imagem 

                    itemBuffer = new Imagem(indice, xRel, yRel, larguraRel, alturaRel);
                }
               
                if (buf[i] == flagForma)
                {
                    //flag para forma = 254
                    itemBuffer = new Forma(indice, xRel, yRel, larguraRel, alturaRel);
                }
                DesenhavelRepositorio.armazenar(itemBuffer, qtdDesenhaveis);
                DesenhavelRepositorio.armazenar(itemBuffer);
                
            }

        }
    }
}
