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
    public List<Enemy> neighbours;

    public EnemyType enemyType;

    public bool hitStunned = false;
    public float hitStunDuration;

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if(!joint.enabled)
        {
            MoveTowardsPlayer();
        }

        //Keep the clamp if we want to implement - hitting multiple enemies in a cluster causes the knockback to be greater
        //Vector2.ClampMagnitude(rb.velocity, 10);
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

    public void GetHit(float forceAmount, Vector2 direction, bool isMultiHit)
    {
        if (!joint.enabled)
        {
            Vector2 gettingHitForce = forceAmount * direction;
            rb.AddForce(gettingHitForce);
            hitStunned = true;
            StopAllCoroutines();
            StartCoroutine(HitStunTimer(hitStunDuration));
        }
        else if(joint.enabled)
        {
            FindObjectOfType<EnemyClusterManager>().HandleClusterHit(this, forceAmount, direction);
        }
    }

    //Should only really be used for applying to enemies that are part of a cluster
    public void ApplyHitStun()
    {
        hitStunned = true;
        StopAllCoroutines();
        StartCoroutine(HitStunTimer(hitStunDuration));
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
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Enemy incomingEnemy = collision.gameObject.GetComponent<Enemy>();
            Debug.Log("Bubble collided: " + collision.gameObject);

            if(incomingEnemy.hitStunned)
            {
                FindObjectOfType<EnemyClusterManager>().HandleCollision(this, incomingEnemy);
                if(!neighbours.Contains(incomingEnemy))
                {
                    neighbours.Add(incomingEnemy);
                }
            }
            
        }
    }

}
