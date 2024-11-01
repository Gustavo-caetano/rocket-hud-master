public static class Utils
{
    public static int IniciaOuFinalizaContagem(bool active, bool status)
    {
        int opcaoValor = 0; // opcao = 0 -> continua contagem
        if(!status && active) opcaoValor = 1; //false -> true = inicia contagem
        if(status && !active) opcaoValor = 2; //true -> false = finaliza contagem

        return opcaoValor; 
    }
}