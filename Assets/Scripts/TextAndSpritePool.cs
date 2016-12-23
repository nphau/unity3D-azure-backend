using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TextMeshPool
{
    private TextMesh[] pool;
    public TextMeshPool(TextMesh obj, int poolCount)
    {
        GameObject container = new GameObject("Pool of " + obj.name);
        obj.gameObject.SetActive(false);
        pool = new TextMesh[poolCount];
        for (int i = 0; i < poolCount; i++)
        {
            pool[i] = GameObject.Instantiate(obj);
            pool[i].transform.parent = container.transform;
        }
    }

    public bool ObjectIsAvailable()
    {
        return pool.Any(o => !o.gameObject.activeInHierarchy);
    }

    public TextMesh GetAvailableObject()
    {
        return pool.FirstOrDefault(o => !o.gameObject.activeInHierarchy);
    }

    public void Project(string text, Vector3 position, float duration)
    {
        if (ObjectIsAvailable())
        {
            //TextMesh mesh = GetAvailableObject();
            //mesh.gameObject.SetActive(true);
            //mesh.text = text;
            //mesh.transform.position = position;
            //MonoBehaviour
            //StartCoroutine(HideTextMeshAfterTime(mesh, duration));
            

        }
    }

   
}
