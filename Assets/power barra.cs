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

    private NumberCounter time;
    private IntegerCounterDisplay power;
    private CounterText maxPower;

    private WebSocketHandler _webSocketHandler;

    private WeightHandler _weightHandler;
    private Cronometro _cronometro;

    void Awake()
    {
        maxPower = maxPowerObj.GetComponent<CounterText>();
        time = timeObj.GetComponent<NumberCounter>();
        power = powerObj.GetComponent<IntegerCounterDisplay>();

        if (maxPower == null || time == null || power == null)
        {
            Debug.LogError("Um ou mais componentes estão faltando nos objetos!");
        }

        _weightHandler = new WeightHandler(6000); // Peso máximo definido aqui
        _cronometro = new Cronometro();

        _webSocketHandler = new WebSocketHandler("ws://0.0.0.0:15000");
        _webSocketHandler.OnMessageReceived += HandleWebSocketMessage;
        _webSocketHandler.Start();
    }

    void Update()
    {
        if (!Input.anyKeyDown) return;

        var comando = new ComandoEsp();

        if (Input.GetKeyDown(KeyCode.T))
        {
            comando.Opcao = 2;
        }
        else
        {
            Debug.Log("Outra tecla pressionada");
            return;
        }

        string json = JsonUtility.ToJson(comando);
        Debug.Log(json);      
        _webSocketHandler.SendMsg(json);
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

        string cronomentro = _cronometro.ContagemTempo(data.Ativo, data.Tempo);

        if (!string.IsNullOrEmpty(cronomentro))
        {
            time.uiText.text = cronomentro;
        }

        maxPower.uiText.text = _weightHandler.ReturnMaxWeight(data.Peso).ToString("F0");
        progressBarImage.fillAmount = _weightHandler.CalculateFillAmount(data.Peso);
        power.displayText.text = data.Peso.ToString("F0");

        yield return new WaitForSeconds(0.01f);
    }
}
[Serializable]
public class ComandoEsp
{
    public int Opcao;
    public string Valor;

    public ComandoEsp()
    {
        Opcao = 0;
    }
}