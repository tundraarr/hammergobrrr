using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetBehaviour : EnemyBehaviour
{
    private Transform targetLocation;
    private Vector2 minMaxTimeLimit = new Vector2(2.5f, 5f);
    private float timeLimit = 0;
    private bool targetReached = true;

    private float targetReachedOffset = 2f;


    public override void RunBehaviour()
    {
        if(targetReached || timeLimit <= 0)
        {
            PickNewTarget();
            timeLimit = Random.Range(minMaxTimeLimit.x, minMaxTimeLimit.y);
        }

        GoToTargetLocation();
        targetReached = TargetReached();
        timeLimit -= Time.deltaTime;
    }

    private void GoToTargetLocation()
    {
        Vector2 direction = (targetLocation.position - self.transform.position).normalized;
        self.rb.velocity = direction * self.moveSpeed * Time.deltaTime;
    }

    private void PickNewTarget()
    {
        Transform[] availableLocations = FindObjectOfType<PresetMoveLocations>().locations;
        Transform newLocation = availableLocations[Random.Range(0, availableLocations.Length)];
        if(newLocation == targetLocation)
        {
            PickNewTarget();
        }
        else
        {
            targetLocation = newLocation;
            targetReached = false;
        }
    }

    private bool TargetReached()
    {
        Vector2 topLeft, botRight;
        topLeft = new Vector2(targetLocation.position.x - targetReachedOffset, targetLocation.position.y + targetReachedOffset);
        botRight = new Vector2(targetLocation.position.x + targetReachedOffset, targetLocation.position.y - targetReachedOffset);

        if(self.transform.position.x >= topLeft.x && self.transform.position.y <= topLeft.y &&
           self.transform.position.x <= botRight.x && self.transform.position.y >= botRight.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
