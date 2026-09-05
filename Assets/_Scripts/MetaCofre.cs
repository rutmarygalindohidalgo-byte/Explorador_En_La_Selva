using System.Collections;
using UnityEngine;

public class MetaCofre : MonoBehaviour
{
    [Header("Ajustes de Victoria")]
    // Distancia a la que debe llegar Alex para que el cofre se active solo
    public float distanciaApertura = 3.5f;

    [Header("Referencias de la Escena")]
    // Objeto del jugador (se detecta solo, pero puedes arrastrarlo)
    public Transform jugador;

    // Panel o pantalla de 'Nivel Completado' que se activará al ganar
    public GameObject panelVictoria;

    // Tapa del cofre (funciona como respaldo por si el cofre no tuviera Animator)
    public Transform piezaTapa;

    [Header("Efectos y Sonido")]
    // Sonido o fanfarria de victoria
    public AudioClip sonidoVictoria;

    // Variables internas para controlar el flujo
    private bool nivelCompletado = false;
    private Animator animCofre;

    void Start()
    {
        // 1. Obtener el Animator del cofre para usar sus animaciones
        animCofre = GetComponent<Animator>();
        if (animCofre == null)
        {
            animCofre = GetComponentInChildren<Animator>();
        }

        // 2. Buscar al jugador automáticamente si la casilla quedó vacía
        if (jugador == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                jugador = playerObj.transform;
            }
            else
            {
                ControladorJugador control = FindFirstObjectByType<ControladorJugador>();
                if (control != null) jugador = control.transform;
            }
        }

        // 3. Buscar la tapa automáticamente como método alternativo
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

        // 4. Asegurar que la pantalla de victoria empiece oculta
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(false);
        }
    }

    void Update()
    {
        // Si el cofre ya se abrió o no encuentra al jugador, no hace nada
        if (nivelCompletado || jugador == null) return;

        // Calcular la distancia horizontal ignorando diferencias de altura
        Vector3 posCofreH = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 posJugadorH = new Vector3(jugador.position.x, 0, jugador.position.z);
        float distancia = Vector3.Distance(posCofreH, posJugadorH);

        // Si Alex entra al rango, se activa la apertura automáticamente
        if (distancia <= distanciaApertura)
        {
            StartCoroutine(SecuenciaAperturaVictoria());
        }
    }

    // Corrutina que maneja los tiempos: salto -> abrir tapa -> cartel de victoria
    private IEnumerator SecuenciaAperturaVictoria()
    {
        nivelCompletado = true;
        Debug.Log("¡NIVEL COMPLETADO! ¡Has alcanzado el cofre del tesoro!");

        // 1. Frena a Alex y devuelve sus animaciones a reposo (Idle)
        DetenerJugador();

        // 2. Desactiva al oso para que no te ataque tras ganar
        DesactivarEnemigos();

        // 3. El cofre da un salto de emoción (animación Bounce)
        if (animCofre != null)
        {
            animCofre.Play("Bounce", 0, 0f);
        }

        // Pequeña espera mientras termina el brinco
        yield return new WaitForSeconds(0.4f);

        // 4. Abre la tapa con la animación
        if (animCofre != null)
        {
            animCofre.Play("Open", 0, 0f);
        }
        else if (piezaTapa != null)
        {
            // Respaldo manual: rota la pieza si no hay animaciones configuradas
            piezaTapa.Rotate(-85f, 0, 0, Space.Self);
        }

        // 5. Reproduce el sonido de recompensa
        if (sonidoVictoria != null)
        {
            AudioSource.PlayClipAtPoint(sonidoVictoria, transform.position);
        }

        // Espera medio segundo para ver el cofre abierto antes de tapar la pantalla
        yield return new WaitForSeconds(0.6f);

        // 6. Activa el cartel de victoria en pantalla
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
        }
    }

    // Detiene el movimiento físico Y regresa la animación a Idle (quieto)
    private void DetenerJugador()
    {
        if (jugador == null) return;

        // Desactiva el CharacterController para frenar la física
        CharacterController cc = jugador.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // Desactiva el script que lee las teclas de caminar
        ControladorJugador cj = jugador.GetComponent<ControladorJugador>();
        if (cj != null) cj.enabled = false;

        // Busca el Animator de Alex y pone todas las variables de movimiento a 0
        Animator animAlex = jugador.GetComponent<Animator>();
        if (animAlex == null) animAlex = jugador.GetComponentInChildren<Animator>();

        if (animAlex != null)
        {
            foreach (AnimatorControllerParameter param in animAlex.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Float)
                {
                    animAlex.SetFloat(param.name, 0f);
                }
                else if (param.type == AnimatorControllerParameterType.Bool)
                {
                    animAlex.SetBool(param.name, false);
                }
            }
        }
    }

    // Busca a los osos en la escena, apaga su script de ataque y los deja en reposo
    private void DesactivarEnemigos()
    {
        OsoEnemigo[] osos = FindObjectsByType<OsoEnemigo>(FindObjectsSortMode.None);
        foreach (OsoEnemigo oso in osos)
        {
            // Apaga el script del oso para que deje de perseguir y golpear
            oso.enabled = false;

            // Pasa su animación a descanso (Idle)
            Animator animOso = oso.GetComponent<Animator>();
            if (animOso != null)
            {
                animOso.Play("Bear_Idle", 0, 0f);
            }
        }
    }

    // Dibuja una esfera amarilla en la vista Scene para ver el radio de activación
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaApertura);
    }
}