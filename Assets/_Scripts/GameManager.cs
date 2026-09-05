using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Textos")]
    public Text textoReliquias;
    public Text textoVidas;

    [Header("Estadisticas")]
    public int totalReliquias = 0;
    public int maxReliquias = 3;
    public int vidas = 3;

    private Vector3 puntoRespawn;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        // Guardar la posicion inicial 
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            puntoRespawn = jugador.transform.position;
        }

        ActualizarUI();
    }

    public void SumarReliquia()
    {
        totalReliquias++;
        ActualizarUI();
    }

    public void PerderVida()
    {
        vidas--;
        ActualizarUI();

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            CharacterController cc = jugador.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false; 

            if (vidas > 0)
            {
                jugador.transform.position = puntoRespawn; // Vuelve al inicio
            }
            else
            {
                // Reinicia la escena al quedarse sin vidas
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (cc != null) cc.enabled = true;
        }
    }

    private void ActualizarUI()
    {
        if (textoReliquias != null)
            textoReliquias.text = "Reliquias: " + totalReliquias + " / " + maxReliquias;

        if (textoVidas != null)
            textoVidas.text = "Vidas: " + vidas;
    }
}