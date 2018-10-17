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
        /*public DesenhavelRepositorio()
        {
            DesenhavelRepositorio.filaDesenhaveis = new Queue<Desenhavel>();
        }*/
        public static void armazenar(Desenhavel aInserir)
        {
            if (DesenhavelRepositorio.filaDesenhaveis == null)
            {
                DesenhavelRepositorio.filaDesenhaveis = new Queue<Desenhavel>();
            }
            DesenhavelRepositorio.filaDesenhaveis.Enqueue(aInserir);
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
