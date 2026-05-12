using System.Collections;
using UnityEngine;

public class ControlChocolate : MonoBehaviour
{
    [Header("Configuracion de Persecucion")]
    public float velocidadPersecucion = 7f;

    [Header("Zona de Deteccion (Ojos)")]
    // Este rectangulo es mas robusto que un Raycast
    public Vector2 tamañoZonaVision = new Vector2(10f, 2f);
    public LayerMask capaJugador; // Asegurate de seleccionar SOLO la layer del jugador

    [Header("Sensores")]
    public Transform sensorSuelo;
    public LayerMask capaSuelo;

    private Rigidbody2D rb;
    private bool mirandoDerecha = false;
    private bool persiguiendo = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.freezeRotation = true;

        // Orientacion inicial (izquierda)
        Vector3 escala = transform.localScale;
        escala.x = -Mathf.Abs(escala.x);
        transform.localScale = escala;
    }

    void FixedUpdate()
    {
        DetectarJugadorConZona(); // ¡Nuevo metodo mas preciso!

        if (persiguiendo)
        {
            Mover();
            RevisarSuelo();
        }
        else
        {
            // Si no persigue, se queda quieto
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    void DetectarJugadorConZona()
    {
        // Calculamos la posicion central de la zona de vision al frente del chocolate
        float dirMultiplier = mirandoDerecha ? 1 : -1;
        Vector2 centroZona = (Vector2)transform.position + new Vector2(dirMultiplier * (tamañoZonaVision.x / 2), 0);

        // Creamos un rectangulo invisible y revisamos si Porky esta adentro
        Collider2D porkyDetectado = Physics2D.OverlapBox(centroZona, tamañoZonaVision, 0f, capaJugador);

        if (porkyDetectado != null && porkyDetectado.CompareTag("Player"))
        {
            // ¡Te vio con toda el area de la zona! Es mucho mas fluido.
            persiguiendo = true;
        }
        else
        {
            persiguiendo = false;
        }
    }

    void Mover()
    {
        float dir = mirandoDerecha ? 1 : -1;
        rb.linearVelocity = new Vector2(dir * velocidadPersecucion, rb.linearVelocity.y);
    }

    void RevisarSuelo()
    {
        if (sensorSuelo == null) return;

        // Si se acaba el suelo, se detiene
        RaycastHit2D suelo = Physics2D.Raycast(sensorSuelo.position, Vector2.down, 1.5f, capaSuelo);

        if (suelo.collider == null)
        {
            persiguiendo = false;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var scriptPinky = collision.gameObject.GetComponent<NewMonoBehaviourScript>();
            if (scriptPinky != null) scriptPinky.TomarDanio();
        }
    }

    private void OnDrawGizmos()
    {
        // Dibujamos la ZONA de vision (el rectangulo) para que la veas en la escena
        Gizmos.color = Color.yellow; // Cambie el color a amarillo por claridad

        float dirMultiplier = mirandoDerecha ? 1 : -1;
        Vector2 centroZona = (Vector2)transform.position + new Vector2(dirMultiplier * (tamañoZonaVision.x / 2), 0);

        Gizmos.DrawWireCube(centroZona, tamañoZonaVision);

        // Dibujamos el sensor de suelo (azul)
        if (sensorSuelo != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(sensorSuelo.position, sensorSuelo.position + Vector3.down * 1.5f);
        }
    }
}