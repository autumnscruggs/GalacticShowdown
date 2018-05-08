using UnityEngine;
using System.Collections;

public class TimedSpawner : Spawner
{
    public float timeBetweenSpawns;
    private float lastSpawnTime;

    public void Update()
    {
        SpawnOverTime();
        base.Update();
    }

    protected void SpawnOverTime()
    {
        lastSpawnTime += Time.deltaTime;
        if (lastSpawnTime > timeBetweenSpawns)
        {
            lastSpawnTime = 0.0f;
            this.Spawn();
        }

    }
}
