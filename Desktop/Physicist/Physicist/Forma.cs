

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physicist
{
    class Forma : Desenhavel
    {
        private int indice;
        private double xRelCentro;
        private double yRelCentro;
        private double larguraRel;
        private double alturaRel;
        private TipoForma tipo;
        public enum TipoForma{
            Reta,
            Elipse,
            Tracejado
        }
        public Forma(int indiceForma, double x, double y, double largura, double altura)
        {
            this.indice = indiceForma;
            this.tipo = (TipoForma) indiceForma;
            this.xRelCentro = x;
            this.yRelCentro = y;
            this.larguraRel = largura;
            this.alturaRel = altura;
        }
        public int Indice
        {
            get
            {
                return this.indice;
            }
        }
        public TipoForma Tipo
        {
            get
            {
                return this.tipo;
            }
        } 
        public double XRelCentro
        {
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
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (obj.GetType() != this.GetType())
                return false;
            Forma im = (Forma)obj;

            return this.indice == im.indice;
        }
    }
}
