using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    [System.Serializable]
    public class WaveChunk
    {
        [Multiline]
        public string note;
        public EnemyType enemyType;
        public float maxWait = -1f; // maximum to wait before spawning this chunk (irrespective of if the previous chunk is completed), -1 -> no max.
        public float spawnDelay = 0; // how long to wait before spawning the chunk when it is first spawnable
        public SpawnPoint spawnPoint;
        public int count = 1; // how many spawn together
        public bool waitUntilPreviousDead = false; // if true wait for the previous enemies to die before beginning this chunk

        public int countMultiplier = 1;
        public float healthMultiplier = 1;
    }

    public enum EnemyType { None, GruntGoal, GruntPlayer, Tank, GruntPlayerWithTank, Explosive, GruntPlayerTutorial };
    public enum SpawnPoint { SpawnPoint1, SpawnPoint2, SpawnPoint3, SpawnPoint4};
    public List<WaveChunk> chunks;

}