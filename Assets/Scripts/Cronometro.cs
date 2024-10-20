using System;

public class Cronometro
{
    private long _tempoInicial;
    private bool _status;

    public Cronometro()
    {
        _tempoInicial = 0;
        _status = false;
    }

    public string ContagemTempo(bool active, long tempoAtual)
    {
        if(IniciaOuFinalizaContagem(active))
        {
            _tempoInicial = active? tempoAtual : 0;
            _status = active;
            return "";
        }
        if(!active) //continue não contando
        {
            return "";
        }
        long tempoDecorrido = tempoAtual - _tempoInicial;
        return FormatTempo(tempoDecorrido);
    }
    public bool IniciaOuFinalizaContagem(bool active)
    {
        return (!_status && active) || (_status && !active); //  false -> true = inicia contagem, true -> false = finaliza contagem 
    }
    private string FormatTempo(long tempo)
    {
        TimeSpan timeSpan = TimeSpan.FromMilliseconds(tempo);
        int seconds = timeSpan.Seconds;
        int centiseconds = timeSpan.Milliseconds / 10;

        return $"{seconds:D2}:{centiseconds:D2}";
    }
}
