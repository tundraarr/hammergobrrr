using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyBehaviourCodes
{
    CHASE = 0,
    ZIGZAG,
    PRESET,
    BOUNCE
}

public abstract class EnemyBehaviour : MonoBehaviour
{
    protected Transform player;
    protected Enemy self;

    public abstract void RunBehaviour();

    public void SetPlayer(Transform _player)
    {
        player = _player;
    }

    public void SetSelf(Enemy enemy)
    {
        self = enemy;
    }
}
