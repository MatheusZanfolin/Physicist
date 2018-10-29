using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Physicist
{
    class Imagem : Desenhavel
    {
        private int indice;
        private double xRelCentro;
        private double yRelCentro;
        private double larguraRel;
        private double alturaRel;
        
        public Imagem(int indiceImg,double x, double y, double largura, double altura)
        {
            this.indice = indiceImg;
            this.xRelCentro = x;
            this.yRelCentro = y;
            this.larguraRel = largura;
            this.alturaRel = altura;
        }
        public Image imagem()
        {
            String nomeArquivo = getArquivoPorIndice();
            return Image.FromFile(nomeArquivo);
        }
        private String getArquivoPorIndice()
        {
            return ""+(this.indice).ToString() + ".png";
        }
        public int Indice
        {
            get
            {
                return this.indice;
            }
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
        public override bool Equals(object obj)
        {
            return this.indice == ((Imagem)obj).indice;
        }
    }
}
