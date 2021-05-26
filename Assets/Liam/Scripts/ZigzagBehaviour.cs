using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigzagBehaviour : EnemyBehaviour
{
    private float activeDuration = 2f;
    private float currentActiveDuration = 0f;
    private float sleepDuration = 2f;
    private float currentSleepDuration = 0f;

    private bool isActive = true;
    private bool targetFound = false;

    private Vector3 targetLocation;

    public override void RunBehaviour()
    {
        GetNewTargetPosition();

        if(isActive)
        {
            if(currentActiveDuration < activeDuration)
            {
                currentActiveDuration += Time.deltaTime;
                MoveToPlayerLastPos();
            }
            else
            {
                targetFound = false;
                isActive = false;
                currentSleepDuration = 0;
            }
        }
        else
        {
            if(currentSleepDuration < sleepDuration)
            {
                currentSleepDuration += Time.deltaTime;
            }
            else
            {
                isActive = true;
                currentActiveDuration = 0;
            }
        }
    }

    //Move to the player's last position when this enemy is active again
    private void MoveToPlayerLastPos()
    {
        Vector2 direction = (targetLocation - self.transform.position).normalized;
        //TODO: Can not use velocity because it can override the swing
        //Maybe just have the enemy launch itself at the player? or lerp
        self.rb.velocity = direction * (self.moveSpeed * 1.5f) * Time.deltaTime;
    }

    private void GetNewTargetPosition()
    {
        if(!targetFound)
        {
            targetLocation = player.transform.position;
            targetFound = true;
            Debug.Log("Found target last position: " + targetLocation);
        }
    }
}
