using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    [SerializeField] ObjectSponer sponer;
    public void Roll()
    {
        sponer.DiceRoll();
    }
}
