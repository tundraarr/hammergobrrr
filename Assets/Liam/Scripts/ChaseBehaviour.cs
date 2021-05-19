using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBehaviour : EnemyBehaviour
{
    public override void RunBehaviour()
    {
        Debug.Log(self.gameObject);
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        if (!self.hitStunned)
        {
            Vector2 directionToMove = (player.position - self.transform.position).normalized;
            self.rb.velocity = directionToMove * self.moveSpeed * Time.deltaTime;
        }
    }
}
