using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    [System.Serializable]
    public class EnemyInstance
    {
        public EnemyType enemyType;
        public float spawndelay = 1f;
        public SpawnPoint spawnPoint;

    }

    public enum EnemyType { None, GruntGoal, GruntPlayer, Tank };
    public enum SpawnPoint { SpawnPoint1, SpawnPoint2, SpawnPoint3, SpawnPoint4};
    public List<EnemyInstance> enemies;

}