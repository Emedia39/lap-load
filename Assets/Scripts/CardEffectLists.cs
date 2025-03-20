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
    public void CardEffects(int ID, bool chack)
    {
        int nextPlayer = serverLink.playerID + 1;
        if (nextPlayer > 4)
        {
            nextPlayer = 1;
        }
        switch (ID)
        {
            case 0:
                //おせっかい
                if (Ganemane.cardCost >= 3)
                {
                    //サイコロを投げて出た目の数分次のプレイヤーのブロックを増やす。
                    isUse = true;
                    if (!chack)
                    {
                        Ganemane.SubCost(3);
                    }
                    subject = $"{serverLink.playerName}";
                }
                break;
            case 1:
                //一服
                if (Ganemane.cardCost >= 1)
                {
                    //カードを追加で2枚引く。
                    isUse = true;
                    if (!chack)
                    {
                        Cardmane.DrawCard(2);
                        Ganemane.SubCost(1);
                    }
                    subject = $"{serverLink.playerName}";
                }
                break;
            case 2:
                //スリの技術
                if (Ganemane.cardCost >= 2)
                {
                    //次のプレイヤーのカードを一枚ランダムに削除。
                    isUse = true;
                    if (!chack)
                    {
                        Cardmane.RemoveRandomCard();
                        Ganemane.SubCost(2);
                    }
                    subject = $"{nextPlayer}";
                }
                break;
            case 3:
                //鎮痛剤
                if (Ganemane.cardCost >= 0)
                {
                    //サイコロを2回投げゾロ目が出れば使えるコストを1回復。
                    isUse = true;
                    if (!chack)
                    {
                        Ganemane.DoubleRollBonus();
                        Ganemane.SubCost(0);
                    }
                    subject = $"{serverLink.playerName}";
                }
                break;
            case 4:
                //断罪
                if (Ganemane.cardCost >= 2)
                {
                    //次のプレイヤーのカードをすべて捨てる。次のプレイヤーは捨てた分の枚数カードを引く。
                    isUse = true;
                    if (!chack)
                    {
                        Cardmane.ForceNextPlayerDiscardAndDraw();
                        Ganemane.SubCost(2);
                    }
                    subject = $"{nextPlayer}";
                }
                break;
            case 5:
                //萎縮
                if (Ganemane.cardCost >= 3)
                {
                    //自分のこのターン落とすすべてのブロックの大きさを小さくする。
                    isUse = true;
                    if (!chack)
                    {
                        objectScale = "0.5";
                        Ganemane.SubCost(3);
                    }
                    subject = $"{serverLink.playerName}";
                }
                break;
            case 6:
                //抹殺
                if (Ganemane.cardCost >= 3)
                {
                    //自分の1つのブロックを消す。
                    isUse = true;
                    if (!chack)
                    {
                        sponer.DecreaseObject(1);
                        Ganemane.SubCost(3);
                    }
                    subject = $"{serverLink.playerName}";
                }
                break;
            case 7:
                //一家心中
                if (Ganemane.cardCost >= 3)
                {
                    //全員のカードを削除。
                    isUse = true;
                    if (!chack)
                    {
                        Cardmane.ClearAllCards();
                        Ganemane.SubCost(3);
                    }
                    subject = $"all";
                }
                break;
            case 8:
                //お手伝い
                if (Ganemane.cardCost >= 0)
                {
                    //設置するすべてのブロックの大きさが1.5倍になる代わりに利用最大コスト数が2増える
                    isUse = true;
                    if (!chack)
                    {
                        Ganemane.AddCost(2);
                        objectScale = "1.5";
                        Ganemane.SubCost(0);
                    }
                    subject = $"{serverLink.playerName}";
                }
                break;
            case 9:
                //命の加護
                if (Ganemane.cardCost >= 4)
                {
                    //一度だけ敗北を回避できる。
                    isUse = true;
                    if (!chack)
                    {
                        Ganemane.blessings = true;
                        Ganemane.SubCost(4);
                    }
                    subject = $"{serverLink.playerName}";
                }
                break;
            case 10:
                //応急処置
                if (Ganemane.cardCost >= 1)
                {
                    //サイコロを1つ増やす代わりに使えるコストを3増やす
                    isUse = true;
                    if (!chack)
                    {
                        Ganemane.AddCost(3);
                        Ganemane.SubCost(1);
                    }
                    subject = $"{serverLink.playerName}";
                }
                break;
            case 11:
                //断固拒否
                if (Ganemane.cardCost >= 4)
                {
                    //次のプレイヤーは次のターン、カードを使えない。
                    isUse = true;
                    if (!chack)
                    {
                        Ganemane.isUseCard = false;
                        Ganemane.SubCost(4);
                    }
                    subject = $"{nextPlayer}";
                }
                break;
        }
        if (isUse)
        {
            audioSource.PlayOneShot(cardUse);
        }
    }
}
