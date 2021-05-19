using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField]
    private float knockbackForce;
    private Vector2 swingDirection;

    [SerializeField]
    private Transform hammer;

    [SerializeField]
    private HammerHitbox hammerHitboxPrefab;

    [SerializeField]
    private Transform hammerHitboxLocation;

    [SerializeField]
    private Animator anim;

    private bool hammerSwung = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (!anim.GetCurrentAnimatorStateInfo(0).IsName("HammerSwing"))
        //{
        //    AimHammer();
        //}

        if (!hammerSwung)
        {
            AimHammer();
        }

        if (Input.GetMouseButtonDown(0))
        {
            SwingHammer();
        }
    }

    private void AimHammer()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = mousePos - (Vector2)transform.position;
        hammer.transform.up = mouseDirection;
    }

    private void SwingHammer()
    {
        //anim.Play("HammerSwing");
        swingDirection = (hammer.transform.up).normalized;
        Vector3 swingPlace = new Vector3(0f, 1f, 1f);
        StartCoroutine(RotateHammer(Quaternion.Euler(swingPlace), 0.6f));
    }

    IEnumerator RotateHammer(Quaternion endValue, float duration)
    {
        hammerSwung = true;
        float time = 0;
        Quaternion startValue = transform.rotation;

        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endValue;
        hammerSwung = false;
    }

    public void ToggleHammerSwung()
    {
        hammerSwung = !hammerSwung;
    }

    public void SpawnHammerHitbox()
    {
        HammerHitbox hitboxInstance = Instantiate(hammerHitboxPrefab, hammerHitboxLocation.position, hammerHitboxLocation.rotation);
        hitboxInstance.forceOfSwing = knockbackForce;
        hitboxInstance.directionOfSwing = swingDirection;
    }
}
