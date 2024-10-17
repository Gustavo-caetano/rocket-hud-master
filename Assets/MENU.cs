using UnityEngine;
using UnityEngine.UI;

public class MenuAndSubmenuController : MonoBehaviour
{
    public GameObject menuImage; // Arraste a UI Image do menu principal aqui
    public GameObject submenuImage; // Arraste a UI Image do submenu aqui
    public Button menuButton; // Arraste o botão do menu principal que abre o menu
    public Button submenuButton; // Arraste o botão do menu que abre o submenu

    private bool isMenuOpen = false;
    private bool isSubmenuOpen = false;

    void Start()
    {
        // Garante que ambos os menus estejam escondidos no início
        menuImage.SetActive(false);
        submenuImage.SetActive(false);

        // Adiciona listeners para os botões
        menuButton.onClick.AddListener(OpenMenu);
        submenuButton.onClick.AddListener(OpenSubmenu);
    }

    void Update()
    {
        // Verifica se a tecla ESC foi pressionada
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isSubmenuOpen)
            {
                CloseSubmenu();
            }
            else if (isMenuOpen)
            {
                CloseMenu();
            }
        }
    }

    void OpenMenu()
    {
        menuImage.SetActive(true);
        submenuImage.SetActive(false);
        isMenuOpen = true;
        isSubmenuOpen = false;
    }

    void OpenSubmenu()
    {
        menuImage.SetActive(false);
        submenuImage.SetActive(true);
        isMenuOpen = false;
        isSubmenuOpen = true;
    }

    void CloseSubmenu()
    {
        submenuImage.SetActive(false);
        menuImage.SetActive(true); // Retorna para o menu principal
        isMenuOpen = true;
        isSubmenuOpen = false;
    }

    void CloseMenu()
    {
        menuImage.SetActive(false);
        isMenuOpen = false;
    }
}
