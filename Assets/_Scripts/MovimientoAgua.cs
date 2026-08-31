using UnityEngine;

public class MovimientoAgua : MonoBehaviour
{
    public float velocidadX = 0.05f;
    public float velocidadY = 0.15f;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float offsetX = Time.time * velocidadX;
        float offsetY = Time.time * velocidadY;

        // Desplaza la textura principal del shader Standard
        rend.material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}