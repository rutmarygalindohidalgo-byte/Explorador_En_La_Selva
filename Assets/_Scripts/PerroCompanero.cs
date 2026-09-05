using UnityEngine;

public class PerroCompanero : MonoBehaviour
{
    [Header("Ajustes de Seguimiento")]
    public Transform jugador;
    public float distanciaParada = 2.5f;
    public float velocidadCaminar = 4.0f;
    public float velocidadCorrer = 9.5f; 
    public float velocidadGiro = 8f;
    public float fuerzaSalto = 8.5f;
    public float gravedad = -24.0f;
    private float velocidadVerticalY = 0f;

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
               
                Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadGiro * Time.deltaTime);
            }

            //2. Acelera si alex esta lejos para no perderse
            float velocidadActual = (distancia > 5.0f) ? velocidadCorrer : velocidadCaminar;
            Vector3 movimientoHorizontal = transform.forward * velocidadActual;

            //3. Salto y Gravedad
            if (characterController.isGrounded)
            {
                if (velocidadVerticalY < 0f) velocidadVerticalY = -2f;
                // Salta 
                if (Input.GetKeyDown(KeyCode.Space) || (jugador.position.y - transform.position.y) > 0.6f)
                {
                    velocidadVerticalY = fuerzaSalto;
                }
            }
            else
            {
                velocidadVerticalY += gravedad * Time.deltaTime;
            }
            // 4. APLICAR MOVIMIENTO FÍSICO AL CONTROLADOR
            Vector3 movimientoFinal = movimientoHorizontal + Vector3.up * velocidadVerticalY;
            characterController.Move(movimientoFinal * Time.deltaTime);


            // 5. Animación de patas
            if (animator != null)
            {
                animator.SetFloat("Vert", 1f);
                // State: 0 para caminar (walk), 1 para correr (run)
                animator.SetFloat("State", (distancia > 5.0f) ? 1f : 0f);
            }
        }

        else
        {
            // Detenido junto al jugador
            if (characterController.isGrounded && velocidadVerticalY < 0f)
            {
                velocidadVerticalY = -2f;
            }
            else
            {
                velocidadVerticalY += gravedad * Time.deltaTime;
            }

            characterController.Move(Vector3.up * velocidadVerticalY * Time.deltaTime);
            if (animator != null)
            {
                animator.SetFloat("Vert", 0f); // Vuelve al Idle
                animator.SetFloat("State", 0f);
            }
        }
    }
}