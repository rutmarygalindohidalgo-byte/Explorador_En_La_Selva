using UnityEngine;

public class ControladorJugador : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidadCaminar = 4.0f;
    public float velocidadCorrer = 9.0f;
    public float velocidadGiro = 130.0f;

    [Header("Física de Salto y Gravedad")]
    public float fuerzaSalto = 8.5f;
    public float gravedad = -24.0f;

    private CharacterController controller;
    private Animator anim;
    private float velocidadVerticalY = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Giro
        float rotacion = Input.GetAxis("Horizontal") * velocidadGiro * Time.deltaTime;
        transform.Rotate(0, rotacion, 0);

        // 2. Correr / Caminar
        bool estaCorriendo = Input.GetKey(KeyCode.LeftShift);
        float velocidadActual = estaCorriendo ? velocidadCorrer : velocidadCaminar;

        // 3. Dirección horizontal
        float vertical = Input.GetAxis("Vertical");
        Vector3 movimientoHorizontal = transform.forward * vertical * velocidadActual;

        // 4. Salto y Gravedad unificados
        if (controller.isGrounded)
        {
            if (velocidadVerticalY < 0f)
            {
                velocidadVerticalY = -2f; 
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
            {
                velocidadVerticalY = fuerzaSalto;
            }
        }
        else
        {
            velocidadVerticalY += gravedad * Time.deltaTime;
        }

        // 5. Un solo Move() para físicas precisas
        Vector3 movimientoFinal = movimientoHorizontal + Vector3.up * velocidadVerticalY;
        controller.Move(movimientoFinal * Time.deltaTime);

        // 6. Animaciones
        if (anim != null)
        {
            float valorAnimacion = 0f;
            if (Mathf.Abs(vertical) > 0.1f)
            {
                valorAnimacion = estaCorriendo ? 1.0f : 0.5f;
            }
            anim.SetFloat("Velocidad", valorAnimacion);
        }
    }
}