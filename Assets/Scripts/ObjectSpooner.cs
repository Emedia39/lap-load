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
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        int rnd = Random.Range(1, 4);
    //        for (int i = 0; i < rnd; i++)
    //        {
    //            float rndX = Random.Range(-setRange, setRange);
    //            float rndZ = Random.Range(-setRange, setRange);
    //            int rndObject = Random.Range(0, 2);
    //            Instantiate(blockObject[rndObject], new Vector3(rndX, 10, rndZ), Quaternion.identity);
    //        }
    //    }
    //}
    private void Start()
    {
        restext.text = " ";
    }
    public void DiceRoll()
    {
        {
            int rnd = Random.Range(1, 4);
            for (int i = 0; i < rnd; i++)
            {
                float rndX = Random.Range(-setRange, setRange);
                float rndZ = Random.Range(-setRange, setRange);
                int rndObject = Random.Range(0, 2);
                Result(rnd);
                
                Instantiate(blockObject[rndObject], new Vector3(rndX, 10, rndZ), Quaternion.identity);
            }
        }
    }
    private void Result(int rnd)
    {
        restext.text = rnd.ToString();
        StartCoroutine(DelayCoroutine(1.25f, () => { TextErease(); }));
        
    }
    private void TextErease()
    {
        restext.text = " ";
    }
    private IEnumerator DelayCoroutine(float seconds, UnityAction action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
}
