using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI gameScore;
    public EnemyClusterManager enemyClusterManager;
    public TextMeshProUGUI endScore;

    // Update is called once per frame
    void Update()
    {
        gameScore.text = enemyClusterManager.clusterDestroyCount.ToString();
    }

    public void GameOverScore()
    {
        endScore.text = "Score: " + enemyClusterManager.clusterDestroyCount;
    }
}
