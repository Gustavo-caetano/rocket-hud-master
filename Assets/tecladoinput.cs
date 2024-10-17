using UnityEngine;

public class EscapeKeyController : MonoBehaviour
{
    public GameObject warningImage; // Arraste aqui a imagem de aviso no Inspector
    public GameObject startButton; // Arraste aqui o botão de start no Inspector
    public GameObject menuButton; // Arraste aqui o botão de menu no Inspector

    private bool isHidden = false;

    void Update()
    {
        // Verifica se a tecla ESC foi pressionada
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Tecla ESC pressionada");

            // Se os objetos estiverem visíveis, não faz nada
            if (warningImage.activeSelf && startButton.activeSelf && menuButton.activeSelf)
            {
                Debug.Log("Os objetos já estão visíveis. Não controlando visibilidade.");
            }
            else
            {
                // Se não estiverem visíveis, torna-os visíveis
                ShowObjects();
            }
        }
    }

    void ShowObjects()
    {
        Debug.Log("Mostrando imagem de aviso, botão de start e botão de menu");
        warningImage.SetActive(true);
        startButton.SetActive(true);
        menuButton.SetActive(true);
        isHidden = false;
    }
}
