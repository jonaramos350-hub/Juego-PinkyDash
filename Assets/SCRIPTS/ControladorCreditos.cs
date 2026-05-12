using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorCreditos : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 40f; // Más bajo = más lento
    public float puntoFinalY = 2000f; // Altura donde se detiene

    void Update()
    {
        // Movimiento constante hacia arriba
        transform.Translate(Vector3.up * velocidad * Time.deltaTime);

        // Si presionas cualquier tecla o llega al punto final
        if (Input.anyKeyDown || transform.localPosition.y >= puntoFinalY)
        {
            RegresarAlMenu();
        }
    }

    void RegresarAlMenu()
    {
        // Usamos tu sistema para que pase por la pantalla rosa
        ControladorFlujo.indiceObjetivo = 0; // 0 suele ser el Menú Principal
        SceneManager.LoadScene(1); // 1 es tu pantalla de carga rosa
    }
}