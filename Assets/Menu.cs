using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Función para el botón JUGAR
    public void PlayGame()
    {
        // 1. Decimos que vamos al VIDEO
        ControladorFlujo.escenaAObjetivo = "IntroVideo";
        // 2. Cargamos la pantalla rosa
        SceneManager.LoadScene("PantallaCarga");
    }

    // --- ESTO ES LO QUE FALTABA PARA TU BOTÓN DE SALIR ---
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit(); // Cierra el juego (solo funciona en el .exe final)
    }
}