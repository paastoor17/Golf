using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int golpesNivel = 0;  // Contador de golpes en el nivel actual

    // Método para registrar un golpe, se puede llamar cada vez que el jugador golpea la bola
    public void RegistrarGolpe()
    {
        golpesNivel++;
    }

    // Método para finalizar el nivel actual y avanzar al siguiente nivel
    public void FinalizarNivel()
    {
        // Sumar los golpes actuales al total de tiros en MainMenuController
        MainMenuController.totalTiros += golpesNivel;

        // Cargar la siguiente escena
        int siguienteEscenaIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (siguienteEscenaIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(siguienteEscenaIndex);
        }
        else
        {
            // Si no hay más niveles, regresa al menú principal
            SceneManager.LoadScene("MainMenu");
        }
    }
}
