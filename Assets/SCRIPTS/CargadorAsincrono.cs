using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CargadorAsincrono : MonoBehaviour
{
    public Slider barraDeCarga; // Asegúrate de arrastrar tu Slider aquí en Unity

    void Start()
    {
        // Prioridad al índice numérico que configuramos en los otros scripts
        if (ControladorFlujo.indiceObjetivo != 0)
        {
            StartCoroutine(CargarNivel(ControladorFlujo.indiceObjetivo));
        }
        else if (!string.IsNullOrEmpty(ControladorFlujo.escenaAObjetivo))
        {
            StartCoroutine(CargarNivelNombre(ControladorFlujo.escenaAObjetivo));
        }
    }

    IEnumerator CargarNivel(int indice)
    {
        AsyncOperation operacion = SceneManager.LoadSceneAsync(indice);
        while (!operacion.isDone)
        {
            // Solo actualiza si la barra existe para evitar el error NullReference
            if (barraDeCarga != null)
            {
                barraDeCarga.value = Mathf.Clamp01(operacion.progress / 0.9f);
            }
            yield return null;
        }
    }

    IEnumerator CargarNivelNombre(string nombre)
    {
        AsyncOperation operacion = SceneManager.LoadSceneAsync(nombre);
        while (!operacion.isDone)
        {
            if (barraDeCarga != null)
            {
                barraDeCarga.value = Mathf.Clamp01(operacion.progress / 0.9f);
            }
            yield return null;
        }
    }
}