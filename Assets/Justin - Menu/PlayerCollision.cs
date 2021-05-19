using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject score;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Player collided, dies");
            gameOverUI.SetActive(true);
            gameObject.SetActive(false);
        }        
    }
}
