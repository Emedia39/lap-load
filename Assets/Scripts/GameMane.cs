using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMane : MonoBehaviour
{
    [SerializeField] GameObject gameOverText;
    public void GameOver()
    {
        gameOverText.SetActive(true);
    }
}
