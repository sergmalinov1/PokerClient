using Assets.Scripts.Cards;
using UnityEngine;
using UnityEngine.UI;

class Player : MonoBehaviour
{

    public GameObject prefab;
    public int placeNum;
    public int ID;

    public Card[] cards = new Card[2];

    public void ShowPreflop(Card card1, Card card2)
    {
        cards[0] = card1;
        cards[1] = card2;


        string imgFileName1 = card1.ToFilename();
        string imgFileName2 = card2.ToFilename();

        Texture2D img1 = Resources.Load("Cards/" + imgFileName1) as Texture2D;
        Texture2D img2 = Resources.Load("Cards/" + imgFileName2) as Texture2D;

        //RawImage rawImage = prefab.GetComponentInChildren<RawImage>();

         
        

        //RawImage rawImage = prefab.GetComponent<RawImage>();
        // RawImage[] rawImage = prefab.GetComponentsInChildren<RawImage>();

       // rawImage.texture = img1;
       // rawImage[1].texture = img2;

        

    }
}
