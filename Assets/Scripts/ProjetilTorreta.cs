using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjetilTorreta : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float velocidade = 8f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = Vector2.left * velocidade;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );

            return;
        }

        if (other.CompareTag("DestroyZone"))
        {
            Destroy(gameObject);
        }
    }
}