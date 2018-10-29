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
        private static bool primeiro = false;
        /*public DesenhavelRepositorio()
        {
            DesenhavelRepositorio.filaDesenhaveis = new Queue<Desenhavel>();
        }*/
        public static void armazenar(Desenhavel aInserir, int repeticoes)
        {
            DesenhavelRepositorio.primeiro = false;
            if(DesenhavelRepositorio.desenhaveisFrame == null)
            {
                DesenhavelRepositorio.desenhaveisFrame = new List<Desenhavel>();
                DesenhavelRepositorio.primeiro = true;
            }
            if (DesenhavelRepositorio.desenhaveisFrame.Exists(x => x.Indice == aInserir.Indice))
                return;//já está inserida a qtd de imagens desse mesmo
                       //tipo nesse frame
            for(int i = 0; i < repeticoes; i++)
            {
                DesenhavelRepositorio.desenhaveisFrame.Add(aInserir);
            }
        }
        public static bool isUltimo()
        {
            return DesenhavelRepositorio.desenhaveisFrame == null || DesenhavelRepositorio.desenhaveisFrame.Count < 1;
        }
        public static bool Primeiro
        {
            get
            {
                return DesenhavelRepositorio.primeiro;
            }
            set
            {
                DesenhavelRepositorio.primeiro = value;
            }
        }
        public static void armazenar(Desenhavel aInserir)
        {
            if (DesenhavelRepositorio.filaDesenhaveis == null)
            {
                DesenhavelRepositorio.filaDesenhaveis = new Queue<Desenhavel>();
            }
            DesenhavelRepositorio.filaDesenhaveis.Enqueue(aInserir);
            DesenhavelRepositorio.desenhaveisFrame.Remove(aInserir);
            if (DesenhavelRepositorio.desenhaveisFrame.Count < 1)
                DesenhavelRepositorio.desenhaveisFrame = null;
        }
        public static Desenhavel obter()
        {
            if (DesenhavelRepositorio.filaDesenhaveis == null || DesenhavelRepositorio.filaDesenhaveis.Count < 1)
                throw new Exception("Fila vazia!");
            return DesenhavelRepositorio.filaDesenhaveis.Dequeue();
        }
        public static bool estaVazio()
        {
            return (DesenhavelRepositorio.filaDesenhaveis == null || DesenhavelRepositorio.filaDesenhaveis.Count < 1);
        }
    }
}
