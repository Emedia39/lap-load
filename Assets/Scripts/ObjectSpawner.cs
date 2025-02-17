using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] SomeObject;
    public void Spawn()
    {
        SomeObject.Clone();

        
    }
    void Awaked()
    {
        SomeObject.transform.SetActive = true;
    }
}
