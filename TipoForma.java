public enum TipoForma{
    RETA(0),
    ELIPSE(1);
    private int indice;
    public TipoForma(int ind){
        this.indice = ind;
    }
    public int getIndice(){
        return this.indice;
    }
    public void setIndice(int novoInd){
        this.indice = novoInd;
    }
}