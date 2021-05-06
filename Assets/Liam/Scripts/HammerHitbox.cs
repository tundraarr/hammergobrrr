using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHitbox : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 0.1f)]
    private float lifetime;

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
}

