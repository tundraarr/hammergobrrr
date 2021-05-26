using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBehaviour : EnemyBehaviour
{
    private LayerMask wallLayer = LayerMask.GetMask("Walls");
    private bool firstDirectionSet = false;
    private Vector2 direction;

    public override void RunBehaviour()
    {
        if(!firstDirectionSet)
        {
            SetInitialDirection();
        }

        self.rb.velocity = direction * (self.moveSpeed * 2f) * Time.deltaTime;

        RaycastHit2D[] theHits = new RaycastHit2D[1];
        self.coll.Raycast((self.rb.velocity).normalized, theHits, 3f, wallLayer);
        if(theHits[0])
        {
            Debug.Log("Run wall touch check");
            if(isTouchingWall())
            {
                direction = Vector2.Reflect(direction, theHits[0].normal);
            }
        }
    }

    //Could also use raycast to check for collision
    private bool isTouchingWall()
    {
        return self.coll.IsTouchingLayers(wallLayer);
    }

    private void SetInitialDirection()
    {
        direction = (player.transform.position - self.transform.position).normalized;
        firstDirectionSet = true;
    }
}
