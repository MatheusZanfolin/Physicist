using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physicist
{
    class Imagem : Desenhavel
    {
        private double xRelCentro;
        private double yRelCentro;
        private double larguraRel;
        private double alturaRel;
        public Imagem(double x, double y, double largura, double altura)
        {
            this.xRelCentro = x;
            this.yRelCentro = y;
            this.larguraRel = largura;
            this.alturaRel = altura;
        }
        public double XRelCentro {
            get
            {
                return this.xRelCentro;
            }
        }

        public double YRelCentro
        {
            get
            {
                return this.yRelCentro;
            }
        }

        public double AlturaRel
        {
            get
            {
                return alturaRel;
            }
        }

        public double LarguraRel
        {
            get
            {
                return larguraRel;
            }
        }
    }
}
