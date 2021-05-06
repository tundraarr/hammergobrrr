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

    private void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    //TODO: Probably fix the bug where the enemy slows down when they approach player because normalization is fked
    private void MoveTowardsPlayer()
    {
        Vector2 directionToMove = (player.position - transform.position).normalized;
        rb.velocity = directionToMove * moveSpeed * Time.deltaTime;
    }
}
