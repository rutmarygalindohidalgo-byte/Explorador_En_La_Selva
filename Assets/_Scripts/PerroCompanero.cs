using UnityEngine;

public class PerroCompanero : MonoBehaviour
{
    [Header("Ajustes de Seguimiento")]
    public Transform jugador;
    public float distanciaParada = 2.5f;
    public float velocidadSeguimiento = 3.5f;
    public float velocidadGiro = 8f;

    private Animator animator;
    private CharacterController characterController;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        if (jugador == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) jugador = p.transform;
            else
            {
                ControladorJugador c = FindFirstObjectByType<ControladorJugador>();
                if (c != null) jugador = c.transform;
            }
        }
    }

    void Update()
    {
        if (jugador == null) return;

        // Calcular distancia horizontal (sin contar la altura)
        Vector3 posPerroH = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 posJugadorH = new Vector3(jugador.position.x, 0, jugador.position.z);
        float distancia = Vector3.Distance(posPerroH, posJugadorH);

        if (distancia > distanciaParada)
        {
            // 1. Dirección horizontal para no inclinar el cuerpo
            Vector3 direccion = (jugador.position - transform.position);
            direccion.y = 0; // Mantiene el cuerpo horizontal

            if (direccion != Vector3.zero)
            {
                // Giro suave hacia Remy
                Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadGiro * Time.deltaTime);
            }

            // 2. Movimiento hacia adelante
            Vector3 movimiento = transform.forward * velocidadSeguimiento * Time.deltaTime;

            // Si tiene CharacterController lo usamos para respetar el suelo
            if (characterController != null && characterController.enabled)
            {
                movimiento.y = Physics.gravity.y * Time.deltaTime; // Aplicar gravedad
                characterController.Move(movimiento);
            }
            else
            {
                // Si no tiene controller, avanza directo manteniendo su altura Y actual
                transform.position += transform.forward * velocidadSeguimiento * Time.deltaTime;
            }

            // Animación de trote
            if (animator != null) animator.SetFloat("Vert", 1f);
        }
        else
        {
            // Animación de estar quieto (Idle)
            if (animator != null) animator.SetFloat("Vert", 0f);
        }
    }
}