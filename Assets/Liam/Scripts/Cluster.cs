using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cluster
{
    public HashSet<Enemy> enemies = new HashSet<Enemy>();
    private Enemy clusterCore;

    //Trigger hit for all enemies in the cluster if ANY of them get hit
    public void HitCluster(float forceAmount, Vector2 direction)
    {
        foreach(Enemy e in enemies)
        {
            //Exclude the core from having hit stunned applied so that it recieves the force of impact
            if(e != clusterCore)
                e.ApplyHitStun();
        }

        clusterCore.GetHit(forceAmount, direction, false);
    }

    //Set the core of this cluster
    public void SetClusterCore(Enemy enemy)
    {
        clusterCore = enemy;
        clusterCore.joint.enabled = false;
        clusterCore.joint.connectedBody = null;

        foreach(Enemy e in enemies)
        {
            if(e != clusterCore)
            {
                //Turn on the fixed joint 2D component on all non-core enemies
                //Set the joint body of all non-core enemies to be the core
                e.joint.enabled = true;
                e.joint.connectedBody = clusterCore.rb;
            }
        }
    }

    public Enemy GetClusterCore()
    {
        return clusterCore;
    }
}
