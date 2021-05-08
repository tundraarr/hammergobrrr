using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cluster
{
    public HashSet<Enemy> enemies = new HashSet<Enemy>();
    private Enemy clusterCore;

    //Set the core of this cluster
    public void SetClusterCore(Enemy enemy)
    {
        clusterCore = enemy;
        clusterCore.joint.enabled = false;

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
