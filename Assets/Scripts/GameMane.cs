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
        serverLink.Transmission("dead");
    }
    public void SubCost(int count)
    {
        cardCost -= count;
        costText.text = cardCost.ToString();
    }
}
