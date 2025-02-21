using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectLists : MonoBehaviour
{
    [SerializeField] ObjectSponer sponer;
    public void CardEffects(int ID)
    {
        switch (ID)
        {
            case 0:
                sponer.DecreaseObject(1);
            break;
        }
    }
}
