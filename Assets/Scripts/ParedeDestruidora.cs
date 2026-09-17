using UnityEngine;

public class DestroyProjectiles : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Algo entrou na parede: " + other.gameObject.name);

        if (other.CompareTag("Projectile"))
        {
            Debug.Log("PROJÉTIL DESTRUÍDO: " + other.gameObject.name);
            Destroy(other.gameObject);
        }
    }
}