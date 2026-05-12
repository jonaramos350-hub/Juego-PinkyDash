using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MetaNivel1 : MonoBehaviour
{
    public int numeroDeEscenaDestino;
    private bool yaGano = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica que sea Porky y que no hayamos ganado ya (para evitar ruidos repetidos)
        if (other.CompareTag("Player") && !yaGano)
        {
            yaGano = true;

            // 1. MANDAMOS LA SEÑAL A LA CÁMARA
            CameraFollow camara = Camera.main.GetComponent<CameraFollow>();
            if (camara != null)
            {
                camara.ActivarEnfoqueFinal(this.transform);
            }

            // 2. Tocamos el sonido
            if (GetComponent<AudioSource>() != null)
            {
                GetComponent<AudioSource>().Play();
            }

            // 3. Iniciamos la carga de la siguiente escena
            StartCoroutine(SecuenciaDeCarga());
        }
    }

    IEnumerator SecuenciaDeCarga()
    {
        // Esperamos 2.5 segundos para que se aprecie el zoom en la meta
        yield return new WaitForSeconds(2.5f);

        // Usamos tu sistema de ControladorFlujo para cargar la pantalla rosa
        ControladorFlujo.indiceObjetivo = numeroDeEscenaDestino;
        SceneManager.LoadScene(1);
    }
}