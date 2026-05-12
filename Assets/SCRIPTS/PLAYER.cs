using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Configuración de Control Directo")]
    public float speed = 6;
    public float jumpForce = 8;

    [Range(0, 1)]
    public float friccionAlSoltar = 0.8f;

    private Rigidbody2D rb2D;
    private float move;

    [Header("Detección de Suelo")]
    public bool isGrounded;
    public Transform groundCheck;
    public float groundRadius = 0.3f;
    public LayerMask groundLayer;

    [Header("Componentes y UI")]
    private Animator animator;
    private int coins;
    public TMP_Text textCoins;
    private Vector3 escalaOriginal;

    [Header("Sistema de Vidas")]
    public int vidas = 3;
    public Image[] corazones;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip sonidoMoneda;
    public AudioClip sonidoMeta;
    public AudioClip sonidoDanio;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        escalaOriginal = transform.localScale;
        audioSource = GetComponent<AudioSource>();

        // En Unity 6 usamos linearDamping
        rb2D.linearDamping = 0;

        ActualizarInterfazVidas();
    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");

        if (move != 0)
        {
            rb2D.linearVelocity = new Vector2(move * speed, rb2D.linearVelocity.y);
            transform.localScale = new Vector3(Mathf.Sign(move) * escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);
        }
        else
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x * friccionAlSoltar, rb2D.linearVelocity.y);
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb2D.linearVelocity.x));
            animator.SetFloat("VerticalVelocity", rb2D.linearVelocity.y);
            animator.SetBool("IsGrounded", isGrounded);
        }

        if (isGrounded && (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            coins++;
            if (textCoins != null) textCoins.text = coins.ToString("00");
            if (audioSource != null && sonidoMoneda != null) audioSource.PlayOneShot(sonidoMoneda);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Picos")) TomarDanio();

        if (collision.CompareTag("Meta")) StartCoroutine(GanarNivelConEspera());
    }

    public void TomarDanio()
    {
        vidas--;
        if (audioSource != null && sonidoDanio != null) audioSource.PlayOneShot(sonidoDanio);

        ActualizarInterfazVidas();

        if (vidas <= 0) Morir();
    }

    public void ActualizarInterfazVidas()
    {
        if (corazones == null) return;
        for (int i = 0; i < corazones.Length; i++)
        {
            if (corazones[i] != null)
                corazones[i].enabled = (i < vidas);
        }
    }

    IEnumerator GanarNivelConEspera()
    {
        // 1. Sonido
        if (audioSource != null && sonidoMeta != null)
            audioSource.PlayOneShot(sonidoMeta);

        // 2. CONGELAR A PINKY
        this.enabled = false;
        rb2D.linearVelocity = Vector2.zero;
        rb2D.bodyType = RigidbodyType2D.Static; // Esto lo clava al piso

        // 3. ZOOM DE CÁMARA
        float duracionZoom = 3.0f;
        float zoomFinal = 3.0f; // Más bajo es más cerca
        float zoomInicial = Camera.main.orthographicSize;
        float tiempo = 0;

        while (tiempo < duracionZoom)
        {
            tiempo += Time.deltaTime;
            Camera.main.orthographicSize = Mathf.Lerp(zoomInicial, zoomFinal, tiempo / duracionZoom);
            yield return null;
        }

        // 4. ESPERA FINAL (Para que termine la canción)
        yield return new WaitForSeconds(3.0f);

        // 5. CAMBIO DE ESCENA
        SceneManager.LoadScene("EscenaCreditos");
    }

    public void Morir()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}