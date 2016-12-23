using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable()]
public class SpawnOddsDefinition
{
    public string slug = "default";
    public int spawnOdds = 1;
    public float TBI_earliestSpawnTime = 0;
    public float TBI_latestSpawnTime = 200;

    public SpawnOddsDefinition(string slug, int spawnOdds)
    {
        this.slug = slug;
        this.spawnOdds = spawnOdds;
    }
}

