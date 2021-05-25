using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy[] enemyPrefabs;
    public EnemyBehaviourCodes[] behaviours;
    public Transform[] spawnLocations;
    public Transform playerRef;
    public float spawnFrequency;
    public float spawnTimer;

    // Update is called once per frame
    void Update()
    {
        if(spawnTimer <= 0)
        {
            SpawnRandomEnemy();
            spawnTimer = spawnFrequency;
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }


    public void SpawnRandomEnemy()
    {
        Enemy enemyInstance = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnLocations[Random.Range(0, spawnLocations.Length)]);
        enemyInstance.player = playerRef;
        enemyInstance.behaviour = GiveRandomBehaviour();
    }

    //Enable random dispersion of behavious
    private EnemyBehaviour GiveRandomBehaviour()
    {
        EnemyBehaviourCodes behvCode = behaviours[Random.Range(0, behaviours.Length)];
        EnemyBehaviour behaviour = null;
        switch(behvCode)
        {
            case EnemyBehaviourCodes.CHASE:
                behaviour = new ChaseBehaviour();
                break;
            case EnemyBehaviourCodes.ZIGZAG:
                behaviour = new ZigzagBehaviour();
                break;
            case EnemyBehaviourCodes.PRESET:
                behaviour = new PresetBehaviour();
                break;
        }

        return behaviour;
    }
}
