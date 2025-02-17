using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSummoner : MonoBehaviour
{
    public ObjectSpawner objectSpawner;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            objectSpawner.Spawn();
        }
    }
}
