using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physicist
{
    class DesenhavelRepositorio
    {
        private static Queue<Desenhavel> filaDesenhaveis;
        private static List<Desenhavel> desenhaveisFrame;
        private static List<int> frames = new List<int>();
        private static int numObtencoes;
        /*public DesenhavelRepositorio()
        {
            DesenhavelRepositorio.filaDesenhaveis = new Queue<Desenhavel>();
        }*/
        public static void armazenar(Desenhavel aInserir, int repeticoes)
        {
            try
            {
                if (DesenhavelRepositorio.desenhaveisFrame == null)
                {
                    DesenhavelRepositorio.desenhaveisFrame = new List<Desenhavel>();
                }
                if (DesenhavelRepositorio.desenhaveisFrame.Exists(x => x.Indice == aInserir.Indice))
                    return;//já está inserida a qtd de imagens desse mesmo
                           //tipo nesse frame

                for (int i = 0; i < repeticoes; i++)
                {
                    DesenhavelRepositorio.desenhaveisFrame.Add(aInserir);
                }
            }
            catch
            {

            }
        }
        public static bool isUltimo()
        {
            try
            {
                return DesenhavelRepositorio.desenhaveisFrame == null || DesenhavelRepositorio.desenhaveisFrame.Count < 1;
            }
            catch
            {


            }
            return false;
        }
        public static bool Primeiro
        {
            get
            {
                try
                {
                    int somatoria = 0;
                    if (somatoria == numObtencoes)
                        return true;
                    for (int i = 0; i < frames.Count; i++)
                    {
                        somatoria += frames[i];
                        if (somatoria == numObtencoes)
                        {
                            for(int j = 0; j <= i; j++)
                            {
                                frames.RemoveAt(0);
                            }
                            numObtencoes = 0;
                            return true;
                        }
                            
                    }
                    
                }
                catch
                {

                }
                return false;
            }
            
        }
        public static void armazenar(Desenhavel aInserir)
        {
            try
            {
                if (DesenhavelRepositorio.filaDesenhaveis == null)
                {
                    DesenhavelRepositorio.filaDesenhaveis = new Queue<Desenhavel>();
                }

                DesenhavelRepositorio.filaDesenhaveis.Enqueue(aInserir);
                DesenhavelRepositorio.desenhaveisFrame.Remove(aInserir);
                if (DesenhavelRepositorio.desenhaveisFrame.Count < 1)
                {
                    DesenhavelRepositorio.desenhaveisFrame = null;
                    DesenhavelRepositorio.frames.Add(DesenhavelRepositorio.filaDesenhaveis.Count);
                }
            }
            catch
            {

            }
        }
        public static Desenhavel obter()
        {
            try
            {
                if (DesenhavelRepositorio.filaDesenhaveis == null || DesenhavelRepositorio.filaDesenhaveis.Count < 1)
                    throw new Exception("Fila vazia!");
                DesenhavelRepositorio.numObtencoes++;
                return DesenhavelRepositorio.filaDesenhaveis.Dequeue();
            }
            catch
            {

            }
            return null;
        }
        public static bool estaVazio()
        {
            return (DesenhavelRepositorio.filaDesenhaveis == null || DesenhavelRepositorio.filaDesenhaveis.Count < 1);
        }
    }
}
