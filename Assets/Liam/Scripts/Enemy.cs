using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    GREEN,
    RED,
    BLUE
}

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    public Rigidbody2D rb;

    public FixedJoint2D joint;

    public Transform player;
    public HashSet<Enemy> neighbours;

    public EnemyType enemyType;

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
        SpreadHit(forceAmount, direction);
    }

    //Spread hit effect to all other enemies attached together
    public void SpreadHit(float forceAmount, Vector2 direction)
    {
        //if(joint.attachedRigidbody)
        //{
        //    joint.attachedRigidbody.GetComponent<Enemy>().GetHit(forceAmount, direction);
        //}
    }

    private IEnumerator HitStunTimer(float time)
    {
        yield return new WaitForSeconds(time);
        hitStunned = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Only trigger this code if the collision occurs between enemies
        //TODO: Make it so that it only triggers when they are in hitstun (been smacked by the player's hammer)
        if(collision.gameObject.CompareTag("Enemy") && hitStunned)
        {
            FindObjectOfType<EnemyClusterManager>().HandleCollision(this, collision.gameObject.GetComponent<Enemy>());
        }
    }
}
