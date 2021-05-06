using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private Rigidbody2D rb;

    public Transform player;

    public bool hitStunned = false;
    public float hitStunDuration;

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    //TODO: Probably fix the bug where the enemy slows down when they approach player because normalization is fked
    private void MoveTowardsPlayer()
    {
        if(!hitStunned)
        {
            Vector2 directionToMove = (player.position - transform.position).normalized;
            rb.velocity = directionToMove * moveSpeed * Time.deltaTime;
        }
    }

    public void GetHit(float forceAmount, Vector2 direction)
    {
        Vector2 gettingHitForce = forceAmount * direction;
        rb.AddForce(gettingHitForce);
        hitStunned = true;
        StopAllCoroutines();
        StartCoroutine(HitStunTimer(hitStunDuration));
    }

    private IEnumerator HitStunTimer(float time)
    {
        yield return new WaitForSeconds(time);
        hitStunned = false;
    }
}
