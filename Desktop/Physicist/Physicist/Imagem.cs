using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Physicist
{
    class Imagem : Desenhavel
    {
        private int indice;
        private double xRelCentro;
        private double yRelCentro;
        private double larguraRel;
        private double alturaRel;
        /*
        0 - lápis 
        1 - lápis invertido
        2 - espelho
        3 - espelho convexo
        4 - espelho côncavo
        5 - lente divergente
        6 - lente convergente
        7 - arraste-me
        */
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
            string nomeArquivo = getArquivoPorIndice();
            if (nomeArquivo != "")
                return Image.FromFile(nomeArquivo);
            else
                return null;
        }
        private string getArquivoPorIndice()
        {
            string pastaAtual = Directory.GetCurrentDirectory();
            pastaAtual += @"\..\..";
            if (this.indice > -1 && this.indice < 7)

                return pastaAtual + @"\imagens\" + (this.indice).ToString() + ".png";
            else
                return "";
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
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (obj.GetType() != this.GetType())
                return false;
            Imagem im = (Imagem)obj;

            return this.indice == im.indice;
        }
    }
}
