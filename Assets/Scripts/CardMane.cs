using System.Collections;
using System.Collections.Generic;
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
        int emptySlots = maxCards - cardInstances.Count;
        int addCount = Mathf.Min(count, emptySlots); // 追加できる最大枚数

        for (int i = 0; i < addCount; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, Vector3.zero, Quaternion.Euler(70, 180, 0));
            cardInstances.Add(newCard);
        }

        RepositionCards(); // 左詰めに配置し直す
    }

    public void UseCard(GameObject targetCard)
    {
        if (cardInstances.Contains(targetCard))
        {
            cardInstances.Remove(targetCard);
            Destroy(targetCard);
            RepositionCards();
        }
    }

    // 指定したインデックスのカードを削除する
    public void RemoveRandomCard()
    {
        if (cardInstances.Count > 0)
        {
            int randomIndex = Random.Range(0, cardInstances.Count); // 0 から カード枚数-1 の範囲でランダム取得
            GameObject cardToRemove = cardInstances[randomIndex];
            cardInstances.RemoveAt(randomIndex);
            Destroy(cardToRemove);
            RepositionCards();
        }
        else
        {
            Debug.LogWarning("削除できるカードがありません");
        }
    }

    // 全てのカードを削除する
    public void ClearAllCards()
    {
        foreach (var card in cardInstances)
        {
            Destroy(card);
        }
        cardInstances.Clear();
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
    public void ForceNextPlayerDiscardAndDraw()
    {
        int discardCount = cardInstances.Count;

        if (discardCount > 0)
        {
            // 次のプレイヤーのカードを全て破棄
            ClearAllCards();

            // 同じ枚数だけカードを引かせる
            DrawCard(discardCount);

            // 必要であればサーバーに通知
            serverLink.Transmission($"次のプレイヤーが{discardCount}枚のカードを捨て、同じ枚数を引き直した");
        }
        else
        {
            Debug.Log("次のプレイヤーはカードを持っていないため、捨てるものがありません");
        }
    }
}
