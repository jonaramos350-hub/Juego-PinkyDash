using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorFlujo : MonoBehaviour
{
    // Variables estáticas obligatorias para evitar errores CS0117
    public static int indiceObjetivo = 0;
    public static string escenaAObjetivo = "";
    public static string nombreEscenaSiguiente = "";

    void Start()
    {
        // Si estamos en la Pantalla de Carga (Rosa)
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Invoke("CargarNivelReal", 2f); // Espera 2 segundos antes de saltar
        }
    }

    void CargarNivelReal()
    {
        if (indiceObjetivo != 0)
            SceneManager.LoadScene(indiceObjetivo);
        else if (!string.IsNullOrEmpty(escenaAObjetivo))
            SceneManager.LoadScene(escenaAObjetivo);
        else
            SceneManager.LoadScene(0); // Regresa al Menú por seguridad
    }
}