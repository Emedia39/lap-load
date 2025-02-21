using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cardtouch : MonoBehaviour
{
    CardEffectLists effectLists;
    CardMane cardTouch;
    public int ID = 0;
    public Material[] materials; // インスペクターで複数のマテリアルをセット
    private Renderer rend;
    private void Start()
    {
        effectLists = GameObject.Find("CardEffectList").GetComponent<CardEffectLists>();
        cardTouch = GameObject.Find("Hand").GetComponent<CardMane> ();
        rend = GetComponent<Renderer>();
        ChangeMaterial(0, 1);
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            effectLists.CardEffects(ID);
            Debug.Log("クリックした");
            cardTouch.UseCard(gameObject);
        }
    }
    public void ChangeMaterial(int slotIndex, int materialIndex)
    {
        if (rend == null || materials.Length == 0) return;

        Material[] mats = rend.materials; // 現在のマテリアル配列を取得
        if (slotIndex >= 0 && slotIndex < mats.Length && materialIndex >= 0 && materialIndex < materials.Length)
        {
            mats[slotIndex] = materials[materialIndex]; // 指定スロットのマテリアルを変更
            rend.materials = mats; // 変更を適用
        }
    }
}
