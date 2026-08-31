using UnityEngine;

public class Coleccionable : MonoBehaviour
{
    public float velocidadRotacion = 90f;
    public float velocidadFlotacion = 2f;
    public float alturaFlotacion = 0.15f;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Gira continuamente
        transform.Rotate(Vector3.up, velocidadRotacion * Time.deltaTime, Space.World);

        // Flota arriba y abajo suavemente
        float nuevoY = posicionInicial.y + Mathf.Sin(Time.time * velocidadFlotacion) * alturaFlotacion;
        transform.position = new Vector3(posicionInicial.x, nuevoY, posicionInicial.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detecta al jugador por Tag o por su CharacterController
        if (other.CompareTag("Player") || other.GetComponent<CharacterController>() != null)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.SumarReliquia();
            }

            Destroy(gameObject);
        }
    }
}