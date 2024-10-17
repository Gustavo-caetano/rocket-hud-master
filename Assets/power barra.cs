using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ProgressBarLooper : MonoBehaviour
{
    public GameObject maxPowerObj;
    public GameObject timeObj;
    public GameObject powerObj;
    public GameObject StartObj;
    public Image progressBarImage;
    private bool status;
    private float MAXWEIGHT;
    private float _maxWeightCurrent;
    private System.Diagnostics.Stopwatch stopwatch;
    private long tempo;
    private NumberCounter time;
    private IntegerCounterDisplay power;
    private CounterText maxPower;
    private ToggleVisibility start;
    private WebSocketHandler _webSocketHandler;
    private DataBase _database;
    private DataBase.DataTeste _dataTeste;

    void Awake()
    {
        status = false;
        MAXWEIGHT = 6000;
        _maxWeightCurrent = 0;

        stopwatch = new();
        stopwatch.Start();

        maxPower = maxPowerObj.GetComponent<CounterText>();
        time = timeObj.GetComponent<NumberCounter>();
        power = powerObj.GetComponent<IntegerCounterDisplay>();

        _dataTeste = new();
        _dataTeste.Registros = new();

        if (maxPower == null || time == null || power == null)
        {
            Debug.LogError("Um ou mais componentes estão faltando nos objetos!");
        }

        _webSocketHandler = new WebSocketHandler("ws://0.0.0.0:15000");
        _webSocketHandler.OnMessageReceived += HandleWebSocketMessage;
        _webSocketHandler.Start();

        _database = new DataBase("mongodb+srv://gustavo:OUyY3FScSj39zzAA@cluster0.5nb5xr6.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0"
                                , "rocket"
                                , "teste");
    }

    private void HandleWebSocketMessage(WebSocketHandler.DataPeso data)
    {
        MainThreadDispatcher.Instance().Enqueue(() =>
        {
            StartCoroutine(AnimateProgressBar(data));
        });
    }

    private IEnumerator AnimateProgressBar(WebSocketHandler.DataPeso data)
    {
        if (data == null) yield return null;

        string cronomentro = ContagemTempo(data.Ativo, data.Tempo, data.Peso);

        if (!string.IsNullOrEmpty(cronomentro)) time.uiText.text = cronomentro;
        maxPower.uiText.text = ReturnMaxWeight(data.Peso).ToString("F0");
        progressBarImage.fillAmount = Mathf.Lerp(0f, 1f, data.Peso / MAXWEIGHT);
        power.displayText.text = data.Peso.ToString("F0");

        yield return new WaitForSeconds(0.01f);
    }

    private string ContagemTempo(bool active, long tempoEsp, float peso)
    {
        switch (status)
        {
            case false when active: // -> passa do status de false para true, comeca a contar o tempo
                tempo = tempoEsp;
                break;
            case true when active: // -> passa do status de true para true, continua a contar o tempo
                long tempoDecorrido = tempoEsp - tempo;

                // _dataTeste.Registros.Add((tempoDecorrido, peso));
                // _dataTeste.Tempo = tempoDecorrido;

                return FormatTempo(tempoDecorrido);
            case true when !active: // -> passa do status de true para false, termina de contar o tempo
                tempo = 0;
                // if (start.isHidden) // caso o botão tenha sido ativo(isHidden = true), então comece a colocar os dados no banco
                // {
                //     EnvioParaOBanco();
                // } 
                _dataTeste = new();
                _dataTeste.Registros = new();
                break;
        }

        status = active;
        return FormatTempo(tempo);
    }

    private string FormatTempo(long tempo)
    {
        if (tempo == 0) return "";
        TimeSpan timeSpan = TimeSpan.FromMilliseconds(tempo);
        int seconds = timeSpan.Seconds;
        int centiseconds = timeSpan.Milliseconds / 10;

        return $"{seconds:D2}:{centiseconds:D2}";

        // return tempo.ToString();

    }

    private float ReturnMaxWeight(float weight)
    {
        if (_maxWeightCurrent < weight)
        {
            _maxWeightCurrent = weight;
            _dataTeste.PesoMaximo = _maxWeightCurrent;
        }
        return _maxWeightCurrent;
    }

    private void EnvioParaOBanco()
    {

        _database.InsertDocument(_dataTeste);

        _dataTeste = new();
    }

}
