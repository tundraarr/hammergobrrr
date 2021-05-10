using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy[] enemyPrefabs;
    public Transform spawnLocation;
    public Transform playerRef;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnRandomEnemy();
        }
    }

    public void SpawnRandomEnemy()
    {
        Enemy enemyInstance = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnLocation);
        enemyInstance.player = playerRef;
    }
}
