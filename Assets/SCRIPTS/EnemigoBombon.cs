using UnityEngine;
using System.Collections;

public class EnemigoBombon : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadPatrulla = 2f;
    public float velocidadCarga = 10f;
    public float distanciaDeteccion = 6f;

    [Header("Sensores y Capas")]
    public Transform sensorFrente;
    public LayerMask capaJugador;
    public LayerMask capaSuelo;

    private Rigidbody2D rb2D;
    private bool cargando = false;
    private int direccion = 1; // 1 derecha, -1 izquierda
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Evita que el bombón rote si choca
        rb2D.freezeRotation = true;
    }

    void Update()
    {
        if (!cargando)
        {
            Patrullar();
            DetectarJugador();
        }
    }

    void Patrullar()
    {
        // Se mueve en la dirección actual
        rb2D.linearVelocity = new Vector2(direccion * velocidadPatrulla, rb2D.linearVelocity.y);

        // LANZAR RAYO PARA SUELO: Detecta si NO hay suelo frente a él
        RaycastHit2D haySuelo = Physics2D.Raycast(sensorFrente.position, Vector2.down, 1f, capaSuelo);

        // LANZAR RAYO PARA PARED: Detecta si hay una pared enfrente
        RaycastHit2D hayPared = Physics2D.Raycast(sensorFrente.position, Vector2.right * direccion, 0.2f, capaSuelo);

        // Si se acaba el suelo O toca una pared, gira
        if (haySuelo.collider == null || hayPared.collider == true)
        {
            Girar();
        }
    }

    void DetectarJugador()
    {
        // Lanza un rayo invisible hacia adelante para ver si Pinky está cerca
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * direccion, distanciaDeteccion, capaJugador);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            StartCoroutine(PrepararCarga());
        }
    }

    IEnumerator PrepararCarga()
    {
        cargando = true;
        rb2D.linearVelocity = Vector2.zero; // Se detiene

        // Cambia a color rojo para avisar que va a atacar
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.6f); // Tiempo de espera antes de la embestida

        // ¡EMBESTIDA!
        rb2D.AddForce(new Vector2(direccion * velocidadCarga, 0), ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.2f); // Tiempo que dura corriendo rápido

        spriteRenderer.color = Color.white; // Vuelve a la normalidad
        cargando = false;
    }

    void Girar()
    {
        direccion *= -1;
        // Gira visualmente al personaje
        transform.localScale = new Vector3(direccion, 1, 1);
    }

    // Si choca con Pinky, le quita vida
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Busca el script de Pinky y llama a TomarDanio
            var scriptPinky = collision.gameObject.GetComponent<NewMonoBehaviourScript>();
            if (scriptPinky != null)
            {
                scriptPinky.TomarDanio();
            }
        }
    }

    // Esto sirve para ver los rayos en la ventana de Escena (solo para ti)
    private void OnDrawGizmos()
    {
        if (sensorFrente != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(sensorFrente.position, sensorFrente.position + Vector3.down * 1f);
        }
    }
}
