using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectLists : MonoBehaviour
{
    [SerializeField] ObjectSponer sponer;
    [SerializeField] CardMane Cardmane;
    [SerializeField] GameMane Ganemane;
    public void CardEffects(int ID)
    {
        switch (ID)
        {
            case 0:
                //•Ïg
                if (Ganemane.cardCost >= 1)
                {
                    sponer.DecreaseObject(1);
                    Ganemane.SubCost(1);
                }
                break;
            case 1:
                //‚¨‚¹‚Á‚©‚¢
                if (Ganemane.cardCost >= 3)
                {
                    Ganemane.SubCost(3);
                }
                break;
            case 2:
                //Ó”C“]‰Å
                if (Ganemane.cardCost >= 3)
                {
                    Ganemane.SubCost(3);
                }
                break;
            case 3:
                //ˆê•ž
                if (Ganemane.cardCost >= 1)
                {
                    Cardmane.DrawCard(2);
                    Ganemane.SubCost(1);
                }
                break;
            case 4:
                //[‚¢ãJ
                if (Ganemane.cardCost >= 3)
                {
                    Ganemane.SubCost(3);
                }
                break;
            case 5:
                //‚·‚è‚Ì‹Zp
                if (Ganemane.cardCost >= 2)
                {
                    Ganemane.SubCost(2);
                }
                break;
            case 6:
                //’Á’ÉÜ
                if (Ganemane.cardCost >= 2)
                {
                    Ganemane.SubCost(2);
                }
                break;
            case 7:
                //ˆ«–‚‚ÌŒ_–ñ
                if (Ganemane.cardCost >= 5)
                {
                    Ganemane.SubCost(5);
                }
                break;
            case 8:
                //’fß
                if (Ganemane.cardCost >= 2)
                {
                    Ganemane.SubCost(2);
                }
                break;
            case 9:
                //ˆá–@‚ÈŽæ‚è—§‚Ä
                if (Ganemane.cardCost >= 3)
                {
                    Ganemane.SubCost(3);
                }
                break;
            case 10:
                //ˆÞk
                if (Ganemane.cardCost >= 1)
                {
                    Ganemane.SubCost(1);
                }
                break;
            case 11:
                //–•ŽE
                if (Ganemane.cardCost >= 3)
                {
                    Ganemane.SubCost(3);
                }
                break;
            case 12:
                //“¦–S
                if (Ganemane.cardCost >= 3)
                {
                    Ganemane.SubCost(3);
                }
                break;
        }
    }
}
