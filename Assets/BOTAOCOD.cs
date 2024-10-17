using UnityEngine;
using UnityEngine.UI;

public class ToggleVisibility : MonoBehaviour
{
    public GameObject warningImage; // Arraste a imagem de aviso aqui
    public Button startButton; // Arraste o botão de start aqui
    public Button menuButton; // Arraste o botão de menu aqui

    public bool isHidden = false;

    void Start()
    {
        // Adiciona um listener ao botão para chamar o método HideWarning quando for clicado
        startButton.onClick.AddListener(HideWarning);
    }

    void HideWarning()
    {
        // Esconde a imagem de aviso, o botão de start e o botão de menu
        warningImage.SetActive(false);
        startButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        isHidden = true;
    }
}
