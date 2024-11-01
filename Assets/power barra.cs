using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ProgressBarLooper : MonoBehaviour
{
    public GameObject maxPowerObj;
    public GameObject timeObj;
    public GameObject powerObj;
    public GameObject cronometroObj;
    public GameObject botaoStartObj;
    public Image progressBarImage;

    private NumberCounter time;
    private IntegerCounterDisplay power;
    private CounterText maxPower;

    private WebSocketHandler _webSocketHandler;

    private WeightHandler _weightHandler;
    private Cronometro _cronometro;
    private ToggleVisibility _botaoStart;
    private CronometroScript _contagemRegressiva;
    private DataBase banco;
    private DataBase.DataTeste dadosTeste;
    void Awake()
    {
        maxPower = maxPowerObj.GetComponent<CounterText>();
        time = timeObj.GetComponent<NumberCounter>();
        power = powerObj.GetComponent<IntegerCounterDisplay>();

        _contagemRegressiva = cronometroObj.GetComponent<CronometroScript>();
        _botaoStart = botaoStartObj.GetComponent<ToggleVisibility>();
        cronometroObj.SetActive(false);
        if (maxPower == null || time == null || power == null || _contagemRegressiva == null || _botaoStart == null)
        {
            Debug.LogError("Um ou mais componentes estão faltando nos objetos!");
        }

        _botaoStart.startButton.onClick.AddListener(HideWarning);

        _weightHandler = new WeightHandler(6000); // Peso máximo definido aqui
        _cronometro = new Cronometro();

        _webSocketHandler = new WebSocketHandler("ws://0.0.0.0:15000");
        _webSocketHandler.OnMessageReceived += HandleWebSocketMessage;
        _webSocketHandler.Start();

        banco = new(
            "mongodb+srv://gustavo:OUyY3FScSj39zzAA@cluster0.5nb5xr6.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0"
            , "rocket"
            , "teste");
    }
    void Start()
    {
        cronometroObj.SetActive(false);
    }
    void Update()
    {
        if (!Input.anyKeyDown) return;

        var comando = new ComandoEsp();
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            comando.Opcao = 2;
            SendCommand(comando);
            return;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            ModalWindow<InputModalWindow>.Create()
                   .SetHeader("Calibração de balança")
                   .SetBody("Informe o valor que deve ser incrementado ao valor de calibração da balança\nvalor padrão: 277550\n")
                   .SetInputField((inputResult) =>
                   {
                       if (float.TryParse(inputResult, out float val))
                       {
                           comando.Opcao = 1;
                           comando.Valor = inputResult;
                           SendCommand(comando);
                       }
                   })
                   .Show();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            _weightHandler.SetMaxWeight(0);
            time.uiText.text = "00:00";
            return;
        }


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

        (string cronomentro, long tempoDecorrido) = _cronometro.ContagemTempo(data.Ativo, data.Tempo);

        if( _botaoStart.warningImage.activeSelf)
        {
            UpdateDadosDeTeste(tempoDecorrido, data);    
        }
        if (!string.IsNullOrEmpty(cronomentro))
        {
            time.uiText.text = cronomentro;
        }
        maxPower.uiText.text = _weightHandler.ReturnMaxWeight(data.Peso).ToString("F0");
        progressBarImage.fillAmount = _weightHandler.CalculateFillAmount(data.Peso);
        power.displayText.text = data.Peso.ToString("F0");

        yield return new WaitForSeconds(0.01f);
    }

    void HideWarning()
    {
        _botaoStart.warningImage.SetActive(false);
        _botaoStart.startButton.gameObject.SetActive(false);
        _botaoStart.menuButton.gameObject.SetActive(false);

        cronometroObj.SetActive(true);
        _botaoStart.isHidden = true;

        _botaoStart.stopwatch.Reset();
        _botaoStart.stopwatch.Start();
        _botaoStart.remainingMs = 10000;
        _botaoStart.durationMs = _botaoStart.remainingMs;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (_botaoStart.remainingMs > 0 && cronometroObj.activeSelf)
        {
            _botaoStart.remainingMs = _botaoStart.durationMs - _botaoStart.stopwatch.ElapsedMilliseconds;

            if (_botaoStart.remainingMs > 0)
            {
                int seconds = Mathf.FloorToInt(_botaoStart.remainingMs % 60000 / 1000);
                int centiseconds = Mathf.FloorToInt(_botaoStart.remainingMs % 1000 / 10);
                string textoTempo = $"{seconds:D2}:{centiseconds:D2}";
                _contagemRegressiva.cronometroTexto.text = textoTempo;
            }
            else
            {
                _contagemRegressiva.cronometroTexto.text = "00:00";
                cronometroObj.SetActive(false);
            
                var comando = new ComandoEsp
                {
                    Opcao = 3
                };
                string json = JsonUtility.ToJson(comando);
                _webSocketHandler.SendMsg(json);
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    private void SendCommand(ComandoEsp comando)
    {
        string json = JsonUtility.ToJson(comando);
        Debug.Log(json);
        _webSocketHandler.SendMsg(json);
    }

    private void UpdateDadosDeTeste(long tempoDecorrido, WebSocketHandler.DataPeso data)
    {
        switch (tempoDecorrido)
        {
            case 1:
                dadosTeste = new()
                {
                    Registros = new()
                };
                break;
            case 2:
                dadosTeste.PesoMaximo = _weightHandler.ReturnMaxWeight();

                banco.InsertDocument(dadosTeste);
                break;
            case > 2:
                dadosTeste.Tempo = tempoDecorrido;
                dadosTeste.Registros.Add(new DataBase.Registro {
                    Tempo = tempoDecorrido,
                    Peso = data.Peso
                });
                break;
            default:
                break;
        }
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