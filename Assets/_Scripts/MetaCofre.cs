using UnityEngine;
using TMPro;

public class MetaCofre : MonoBehaviour
{
    [Header("Ajustes de Victoria")]
    public float distanciaInteraccion = 5.0f;
    public KeyCode teclaAbrir = KeyCode.Return;

    [Header("Referencias")]
    public Transform piezaTapa;      // Arrastra aquí 'Lid'
    public Transform jugador;        // Arrastra aquí a 'Player'
    public GameObject panelVictoria; // Arrastra aquí tu UI de Victoria

    private bool nivelCompletado = false;
    private bool abriendoTapa = false;
    private float anguloRotado = 0f;

    void Start()
    {
        // 1. Localizar a Remy
        if (jugador == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) jugador = playerObj.transform;
            else
            {
                ControladorJugador control = FindFirstObjectByType<ControladorJugador>();
                if (control != null) jugador = control.transform;
            }
        }

        // 2. Localizar la tapa 'Lid'
        if (piezaTapa == null)
        {
            foreach (Transform hijo in GetComponentsInChildren<Transform>())
            {
                if (hijo.name.ToLower().Contains("lid") || hijo.name.ToLower().Contains("tapa"))
                {
                    piezaTapa = hijo;
                    break;
                }
            }
        }

        // Apagar el Animator del cofre para rotar la tapa por código
        Animator anim = GetComponent<Animator>();
        if (anim != null) anim.enabled = false;

        // Ocultar panel de victoria al iniciar
        if (panelVictoria != null) panelVictoria.SetActive(false);
    }

    void Update()
    {
        // 1. Animación suave de apertura de la tapa
        if (abriendoTapa && piezaTapa != null && anguloRotado < 95f)
        {
            float paso = 160f * Time.deltaTime;
            anguloRotado += paso;
            piezaTapa.Rotate(-paso, 0, 0, Space.Self);
        }

        if (nivelCompletado || jugador == null) return;

        // 2. Medir distancia horizontal
        Vector3 posCofreH = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 posJugadorH = new Vector3(jugador.position.x, 0, jugador.position.z);
        float distancia = Vector3.Distance(posCofreH, posJugadorH);

        // 3. Apertura al presionar tecla o acercarse
        if (distancia <= distanciaInteraccion)
        {
            if (Input.GetKeyDown(teclaAbrir) || Input.GetKeyDown(KeyCode.KeypadEnter) ||
                Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || distancia <= 2.0f)
            {
                CompletarNivel();
            }
        }
    }

    private void CompletarNivel()
    {
        if (nivelCompletado) return;
        nivelCompletado = true;
        abriendoTapa = true;

        Debug.Log("¡NIVEL COMPLETADO! ¡Has abierto el cofre del tesoro!");

        // Inmovilizar a Remy por completo
        if (jugador != null)
        {
            // Apagar scripts de movimiento en Remy
            MonoBehaviour[] scripts = jugador.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour s in scripts)
            {
                if (s != this) s.enabled = false;
            }

            // Desactivar CharacterController
            CharacterController cc = jugador.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            // Resetear animaciones a estado quieto (Idle)
            Animator animJugador = jugador.GetComponent<Animator>();
            if (animJugador != null)
            {
                animJugador.SetFloat("Speed", 0f);
                animJugador.SetFloat("Vert", 0f);
            }
        }

        // Mostrar únicamente tu UI configurada
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanciaInteraccion);
    }
}