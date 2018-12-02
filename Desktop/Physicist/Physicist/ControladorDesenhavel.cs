using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Physicist
{
    class ControladorDesenhavel
    {
        private static bool interpretando = false;
        public static bool Interpretando
        {
            get
            {
                return ControladorDesenhavel.interpretando;
            }
        }
        private const byte flagForma = 0xFE;
        private const byte flagFim = 0xFF;
        private const byte flagInicio = 0xFD;
        public static void tratarDados()
        {
            //byte[] buffer = ;
            if (!ControladorDesenhavel.Interpretando)
            {
                ControladorDesenhavel.interpretarBuffer(ConexaoP2P.Buffer);
                
            }
            

        }
        private static byte[] inverter (byte[] buffer, int comeco, int fim)
        {
            byte[] resultado = new byte[fim - comeco];
            for(int i = comeco; i < fim; i++)
            {
                byte atual = buffer[fim + comeco - 1 - i];
                /*if (atual < 128)
                    atual = (byte)~atual;
                */
                resultado[i - comeco] = atual;
            }
            return resultado;
        }
        public static void interpretarBuffer(byte[] buf)
        {
            
            ControladorDesenhavel.interpretando = true;
            
            // enfileirar dados
            /* byte[] buffer = new byte[buf.Length];
             for (int i = 0; i < buf.Length; i++)
                 buffer[i] = buf[i];
             *///MessageBox.Show("Tratando dados");
               //pegar conjuntos de 41 em 41 bytes
               //repDes = new DesenhavelRepositorio();
               //throw new Exception("a");
               //MessageBox.Show("Recebeu TCP");
            for (int i = 0; i==0 || i < buf.Length - 40; i = i + 41)
            {
                if (buf[i] == flagFim)
                {
                    //flag para FIM = 255
                    Form2.flagFimSimulacao = true;
                    ControladorDesenhavel.interpretando = false;

                    return;
                }
                if(buf[i] == flagInicio)
                {
                    frmPrincipal.flagAbrirSimulacao = true;
                    ControladorDesenhavel.interpretando = false;

                    return;
                }
                byte[] convertidoIndice = inverter(buf, i + 1, i + 5);

                byte[] convertidoQTD = inverter(buf, i + 5, i + 9);
                byte[] convertidoXRel = inverter(buf, i + 9, i + 17);
                byte[] convertidoYRel = inverter(buf, i + 17, i + 25);
                byte[] convertidoLarguraRel = inverter(buf, i + 25, i + 33);
                byte[] convertidoAlturaRel = inverter(buf, i + 33, i + 41);
                Desenhavel itemBuffer = null;
                //byte 0-> indica se é forma, imagem, fim, início, ...
                int indice = BitConverter.ToInt32(convertidoIndice, 0);
                //byte 1 até 4 => índice
                int qtdDesenhaveis = BitConverter.ToInt32(convertidoQTD, 0);
                //byte 5 até 8 => qtd Desenhaveis desse tipo
                double xRel = BitConverter.ToDouble(convertidoXRel, 0);
                //byte 9 até 16 => xRel
                double yRel = BitConverter.ToDouble(convertidoYRel, 0);
                //byte 17 até 24 => yRel
                double larguraRel = BitConverter.ToDouble(convertidoLarguraRel, 0);
                //byte 25 até 32 => larguraRel
                double alturaRel =(double) BitConverter.ToDouble(convertidoAlturaRel, 0);
                //byte 33 até 40 => alturaRel
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
            Form2.flagFimInterpretacao = false;
            Form2.flagFimRecebimento = true;
            ControladorDesenhavel.interpretando = false;
        }

    }
}
