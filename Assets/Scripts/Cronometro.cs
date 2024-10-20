using System;

public class Cronometro
{
    private long _tempoInicial;
    private bool _status;

    public string ContagemTempo(bool active, long tempoAtual, float peso)
    {
        if (!_status && active) // de false para true -> inicia
        {
            _tempoInicial = tempoAtual;
        }
        else if (_status && active) // continua contando o tempo
        {
            long tempoDecorrido = tempoAtual - _tempoInicial;
            return FormatTempo(tempoDecorrido);
        }
        else if (_status && !active) // de true para false -> zera a contagem
        {
            _tempoInicial = 0;
        }

        _status = active;
        return FormatTempo(tempoAtual);
    }

    private string FormatTempo(long tempo)
    {
        if (tempo == 0) return "";
        TimeSpan timeSpan = TimeSpan.FromMilliseconds(tempo);
        int seconds = timeSpan.Seconds;
        int centiseconds = timeSpan.Milliseconds / 10;

        return $"{seconds:D2}:{centiseconds:D2}";
    }
}
