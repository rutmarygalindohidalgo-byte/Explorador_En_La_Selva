using UnityEngine;

public class TrampaMortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Detecta al jugador por Tag o CharacterController
        if (other.CompareTag("Player") || other.GetComponent<CharacterController>() != null)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.PerderVida();
            }
        }
    }
}