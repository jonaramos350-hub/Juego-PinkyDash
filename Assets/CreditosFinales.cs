using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditosFinales : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public RectTransform rectContenido; // Arrastra aquí el objeto con los textos
    public float velocidad = 45f; // Ajusta esto para que sea más lento
    public float puntoFinalY = 1600f; // Altura donde desaparecen

    [Header("Destino Final")]
    public int indiceMenu = 0;

    private bool yaTermino = false;

    void Update()
    {
        if (!yaTermino)
        {
            // Movimiento constante hacia arriba
            rectContenido.anchoredPosition += new Vector2(0, velocidad * Time.deltaTime);

            if (rectContenido.anchoredPosition.y >= puntoFinalY)
            {
                yaTermino = true;
                Regresar();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
            Regresar();
    }

    void Regresar()
    {
        ControladorFlujo.indiceObjetivo = indiceMenu;
        SceneManager.LoadScene(1);
    }
}
