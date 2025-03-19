using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cardtouch : MonoBehaviour
{
    CardEffectLists effectLists;
    CardMane cardTouch;
    GameMane gameMane;
    ServerLink serverLink;
    public int ID = 0;
    [SerializeField] Material[] materials;

    private void Start()
    {
        effectLists = GameObject.Find("CardEffectList").GetComponent<CardEffectLists>();
        cardTouch = GameObject.Find("Hand").GetComponent<CardMane>();
        gameMane = GameObject.Find("GameMane").GetComponent<GameMane>();
        serverLink = GameObject.Find("ServerLink").GetComponent<ServerLink>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        ID = Random.Range(0, materials.Length);

        if (meshRenderer != null)
        {
            meshRenderer.material = materials[ID];
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && gameMane.cardCost > 0 && serverLink.isPlay)
        {
            effectLists.CardEffects(ID);
            if (effectLists.CheckUse())
            {
                serverLink.Transmission($"use/{serverLink.playerName}/{serverLink.playerName}/{ID}");
                cardTouch.UseCard(gameObject);
                effectLists.isUse = false;
            }
        }
    }
}
