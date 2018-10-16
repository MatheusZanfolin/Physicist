using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physicist
{
    public interface Desenhavel
    {
        /*
         dados da simulação
taxa de proporção altura
taxa de proporção largura
//largura e altura da tela do pc

dados de cada imagem
flagReta	(1 byte)

se não é reta (imagem)
xRelCentro	(8 bytes)double
yRelCentro	(8 bytes)
larguraRel	(8 bytes)
alturaRel	(8 bytes)

se é reta
x1RelPonto	(8 bytes)
y1RelPonto	(8 bytes)
larguraRel	(8 bytes)(pode ser negativo)
alturaRel	(8 bytes)(pode ser negativo)

desenhavel
x1RelPonto	(8 bytes)
y1RelPonto	(8 bytes)
larguraRel	(8 bytes)
alturaRel	(8 bytes)



buffer[2048]
             */
        /*private double xRelPonto;
        private double yRelPonto;
        private double larguraRel;
        private double alturaRel;*/
        /*public Desenhavel(double x, double y, double largura, double altura)
        {
            this.xRelPonto = x;
            this.yRelPonto = y;
            this.larguraRel = largura;
            this.alturaRel = altura;
        }*/
        double XRelCentro { get; }
        double YRelCentro{ get; }
        double AlturaRel{ get; }
        double LarguraRel{ get; }
            
        }
    }
}
