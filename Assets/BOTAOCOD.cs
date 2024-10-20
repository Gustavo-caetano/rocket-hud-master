using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToggleVisibility : MonoBehaviour
{
    public GameObject warningImage; // Arraste a imagem de aviso aqui
    public GameObject cronometro;
    public Button startButton; // Arraste o botão de start aqui
    public Button menuButton; // Arraste o botão de menu aqui
    public bool isHidden = false;

    public Stopwatch stopwatch;
    public long durationMs = 10000; // 1 minuto em milissegundos
    public long remainingMs;

    void Start()
    {
        stopwatch = new();
        remainingMs = durationMs;
    }
}
