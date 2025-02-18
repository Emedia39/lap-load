using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObjectEria : MonoBehaviour
{
    [SerializeField] GameMane gameMane;
    private void OnTriggerEnter(Collider other)
    {
        gameMane.GameOver();
        Destroy(other.gameObject);
    }
}
