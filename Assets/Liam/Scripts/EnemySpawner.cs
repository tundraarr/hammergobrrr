using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy[] enemyPrefabs;
    public EnemyBehaviourCodes[] behaviours;
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
            case EnemyBehaviourCodes.BOUNCE:
                behaviour = new BounceBehaviour();
                break;
        }

        return behaviour;
    }
}
