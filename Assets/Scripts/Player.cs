﻿using Assets.Scripts.Cards;
using UnityEngine;
using UnityEngine.UI;

class Player : MonoBehaviour
{

    public GameObject prefab;
    public int placeNum;
    public int ID;

    public Card[] cards = new Card[2];


    public void Awake()
    {

    }

    public void ShowPreflop(Card card1, Card card2)
    {
      //  Debug.Log($"ShowPreflop");
        cards[0] = card1;
        cards[1] = card2;


        string imgFileName1 = card1.ToFilename();
        string imgFileName2 = card2.ToFilename();

        Texture2D img1 = Resources.Load("Cards/" + imgFileName1) as Texture2D;
        Texture2D img2 = Resources.Load("Cards/" + imgFileName2) as Texture2D;

        RawImage[] rawImage = prefab.GetComponentsInChildren<RawImage>();

        rawImage[0].texture = img1;
        rawImage[1].texture = img2;
   
    }

    public void ShowCover()
    {
     //   Debug.Log($"ShowCover");

        Texture2D img = Resources.Load("Cards/cover") as Texture2D;

        RawImage[] rawImage = prefab.GetComponentsInChildren<RawImage>();

        rawImage[0].texture = img;
        rawImage[1].texture = img;

    }

    public void EnableActiveState()
    {
        Image img = prefab.transform.Find("Panel").GetComponent<Image>();
        img.color = UnityEngine.Color.blue;

      //  Debug.Log($"EnableActiveState");

    }

    public void DisableActiveState()
    {
        Image img = prefab.transform.Find("Panel").GetComponent<Image>();
        img.color = UnityEngine.Color.black;
    }

    public void Fold()
    {
        cards[0] = null;
        cards[1] = null;

        RawImage[] rawImage = prefab.GetComponentsInChildren<RawImage>();

        rawImage[0].texture = null;
        rawImage[1].texture = null;
    }

}
