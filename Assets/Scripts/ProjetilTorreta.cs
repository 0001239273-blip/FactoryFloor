using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjetilTorreta : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float velocidade = 8f;

    private void Update()
    {
        // Move o projétil para a esquerda
        transform.Translate(
            Vector2.left * velocidade * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se acertar o Player, reinicia a cena
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }
    }
}