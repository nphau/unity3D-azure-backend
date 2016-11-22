using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// Class with static functions for doing calculations related to math
class Calculation
{
    // From a list of object-odds pairs, pick an object at random to spawn
    public static SpawnableObject DetermineSpawn(Dictionary<SpawnableObject, int> oddsMap, SpawnableObject[] allPossibleSpawns)
    {
        Dictionary<SpawnableObject, int> spawnIntKeys = new Dictionary<SpawnableObject, int>();
        for (int i = 0; i < allPossibleSpawns.Length; i++)
        {
            spawnIntKeys[allPossibleSpawns[i]] = i;
        }
        int rollMax = oddsMap.Values.Sum();
        int roll = Random.Range(0, rollMax);
        SpawnableObject[] options = new SpawnableObject[rollMax];

        for (int i = 0; i < rollMax; i++)
        {
            foreach (SpawnableObject obj in allPossibleSpawns)
            {
                if (oddsMap.ContainsKey(obj) && options.Count(o => o && spawnIntKeys[o] == spawnIntKeys[obj]) < oddsMap[obj])
                {
                    options[i] = obj;
                }
            }
        }

        return options[roll];
    }
}

