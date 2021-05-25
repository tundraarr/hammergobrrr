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

    public float attackTimer;
    public float currentTimer;

    // Update is called once per frame
    void Update()
    {
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("HammerSwing"))
        {
            AimHammer();
        }

        if(Input.GetMouseButtonDown(0) && currentTimer <= 0)
        {
            SwingHammer();
        }
        else
        {
            currentTimer -= Time.deltaTime;
        }

        //if(currentTimer > 0 )
        //{
        //    currentTimer -= Time.deltaTime;
        //}
        //else
        //{
        //    hammerSwung = false;
        //}
    }

    private void AimHammer()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = mousePos - (Vector2)transform.position;
        hammer.transform.up = mouseDirection;
        hammerSwung = true;
    }

    private void SwingHammer()
    {
        anim.Play("HammerSwing");
        swingDirection = (hammer.transform.up).normalized;
        currentTimer = attackTimer;
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
