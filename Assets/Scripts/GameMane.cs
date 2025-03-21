using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameMane : MonoBehaviour
{ 
    [SerializeField] Text costText;
    [SerializeField] ObjectSponer sponer;
    [SerializeField] AudioClip diceRoll;
    [SerializeField] ServerLink serverLink;
    AudioSource audioSource;
    public int cardCost = 0;
    public bool blessings = false;
    public bool isUseCard = true; 

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cardCost = 5;
        costText.text = cardCost.ToString();
    }
    public void Roll()
    {
        if (serverLink.isPlay)
        {
            audioSource.PlayOneShot(diceRoll);
            cardCost = 5;
            costText.text = cardCost.ToString();
            sponer.DiceRoll();
        }
    }
    public void GameOver()
    {
        if (serverLink.isPlay && !blessings)
        {
            serverLink.Transmission("dead");
        }
        else if (serverLink.isPlay && blessings)
        {
            serverLink.Transmission("endturn");
            blessings = false;
        }
    }
    public void SubCost(int count)
    {
        cardCost -= count;
        costText.text = cardCost.ToString();
    }
    public void AddCost(int count)
    {
        cardCost += count;
        costText.text = cardCost.ToString();
    }
    public void DoubleRollBonus()
    {
        if (serverLink.isPlay)
        {
            audioSource.PlayOneShot(diceRoll);
            int firstRoll = Random.Range(1, 4);
            int secondRoll = Random.Range(1, 4);

            if (firstRoll == secondRoll)
            {
                AddCost(1);
                Debug.Log($"ゾロ目！コストが1回復しました。現在のコスト: {cardCost}");
            }
            else
            {
                Debug.Log($"ゾロ目ではありませんでした。結果: {firstRoll} と {secondRoll}");
            }
        }
    }
    public void TitleBack()
    {
        serverLink.Transmission("__end");
        Initiate.Fade("TitleScene", Color.black, 1.0f);
    }
}
