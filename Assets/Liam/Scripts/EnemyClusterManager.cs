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
        Debug.Log("The caller: " + caller + " The Collider: " + collider);
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

        //Check if this collision will cause enemies to be destroyed
        HashSet<Enemy> enemiesToDestroy = DefineDestroyableEnemies(caller);
        if (enemiesToDestroy != null)
        {
            Cluster clusterToCheck = GetCluster(caller);
            DestroyClusteredEnemies(enemiesToDestroy, clusterToCheck);

            //Handle remaining enemies in the cluster if any
            Queue<HashSet<Enemy>> remainingEnemies = new Queue<HashSet<Enemy>>();
            StartCoroutine(DefineNewClusters(clusterToCheck, remainingEnemies));
            ////Remove the cluster if there are no more enemies remaining in it
            //if (remainingEnemies.Count == 0)
            //{
            //    enemyClusters.Remove(clusterToCheck);
            //    return true;
            //}

            ////Split the remaining enemies into clusters and then end collision handling
            //SplitCluster(remainingEnemies, clusterToCheck);
            return true;
        }

        //Returns true if the method completed properly
        return true;
    }

    //Find the cluster a enemy is from
    private Cluster GetCluster(Enemy enemy)
    {
        foreach(Cluster c in enemyClusters)
        {
            if(c.enemies.Contains(enemy))
            {
                return c;
            }
        }

        return null;
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

    //Create a new cluster from two enemies
    private void CreateCluster(Enemy caller, Enemy collider)
    {
        Cluster newCluster = new Cluster();
        newCluster.enemies.Add(caller);
        newCluster.enemies.Add(collider);

        //Make the collider the core
        AssignClusterCore(newCluster, collider);

        enemyClusters.Add(newCluster);
    }

    //Used to create a cluster out of a HashSet - mainly used when a cluster is destroyed and remaining enemies are to become clusters
    private void CreateCluster(HashSet<Enemy> enemies)
    {
        Cluster newCluster = new Cluster();
        newCluster.enemies = enemies;

        //Really disgusting way of having to select a new core for the cluster.
        //Result of using hashset for keeping track of enemies in a cluster - should definitely look for a better way to do this
        Enemy[] enemiesArray = new Enemy[newCluster.enemies.Count]; 
        newCluster.enemies.CopyTo(enemiesArray);
        Enemy newCore = enemiesArray[0];
        //===========================================================================================================================
        AssignClusterCore(newCluster, newCore);
        enemyClusters.Add(newCluster);
    }

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

    //Define the enemies to destroy
    public HashSet<Enemy> DefineDestroyableEnemies(Enemy caller)
    {
        HashSet<Enemy> allSameTypeNeighbours = new HashSet<Enemy>();
        //From the enemy that has collided
        GetNeighbours(caller, allSameTypeNeighbours, caller.enemyType);
        if(allSameTypeNeighbours.Count >= 3)
        {
            foreach(Enemy n in allSameTypeNeighbours)
            {
                Debug.Log("The same type neighbours are: " + n);
            }
            Debug.Log("Destroy Enemies");
            return allSameTypeNeighbours;
        }

        return null;
    }

    //Recursive method to return all neighbours of the same enemy type or any neighbours
    private void GetNeighbours(Enemy enemy, HashSet<Enemy>neighboursList, EnemyType enemyType=EnemyType.ANY)
    {
        Debug.Log("I am: " + enemy + "  and I have this many neighbours: " + enemy.neighbours.Count);
        neighboursList.Add(enemy);
        foreach(Enemy en in neighboursList)
        {
            Debug.Log("Status: " + en);
        }

        foreach(Enemy neighbour in enemy.neighbours)
        {
            //If there is a specified enemy type
            if(neighbour != null)
            {
                if(enemyType != EnemyType.ANY)
                {
                    Debug.Log("Has type");
                    if(neighbour.enemyType == enemy.enemyType && !neighboursList.Contains(neighbour))
                    {
                        GetNeighbours(neighbour, neighboursList, neighbour.enemyType);
                    }
                }
                else
                {
                    Debug.Log("Has No Type");
                    if(!neighboursList.Contains(neighbour))
                        GetNeighbours(neighbour, neighboursList);
                }
            }
        }
    }

    //Destroy the enemies in a cluster
    public void DestroyClusteredEnemies(HashSet<Enemy> enemies, Cluster associatedCluster)
    {
        foreach (Enemy e in enemies)
        {
            if (associatedCluster.enemies.Contains(e))
            {
                associatedCluster.enemies.Remove(e);
                Destroy(e.gameObject);
            }
        }
    }

    //Return a list of all the mini clusters present after enemies have been destroyed in the main cluster
    //EDIT: Will have to make this a coroutine so that it updates after the deleted enemies - this solution is wack af
    private IEnumerator DefineNewClusters(Cluster currentCluster, Queue<HashSet<Enemy>> miniClusters)
    {
        yield return null;
        //Queue<HashSet<Enemy>> miniClusters = new Queue<HashSet<Enemy>>();
        Debug.Log("Hold this shit: " + currentCluster.enemies.Count);

        HashSet<Enemy> checkedEnemies = new HashSet<Enemy>();

        if(currentCluster.enemies.Count > 0)
        {
            //Check whether there are any indivudal enemies remaining
            //Individuals will be removed from existing cluster and have their joint turned off so they can move independently again
            foreach (Enemy enemy in currentCluster.enemies) //NOTE: THIS RUNS DUPES
            {
                //Free the enemy if they are no longer connected to the cluster 
                if (enemy.neighbours.Count == 0)
                {
                    currentCluster.enemies.Remove(enemy);
                    enemy.neighbours.Clear();
                    enemy.joint.connectedBody = null;
                    enemy.joint.enabled = false;
                }
                else
                {
                    //Find what the new mini clusters are and add them to the List
                    if(!checkedEnemies.Contains(enemy))
                    {
                        HashSet<Enemy> enemyGroup = new HashSet<Enemy>();
                        GetNeighbours(enemy, enemyGroup);

                        foreach (Enemy e in enemyGroup)
                        {
                            Debug.Log("A r neighbourslist: " + e);
                            checkedEnemies.Add(e);
                        }

                        miniClusters.Enqueue(enemyGroup);
                    }
                }
            }
        }

        //Remove the cluster if there are no more enemies remaining in it
        if (miniClusters.Count == 0)
        {
            enemyClusters.Remove(currentCluster);
        }

        //Split the remaining enemies into clusters and then end collision handling
        SplitCluster(miniClusters, currentCluster);

        Debug.Log("Number of miniclusters: " + miniClusters.Count);
        //return miniClusters;
    }

    //Split a cluster - occurs when enemies are destroyed in a cluster
    private void SplitCluster(Queue<HashSet<Enemy>> remainingEnemies, Cluster existingCluster)
    {
        Debug.Log("Size of queue: " + remainingEnemies.Count);
        foreach (Enemy e in remainingEnemies.Peek())
        {
            Debug.Log("Remainer: " + e);
        }

        enemyClusters.Remove(existingCluster);
        //Reuse existing cluster for one of the remaining enemy groups
        //TODO: SET CORE
        //existingCluster.enemies.Clear();
        //existingCluster.enemies = remainingEnemies.Dequeue();
        //existingCluster.SetClusterCore()

        //For every other group of enemies, make a new cluster for them
        foreach (HashSet<Enemy> enemies in remainingEnemies)
        {
            CreateCluster(enemies);
        }
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
