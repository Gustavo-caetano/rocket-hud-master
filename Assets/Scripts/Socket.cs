using System;
using Fleck;
using UnityEngine;

public class WebSocketHandler
{
    private WebSocketServer _server;
    private IWebSocketConnection _client;
    private bool _conectado;
    public event Action<DataPeso> OnMessageReceived;

    public WebSocketHandler(string address)
    {
        _server = new WebSocketServer(address);
    }

    public void Start()
    {
        _server.Start(ws =>
        {
            ws.OnOpen = () =>
            {
                _conectado = true;
                Debug.Log("Conexão WebSocket estabelecida: "+ _conectado);

                _client = ws;
            };

            ws.OnMessage = (msg) =>
            {
                Debug.Log("Mensagem recebida no WebSocket");

                DataPeso data = JsonUtility.FromJson<DataPeso>(msg);

                OnMessageReceived?.Invoke(data);
            };

            ws.OnClose = () =>
            {
                _conectado = false;
                Debug.Log("Conexão WebSocket fechada" + _conectado);
            };

            ws.OnError = (ex) =>
            {
                Debug.LogError($"Erro no WebSocket: {ex.Message}");
            };
        });
    }
    public void SendMsg(string json)
    {
        if(_client == null) return;

        _client.Send(json);
    }
    public bool IsConnected()
    {
        return _conectado;
    }

    [Serializable]
    public class DataPeso
    {
        public long Tempo;
        public float Peso;
        public bool Ativo;
    }
}
