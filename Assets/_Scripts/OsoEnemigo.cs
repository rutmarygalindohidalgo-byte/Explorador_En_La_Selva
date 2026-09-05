using UnityEngine;



public class OsoEnemigo : MonoBehaviour
{
    [Header("Ajustes de Detección de Larga Distancia")]
    public Transform jugador;
    public float rangoVision = 60f;
    public float velocidadPersecucion = 5.5f;
    public float distanciaAtaque = 2.5f;
    public float tiempoEntreAtaques = 1.5f;

    [Header("Audio de Ataque")]
    public AudioClip rugidoAtaque;
    private AudioSource audioSource;
    private bool haRugido = false;

    private float tiempoSiguienteAtaque = 0f;
    private Animator animator; // Control de animaciones del oso

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        // 1. Configurar audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.spatialBlend = 1f;

        // 2. Buscar al jugador automáticamente
        if (jugador == null)
        {
            GameObject pTag = GameObject.FindGameObjectWithTag("Player");
            if (pTag != null)
            {
                jugador = pTag.transform;
            }
            else
            {
                ControladorJugador control = FindFirstObjectByType<ControladorJugador>();
                if (control != null) jugador = control.transform;
            }
        }
    }

    void Update()
    {
        if (jugador == null) return;

        // Distancia horizontal hacia el jugador
        Vector3 posOsoHorizontal = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 posJugadorHorizontal = new Vector3(jugador.position.x, 0, jugador.position.z);
        float distancia = Vector3.Distance(posOsoHorizontal, posJugadorHorizontal);

        if (distancia <= rangoVision)
        {
            // Rugir al avistar
            if (!haRugido && rugidoAtaque != null)
            {
                audioSource.PlayOneShot(rugidoAtaque);
                haRugido = true;
            }

            // Mirar siempre hacia el jugador
            Vector3 objetivoMirar = new Vector3(jugador.position.x, transform.position.y, jugador.position.z);
            transform.LookAt(objetivoMirar);

            // Si está lejos, corre hacia él
            if (distancia > distanciaAtaque)
            {
                transform.position = Vector3.MoveTowards(transform.position, objetivoMirar, velocidadPersecucion * Time.deltaTime);
            }
            else
            {
                // Si llegó al rango de cuerpo a cuerpo, se planta y ataca con las garras
                if (Time.time >= tiempoSiguienteAtaque)
                {
                    AtacarJugador();
                    tiempoSiguienteAtaque = Time.time + tiempoEntreAtaques;
                }
            }
        }
        else
        {
            haRugido = false;
        }
    }

    private void AtacarJugador()
    {
        Debug.Log("¡El oso atacó con sus garras!");

        // Dispara la animación de zarpazo
        if (animator != null)
        {
            animator.Play("Bear_Attack1", 0, 0f);
        }

        // Descuenta la vida en el GameManager
        if (GameManager.instance != null)
        {
            GameManager.instance.PerderVida();
        }
        else
        {
            GameManager gm = FindFirstObjectByType<GameManager>();
            if (gm != null) gm.PerderVida();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoVision);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaAtaque);
    }
}