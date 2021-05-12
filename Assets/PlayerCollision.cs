using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Player collided, dies");
            gameObject.SetActive(false);
            SceneManager.LoadScene("LiamScene 1");
        }        
    }
}
