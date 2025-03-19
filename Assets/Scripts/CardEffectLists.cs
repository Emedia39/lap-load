using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectLists : MonoBehaviour
{
    [SerializeField] ObjectSponer sponer;
    [SerializeField] CardMane Cardmane;
    [SerializeField] GameMane Ganemane;
    [SerializeField] AudioClip cardUse;
    [SerializeField] ServerLink serverLink;
    AudioSource audioSource;
    public bool isUse = false;
    public string objectScale = "1.0";
    public string subject;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public bool CheckUse()
    {
        return isUse;
    }
    public void CardEffects(int ID)
    {
        int nextPlayer = serverLink.playerID + 1;
        if (nextPlayer > 4)
        {
            nextPlayer = 1;
        }
        switch (ID)
        {
            case 0:
                //‚¨‚¹‚Á‚©‚¢
                if (Ganemane.cardCost >= 3)
                {
                    isUse = true;
                    subject = $"{serverLink.playerName}";


                    Ganemane.SubCost(3);
                }
                break;
            case 1:
                //ˆê•ž
                if (Ganemane.cardCost >= 1)
                {
                    isUse = true;
                    subject = $"{serverLink.playerName}";
                    Cardmane.DrawCard(2);
                    Ganemane.SubCost(1);
                }
                break;
            case 2:
                //ƒXƒŠ‚Ì‹Zp
                if (Ganemane.cardCost >= 2)
                {
                    isUse = true;
                    subject = $"Player{nextPlayer}";
                    Ganemane.SubCost(2);
                }
                break;
            case 3:
                //’Á’ÉÜ
                if (Ganemane.cardCost >= 0)
                {
                    isUse = true;
                    subject = $"{serverLink.playerName}";

                    Ganemane.SubCost(0);
                }
                break;
            case 4:
                //’fß
                if (Ganemane.cardCost >= 2)
                {
                    isUse = true;
                    subject = $"Player{nextPlayer}";

                    Ganemane.SubCost(2);
                }
                break;
            case 5:
                //ˆÞk
                if (Ganemane.cardCost >= 3)
                {
                    isUse = true;
                    subject = $"{serverLink.playerName}";
                    objectScale = "0.5";
                    Ganemane.SubCost(3);
                }
                break;
            case 6:
                //–•ŽE
                if (Ganemane.cardCost >= 3)
                {
                    isUse = true;
                    subject = $"{serverLink.playerName}";
                    sponer.DecreaseObject(1);
                    Ganemane.SubCost(3);
                }
                break;
            case 7:
                //ˆê‰ÆS’†
                if (Ganemane.cardCost >= 3)
                {
                    isUse = true;
                    subject = $"all";

                    Ganemane.SubCost(3);
                }
                break;
            case 8:
                //‚¨Žè“`‚¢
                if (Ganemane.cardCost >= 0)
                {
                    isUse = true;
                    subject = $"{serverLink.playerName}";
                    objectScale = "1.5";
                    Ganemane.SubCost(0);
                }
                break;
            case 9:
                //–½‚Ì‰ÁŒì
                if (Ganemane.cardCost >= 4)
                {
                    isUse = true;
                    subject = $"{serverLink.playerName}";

                    Ganemane.SubCost(4);
                }
                break;
            case 10:
                //‰ž‹}ˆ’u
                if (Ganemane.cardCost >= 1)
                {
                    isUse = true;
                    subject = $"{serverLink.playerName}";

                    Ganemane.SubCost(1);
                }
                break;
            case 11:
                //’fŒÅ‹‘”Û
                if (Ganemane.cardCost >= 4)
                {
                    isUse = true;
                    subject = $"Player{nextPlayer}";

                    Ganemane.SubCost(4);
                }
                break;
        }
        if (isUse)
        {
            audioSource.PlayOneShot(cardUse);
        }
    }
}
