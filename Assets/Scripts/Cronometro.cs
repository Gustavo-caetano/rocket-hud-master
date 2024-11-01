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

    public (string, long) ContagemTempo(bool active, long tempoAtual)
    {
        int opcao = Utils.IniciaOuFinalizaContagem(active, _status);
        
        if(opcao != 0)
        {
            _tempoInicial = active? tempoAtual : 0;
            _status = active;
            return ("", opcao);
        }
        if(!active) //continue não contando
        {
            return ("", opcao);
        }
        long tempoDecorrido = tempoAtual - _tempoInicial;
        return (FormatTempo(tempoDecorrido), tempoDecorrido);
    }
    private string FormatTempo(long tempo)
    {
        TimeSpan timeSpan = TimeSpan.FromMilliseconds(tempo);
        int seconds = timeSpan.Seconds;
        int centiseconds = timeSpan.Milliseconds / 10;

        return $"{seconds:D2}:{centiseconds:D2}";
    }
}
