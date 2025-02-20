using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;

public class ObjectSponer : MonoBehaviour
{
    float setRange = 3.5f;
    [SerializeField] GameObject[] blockObject;
    [SerializeField] Text restext;
    int rnd = 0;
    bool isRoll = false;
    private void Start()
    {
        restext.text = "";
    }
    public void DiceRoll()
    {
        if (!isRoll)
        {
            rnd = Random.Range(1, 4);
            restext.text = rnd.ToString();
            isRoll = true;
        }
    }
    public void DropObject()
    {
        if (isRoll)
        {
            for (int i = 0; i < rnd; i++)
            {
                float rndX = Random.Range(-setRange, setRange);
                float rndZ = Random.Range(-setRange, setRange);
                int rndObject = Random.Range(0, 2);

                Instantiate(blockObject[rndObject], new Vector3(rndX, 10, rndZ), Quaternion.identity);
            }
            rnd = 0;
            restext.text = "";
            isRoll = false;
        }
    }      
}
