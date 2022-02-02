using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    Spawnpoint[] spawnpoints;

    void Awake()
    {
        Instance = this;
        spawnpoints = GetComponentsInChildren<Spawnpoint>();
    }

    public Transform GetTaggerSpawnpoint()
    {
        return spawnpoints[0].transform;
    }

    public Transform GetRunnerSpawnpoint()
    {
        return spawnpoints[1].transform;
    }
}
