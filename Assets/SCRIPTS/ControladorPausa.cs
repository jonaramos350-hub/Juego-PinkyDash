using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorPausa : MonoBehaviour
{
    [Header("Configuracion de UI")]
    public GameObject objetoMenuPausa; // Arrastra aqui tu Panel de Pausa

    private bool estaPausado = false;

    // --- ESTO ES LO QUE TE FALTABA ---
    void Start()
    {
        // Asegura que el menú esté APAGADO apenas el nivel cargue
        if (objetoMenuPausa != null)
        {
            objetoMenuPausa.SetActive(false);
            Time.timeScale = 1f; // Asegura que el juego NO empiece congelado
            estaPausado = false;
        }
    }
    // ---------------------------------

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (estaPausado) Reanudar();
            else Pausar();
        }
    }

    public void Pausar()
    {
        estaPausado = true;
        Time.timeScale = 0f;
        objetoMenuPausa.SetActive(true);
        Debug.Log("Juego Pausado");
    }

    public void Reanudar()
    {
        estaPausado = false;
        Time.timeScale = 1f;
        objetoMenuPausa.SetActive(false);
        Debug.Log("Juego Reanudado");
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        // OJO: Dijiste que tu nivel se llama "mainmenu", 
        // pon aquí el nombre de la escena del BOTÓN JUGAR.
        SceneManager.LoadScene("Nivel 1");
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}