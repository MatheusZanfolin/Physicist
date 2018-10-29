import java.nio.*;
public class Imagem implements Desenhavel{
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
    public int getIndice(){
        return this.indice;
    }
    public double getXRelCentro(){
        return this.xRelCentro;
    }
    public double getYRelCentro(){
        return this.yRelCentro;
    }
    public double getAlturaRel(){
        return this.alturaRel;
    }
    public double getLarguraRel(){
        return this.larguraRel;
    }
    public byte[] converteBytes(){
        
    }
}