using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cardtouch : MonoBehaviour
{
    CardEffectLists effectLists;
    CardMane cardTouch;
    public int ID = 0;
    [SerializeField] Material[] materials; // 複数のマテリアルを設定できるようにする

    private void Start()
    {
        effectLists = GameObject.Find("CardEffectList").GetComponent<CardEffectLists>();
        cardTouch = GameObject.Find("Hand").GetComponent<CardMane>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        ID = Random.Range(0, 4);
        if (meshRenderer != null)
        {
            Material[] mats = meshRenderer.materials;

            this.GetComponent<MeshRenderer>().material = materials[ID];
        }
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
}
