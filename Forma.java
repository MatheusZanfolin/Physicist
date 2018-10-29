public class Forma implements Desenhavel{
    private int indice;
    private double xRelCentro;
    private double yRelCentro;
    private double larguraRel;
    private double alturaRel;
    private TipoForma tipo;
    public Forma(int indiceForma, double x, double y, double largura, double altura)
    {
            this.indice = indiceForma;
            TipoForma encontrado;
loop:       for(encontrado : TipoForma.values()){
                if(encontrado.getIndice() == indiceForma)
                    break loop;                
            }
            this.tipo = encontrado;
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