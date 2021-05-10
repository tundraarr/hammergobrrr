using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClusterManager : MonoBehaviour
{
    //Keep track of enemy clusters
    public HashSet<Cluster> enemyClusters = new HashSet<Cluster>();

    //Handle the collission
    public bool HandleCollision(Enemy caller, Enemy collider)
    {
        foreach (Cluster c in enemyClusters)
        {
            //Check if they are both in the same cluster - a consequence of the collision checks occuring for both caller and collider
            if (c.enemies.Contains(caller) && c.enemies.Contains(collider))
            {
                return false;
            }
        }

        bool isCallerInCluster = false;
        bool isColliderInCluster = false;
        Cluster callerCluster = null;
        Cluster colliderCluster = null;

        //Find whether or not the caller and the collider is in a cluster 
        foreach(Cluster c in enemyClusters)
        {
            if(c.enemies.Contains(caller))
            {
                isCallerInCluster = true;
                callerCluster = c;
            }

            if(c.enemies.Contains(collider))
            {
                isColliderInCluster = true;
                colliderCluster = c;
            }
        }

        //If the neither the caller nor the collider are in a cluster - create a new cluster and have them join each other
        //If the caller is not in a cluster and the collider is - join the collider's cluster
        //If the caller is in a cluster and the collider is NOT - have the collider join the caller's cluster
        //If both the caller are in a cluster have the caller's cluster join the collider's cluster

        if (!isCallerInCluster && !isColliderInCluster)
        {
            CreateCluster(caller, collider);
        } else if(!isCallerInCluster && isColliderInCluster) {
            JoinCluster(colliderCluster, caller);
        } else if(isCallerInCluster && !isColliderInCluster) {
            JoinCluster(callerCluster, collider);
        } else if(isCallerInCluster && isColliderInCluster) {
            JoinCluster(colliderCluster, callerCluster);
        }

        //Returns true if the method completed properly
        return true;
    }

    //Enemy and Cluster Join
    public void JoinCluster(Cluster existingCluster, Enemy joiner)
    {
        existingCluster.enemies.Add(joiner);
        SetJoint(existingCluster, joiner);
    }
    
    //Cluster and Cluster Join
    public void JoinCluster(Cluster existingCluster, Cluster joinerCluster)
    {
        existingCluster.enemies.UnionWith(joinerCluster.enemies);
        //Remove the joiner cluster after it has merged with the existing cluster
        enemyClusters.Remove(joinerCluster);

        //Assign the core to the newly joined enemies
        AssignClusterCore(existingCluster, existingCluster.GetClusterCore());
    }

    //Create a new cluster
    public void CreateCluster(Enemy caller, Enemy collider)
    {
        Cluster newCluster = new Cluster();
        newCluster.enemies.Add(caller);
        newCluster.enemies.Add(collider);

        enemyClusters.Add(newCluster);

        //Make the collider the core
        AssignClusterCore(newCluster, collider);
    }

    //Update the the neighbours for each enemy in the cluster


    //Assign cluster core
    //Conditions for assigning new cluster core: 
    //When a new cluster is made | When clusters are merged together | When a cluster core is to be destroyed |
    public void AssignClusterCore(Cluster cluster, Enemy core)
    {
        cluster.SetClusterCore(core);
    }

    //Add the core as the joint to new enemies of a cluster and turn on their fixed joint 2d component 
    public void SetJoint(Cluster cluster, Enemy joiner)
    {
        joiner.joint.connectedBody = cluster.GetClusterCore().rb;
        joiner.joint.enabled = true;
    }

    public void DestroyClusteredEnemies()
    {

    }

    //Spread hit among all enemies in cluster
    public void HandleClusterHit(Enemy enemy, float forceAmount, Vector2 direction)
    {
        //Trying out: just hit stun all enemies in cluster but don't apply the force to any of them except the one hit

        foreach (Cluster c in enemyClusters)
        {
            if (c.enemies.Contains(enemy))
            {
                c.HitCluster(forceAmount, direction);
            }
        }
    }

    private void Update()
    {
        //Testing code for checking the status of the enemy cluster hash set
        if(Input.GetMouseButtonDown(1))
        {
            foreach(Cluster c in enemyClusters)
            {
                Debug.Log(c.GetClusterCore());
            }
        }
    }
}
