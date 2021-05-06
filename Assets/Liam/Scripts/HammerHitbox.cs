using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHitbox : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 0.1f)]
    private float lifetime;

    public float forceOfSwing;
    public Vector2 directionOfSwing;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Despawn(lifetime));
    }

    private IEnumerator Despawn(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("Collision occurred");
            other.GetComponent<Enemy>().GetHit(forceOfSwing, directionOfSwing);
        }
    }
}

