using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectManager : MonoBehaviour
{
    private List<MoveObject> activeObjects = new List<MoveObject>();
    private ServerLink serverLink;
    [SerializeField] ObjectSponer objectSponer;
    [SerializeField] GameMane gameMane;
    private bool hasSentEndTurn = false;

    void Start()
    {
        serverLink = GameObject.Find("ServerLink").GetComponent<ServerLink>();
    }

    public void Register(MoveObject obj)
    {
        if (!activeObjects.Contains(obj))
        {
            activeObjects.Add(obj);
        }
    }

    public void NotifyStopped(MoveObject obj)
    {
        // 既に送信済みなら即リターン
        if (hasSentEndTurn)
            return;

        if (activeObjects.Contains(obj))
        {
            activeObjects.Remove(obj);
        }

        if (activeObjects.Count == 0 && serverLink.isPlay && !hasSentEndTurn && objectSponer.darwCard)
        {
            Debug.Log("全てのオブジェクトが停止しました -> endturn送信");
            serverLink.Transmission($"endturn/{serverLink.playerName}");
            hasSentEndTurn = true;
            objectSponer.darwCard = false;
            gameMane.AllReset();
        }
    }

    public void ResetEndTurnFlag()
    {
        hasSentEndTurn = false;
        activeObjects.Clear();
        Debug.Log("MoveObjectManager: フラグとリストをリセットしました");
    }
}
