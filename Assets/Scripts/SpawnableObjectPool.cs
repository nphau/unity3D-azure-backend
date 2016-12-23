using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SpawnableObjectPool 
{
    private SpawnableObject[] pool;
    public SpawnableObjectPool(SpawnableObject obj, int poolCount)
    {
        GameObject container = new GameObject("Pool of " + obj.name);
        obj.gameObject.SetActive(false);
        pool = new SpawnableObject[poolCount];
        for (int i = 0; i < poolCount; i++)
        {
            pool[i] = GameObject.Instantiate(obj);
            pool[i].transform.parent = container.transform;
        }
    }

    public bool ObjectIsAvailable()
    {
        return pool.Any(o=>!o.gameObject.activeInHierarchy);
    }

    public SpawnableObject GetAvailableObject(bool initObject = true)
    {
        SpawnableObject spawn = pool.FirstOrDefault(o => !o.gameObject.activeInHierarchy);
        if (initObject && spawn != null)
        {
            spawn.gameObject.SetActive(true);
            spawn.GetComponent<DamagableObject>().Init();
        }
        return spawn;
    }
}
