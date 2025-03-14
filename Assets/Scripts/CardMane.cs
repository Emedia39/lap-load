using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardMane : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] ServerLink serverLink;
    [SerializeField] int maxCards = 5; // 最大枚数
    [SerializeField] float spacing = 1.5f; // カード間の間隔
    [SerializeField] float zOffset = 0.1f; // Z方向のずらし量（重なり）

    private List<GameObject> cardInstances = new List<GameObject>(); // カードリスト

    public void DrawCard(int count)
    {
        serverLink.Transmission($"カードを{count}枚引いた");
        int emptySlots = maxCards - cardInstances.Count;
        int addCount = Mathf.Min(count, emptySlots); // 追加できる最大枚数

        for (int i = 0; i < addCount; i++)
        {
            // 新しいカードを生成し、リストに追加
            GameObject newCard = Instantiate(cardPrefab, Vector3.zero, Quaternion.Euler(70, 180, 0));
            cardInstances.Add(newCard);
        }

        RepositionCards(); // 左詰めに配置し直す
    }

    public void UseCard(GameObject targetCard)
    {
        if (cardInstances.Contains(targetCard))
        {
            cardInstances.Remove(targetCard); // リストから削除
            Destroy(targetCard); // オブジェクトを削除
            RepositionCards(); // 残りのカードを左詰めにする
        }
    }

    private void RepositionCards()
    {
        Vector3 startPos = transform.position;

        for (int i = 0; i < cardInstances.Count; i++)
        {
            Vector3 newPos = startPos + new Vector3(i * spacing, 0, i * zOffset);
            cardInstances[i].transform.position = newPos;
        }
    }
}
