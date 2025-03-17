using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;
using System.Threading;

public class ObjectSponer : MonoBehaviour
{
    float setRange = 3.5f;
    [SerializeField] GameObject[] blockObject;
    [SerializeField] Text restext;
    [SerializeField] CardMane cardMane;
    [SerializeField] ServerLink serverLink;
    int rnd = 0;
    public int dropCount = 0;
    bool isRoll = false;
    private void Start()
    {
        restext.text = "";
    }
    public void DiceRoll()
    {
        if (!isRoll)
        {
            rnd = Random.Range(1, 3);
            dropCount = rnd;
            restext.text = rnd.ToString();
            cardMane.DrawCard(rnd);
            isRoll = true;
        }
    }
    public void DropCount()
    {
        if (isRoll)
        {
            for (int i = 0; i < dropCount; i++)
            {
                float rndX = Random.Range(-setRange, setRange);
                float rndZ = Random.Range(-setRange, setRange);
                int rndObject = Random.Range(0, 2);
                serverLink.Transmission($"drop/{serverLink.playerName}/{rndX}/{rndZ}/{rndObject}");
            }
            //serverLink.Transmission($"オブジェクトを{dropCount}個落とした");
            dropCount = 0;
            restext.text = "";
            isRoll = false;
        }
    }
    public void DropObject(float rndX, float rndZ, int rndObject)
    {
        Instantiate(blockObject[rndObject], new Vector3(rndX, 10, rndZ), Quaternion.identity);
    }
    public void DecreaseObject(int count)
    {
        if (dropCount > 0)
        {
            dropCount -= count;
            restext.text = dropCount.ToString();
        }
    }
}
