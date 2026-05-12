using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 2f;
    public NewMonoBehaviourScript jugador;
    public float margenIzquierdo = 15f;

    [Header("Configuración de Meta y Zoom")]
    public float zoomObjetivo = 3f; // Qué tan cerca quieres la cámara al ganar
    public float velocidadZoom = 2f;
    public float velocidadCentrado = 3f;

    private Camera cam;
    private Transform objetivoMeta;
    private bool haComenzado = false;
    private bool haciendoZoom = false;

    void Start()
    {
        cam = GetComponent<Camera>();
        // Busca al jugador automáticamente si no lo arrastraste
        if (jugador == null) jugador = FindObjectOfType<NewMonoBehaviourScript>();
    }

    void Update()
    {
        // SI YA GANAMOS: Deja de avanzar y hace zoom
        if (haciendoZoom)
        {
            // 1. Zoom suave
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomObjetivo, Time.deltaTime * velocidadZoom);

            // 2. Centrar en la meta
            if (objetivoMeta != null)
            {
                Vector3 posicionDeseada = new Vector3(objetivoMeta.position.x, objetivoMeta.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, posicionDeseada, Time.deltaTime * velocidadCentrado);
            }
            return; // IMPORTANTE: Esto evita que la cámara siga avanzando a la derecha
        }

        // MOVIMIENTO AUTOMÁTICO (AUTOSCROLL)
        if (!haComenzado && Input.GetAxisRaw("Horizontal") != 0) haComenzado = true;

        if (haComenzado)
        {
            // Avanza a la derecha solo
            float nuevaX = transform.position.x + (velocidad * Time.deltaTime);
            transform.position = new Vector3(nuevaX, transform.position.y, transform.position.z);

            // Si Porky se queda atrás, muere
            if (jugador != null && jugador.transform.position.x < transform.position.x - margenIzquierdo)
            {
                jugador.Morir();
            }
        }
    }

    // Esta función la llama el script de la Meta para activar el zoom
    public void ActivarEnfoqueFinal(Transform posicionMeta)
    {
        objetivoMeta = posicionMeta;
        haciendoZoom = true;
    }
}