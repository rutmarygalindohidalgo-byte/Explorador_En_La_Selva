using UnityEngine;

public class OsoEnemigo : MonoBehaviour
{
    [Header("Ajustes de Detección de Larga Distancia")]
    public Transform jugador;             // Asignado o detectado automáticamente
    public float rangoVision = 60f;       // Detecta a Remy desde muy lejos al pasar
    public float velocidadPersecucion = 5.5f;
    public float distanciaAtaque = 2.5f;   // Rango cuerpo a cuerpo
    public float tiempoEntreAtaques = 1.5f; // Cooldown para no vaciar vidas de golpe

    [Header("Audio de Ataque")]
    public AudioClip rugidoAtaque;
    private AudioSource audioSource;
    private bool haRugido = false;

    private float tiempoSiguienteAtaque = 0f;

    void Start()
    {
        // 1. Configurar audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.spatialBlend = 1f;

        // 2. Buscar al jugador automáticamente si no está asignado
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

        if (jugador == null)
        {
            Debug.LogError("¡ERROR OSO! Falta asignar al Jugador.");
        }
        else
        {
            Debug.Log("Oso listo. Detectando a: " + jugador.name);
        }
    }

    void Update()
    {
        if (jugador == null) return;

        // Medir distancia en el plano horizontal (ignora altura)
        Vector3 posOsoHorizontal = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 posJugadorHorizontal = new Vector3(jugador.position.x, 0, jugador.position.z);
        float distancia = Vector3.Distance(posOsoHorizontal, posJugadorHorizontal);

        // 1. Detección y persecución lejana
        if (distancia <= rangoVision)
        {
            // Rugir al avistar
            if (!haRugido && rugidoAtaque != null)
            {
                audioSource.PlayOneShot(rugidoAtaque);
                haRugido = true;
            }

            // Mirar hacia la posición horizontal de Remy
            Vector3 objetivoMirar = new Vector3(jugador.position.x, transform.position.y, jugador.position.z);
            transform.LookAt(objetivoMirar);

            // Perseguir a Remy
            transform.position = Vector3.MoveTowards(transform.position, objetivoMirar, velocidadPersecucion * Time.deltaTime);

            // 2. Daño cuerpo a cuerpo con recarga
            if (distancia <= distanciaAtaque && Time.time >= tiempoSiguienteAtaque)
            {
                AtacarJugador();
                tiempoSiguienteAtaque = Time.time + tiempoEntreAtaques;
            }
        }
        else
        {
            haRugido = false;
        }
    }

    private void AtacarJugador()
    {
        Debug.Log("¡El oso alcanzó a Remy y atacó!");
        if (GameManager.instance != null)
        {
            GameManager.instance.PerderVida();
        }
        else
        {
            Debug.LogWarning("No se encontró GameManager en la escena.");
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