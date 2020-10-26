using Assets.Scripts.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class UIManagerGame : MonoBehaviour
{
    public static UIManagerGame instance;

    //=========GAME MANAGER ===============
   // public Card[] cards = new Card[2];

    public InputField chatInput;
    public Transform chatContent;
    public GameObject chatMessage;

    public Button btnJoin;

    public GameObject[] place = new GameObject[4];
    public GameObject playerPrefab;
    public Transform gameField;

    private List<Player> opponent = new List<Player>();
    private Player player = new Player();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }


    public void SentMsgToChat()
    {
        string msg = chatInput.text;

        if (msg != "")
        {
            GameObject newMsg = Instantiate(chatMessage, chatContent);
            Text content = newMsg.GetComponent<Text>();
            content.text = string.Format(content.text, "ME ", msg);

            ClientSend.SendChatMsg();

            chatInput.text = "";
        }
     
    }

    public void ShowMsgToChat(string user, string msg)
    {
        if (msg != "")
        {
          
            GameObject newMsg = Instantiate(chatMessage, chatContent);
           
            Text content = newMsg.GetComponent<Text>();
            content.text = string.Format(content.text, user, msg);
      
        }
    }

    public void JoinToGame()
    {
        btnJoin.enabled = false;
        ClientSend.JoinTheRoom();
    }

    public void NewPlayer(int _placeNum, string _userName)
    {
        Debug.Log($"NewPlayer 222 ");
        Vector3 pos = place[_placeNum].transform.position;
        Quaternion rotation = place[_placeNum].transform.rotation;



        player.prefab = Instantiate(playerPrefab, pos, rotation, gameField);

        Text[] content = player.prefab.GetComponentsInChildren<Text>();

        for (int i = 0; i < content.Length; i++)
        {
            if (content[i].name == "username")
                content[1].text = _userName; //User name
        }


        player.prefab.GetComponent<PlayerScript>().ID = _placeNum;
        player.prefab.name = "Player_" + _placeNum.ToString();

        Debug.Log($"prefab.name {player.prefab.name}");

    }

    public void NewOpponent(int _placeNum, string _userName)
    {
       // Debug.Log("_plaseNum " + _placeNum);
        Vector3 pos = place[_placeNum].transform.position;
        Quaternion rotation = place[_placeNum].transform.rotation;

        Player newPlayer = new Player();

        newPlayer.prefab = Instantiate(playerPrefab, pos, rotation, gameField);

        Text[] content = newPlayer.prefab.GetComponentsInChildren<Text>();

        for(int i=0; i< content.Length; i++)
        {
            if(content[i].name == "username")
                content[1].text = _userName; //User name
        }


        newPlayer.prefab.GetComponent<PlayerScript>().ID = _placeNum;
        newPlayer.prefab.name = "Player_" + _placeNum.ToString();

        opponent.Add(newPlayer);


    }

    public void LeaveRoom(int _placeNum)
    {
        string prefabName = "Player_" + _placeNum.ToString();

        Player playerToDestroy = null; 

        foreach (Player opn in opponent)
        {
            if (opn.prefab.name == prefabName)
            {
                Destroy(opn.prefab.gameObject);
                playerToDestroy = opn;
            }
        }

        if(playerToDestroy != null)
        {
            opponent.Remove(playerToDestroy);
        }

       /*
        Component[] _children = gameField.GetComponentsInChildren<Component>();
        foreach (Component child in _children)
        {
            if(child.name == playerName)
            {
                Destroy(child.gameObject);
            }         
        }*/
    }

    public void Disconect()
    {
        Client.instance.Disconnect();
    }


    //=========GAME MANAGER ===============

    public void Preflop(Card card1, Card card2)
    {
        
        player.ShowPreflop(card1, card2);
    }
}
