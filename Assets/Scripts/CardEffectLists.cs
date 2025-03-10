using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectLists : MonoBehaviour
{
    [SerializeField] ObjectSponer sponer;
    [SerializeField] CardMane Cardmane;
    public void CardEffects(int ID)
    {
        switch (ID)
        {
            case 0:
                //ïœêg
                sponer.DecreaseObject(1);
                break;
            case 1:
                //Ç®ÇπÇ¡Ç©Ç¢
                sponer.DecreaseObject(1);
                break;
            case 2:
                //ê”îCì]â≈
                sponer.DecreaseObject(1);
                break;
            case 3:
                //àÍïû
                Cardmane.DrawCard(2);
                break;
        }
    }
}
