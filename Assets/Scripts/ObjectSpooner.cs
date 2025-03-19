using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

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
        restext.text = "0";
    }

    public void DiceRoll()
    {
        if (!isRoll)
        {
            rnd = Random.Range(1, 4);
            dropCount = rnd;
            restext.text = rnd.ToString();
            cardMane.DrawCard(rnd);
            isRoll = true;
        }
    }

    public async void DropCount()
    {
        if (isRoll && serverLink.isPlay)
        {
            List<Vector3> placedPositions = new List<Vector3>();
            List<Task> dropTasks = new List<Task>();
            float minDistance = 3.0f; // 衝突防止のための最小距離

            for (int i = 0; i < dropCount; i++)
            {
                Vector3 dropPos;
                int safetyCounter = 0;

                // 他オブジェクトと被らない位置を探す
                do
                {
                    float rndX = Random.Range(-setRange, setRange);
                    float rndZ = Random.Range(-setRange, setRange);
                    dropPos = new Vector3(rndX, 0, rndZ);
                    safetyCounter++;

                    // 無限ループ防止
                    if (safetyCounter > 1000)
                    {
                        Debug.LogWarning("適切な配置位置を見つけられませんでした。");
                        break;
                    }

                } while (placedPositions.Exists(pos => Vector3.Distance(pos, dropPos) < minDistance));

                placedPositions.Add(dropPos);

                int rndObject = Random.Range(0, blockObject.Length);
                string message = $"drop/{serverLink.playerName}/{dropPos.x}/{dropPos.z}/{rndObject}";

                dropTasks.Add(Task.Run(() => serverLink.Transmission(message)));
            }

            await Task.WhenAll(dropTasks);

            dropCount = 0;
            restext.text = "0";
            isRoll = false;
            serverLink.Transmission("endturn");
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
