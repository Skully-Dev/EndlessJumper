using UnityEngine;
using UnityEngine.SceneManagement;

public class Killzone : MonoBehaviour
{
    [SerializeField]
    private Death death;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            death.PlayerDied();
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
