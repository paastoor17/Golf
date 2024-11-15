using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public TMP_Text tirosText;  // Para mostrar el total de tiros
    public Button startButton;  // Botón para iniciar el juego

    public static int totalTiros = 0;  // Variable estática para almacenar el número total de tiros de todos los mapas

    void Start()
    {
        // Actualizar el texto de los tiros en el menú principal
        UpdateTirosText();

        // Asignar el evento para iniciar el juego
        startButton.onClick.AddListener(StartGame);
    }

    // Método para iniciar el juego y cargar la primera escena
    void StartGame()
    {
        totalTiros = 0;  // Reiniciar el contador al iniciar el juego
        SceneManager.LoadScene("Map1");  // Asegúrate de que "Map1" esté en Build Settings
    }

    // Actualizar el texto con el total de tiros acumulados
    public void UpdateTirosText()
    {
        tirosText.text = "Tiros Totales: " + totalTiros.ToString();
    }
}
