using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameMane : MonoBehaviour
{
    [SerializeField] GameObject gameOverText;
    [SerializeField] Text costText;
    [SerializeField] ObjectSponer sponer;
    public int cardCost = 0;

    private void Start()
    {
        cardCost = 5;
        costText.text = cardCost.ToString();
    }
    public void Roll()
    {
        cardCost = 5;
        costText.text = cardCost.ToString();
        sponer.DiceRoll();
    }
    public void GameOver()
    {
        gameOverText.SetActive(true);
    }
    public void SubCost(int count)
    {
        cardCost -= count;
        costText.text = cardCost.ToString();
    }
}
