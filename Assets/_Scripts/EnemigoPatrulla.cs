using UnityEngine;

public class EnemigoPatrulla : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float distancia = 4.0f;
    public float velocidad = 2.0f;

    private Vector3 puntoInicial;
    private bool moviendoDerecha = true;

    void Start()
    {
        puntoInicial = transform.position;
    }

    void Update()
    {
        float limiteDerecha = puntoInicial.x + distancia;
        float limiteIzquierda = puntoInicial.x - distancia;

        if (moviendoDerecha)
        {
            transform.position += Vector3.right * velocidad * Time.deltaTime;
            if (transform.position.x >= limiteDerecha)
            {
                moviendoDerecha = false;
                transform.rotation = Quaternion.Euler(0, 270, 0); // Gira hacia la izquierda
            }
        }
        else
        {
            transform.position -= Vector3.right * velocidad * Time.deltaTime;
            if (transform.position.x <= limiteIzquierda)
            {
                moviendoDerecha = true;
                transform.rotation = Quaternion.Euler(0, 90, 0); // Gira hacia la derecha
            }
        }
    }
}