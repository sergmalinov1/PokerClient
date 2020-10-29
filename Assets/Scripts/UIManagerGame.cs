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

    public List<Player> opponent = new List<Player>();
    public Player player = new Player();
    public GameObject canvas;

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
        btnJoin.gameObject.SetActive(false);


        ClientSend.JoinTheRoom();
    }

    public void NewPlayer(int _idPlayer, int _placeNum, string _userName, PlayerStatus _playerStatus = PlayerStatus.inGame)
    {
     //   Debug.Log($"NewPlayer 222 " + _idPlayer.ToString());
        Vector3 pos = place[_placeNum].transform.position;
        Quaternion rotation = place[_placeNum].transform.rotation;



        player.prefab = Instantiate(playerPrefab, pos, rotation, gameField);

        Text[] content = player.prefab.GetComponentsInChildren<Text>();

        for (int i = 0; i < content.Length; i++)
        {
            if (content[i].name == "username")
                content[1].text = _userName; //User name
        }

        player.ID = _idPlayer;
        player.prefab.GetComponent<PlayerScript>().ID = _placeNum;
        player.prefab.name = "Player_" + _placeNum.ToString();

       // Debug.Log($"prefab.name {player.prefab.name}");

    }

    public void NewOpponent(int _idPlayer, int _placeNum, string _userName, PlayerStatus _playerStatus = PlayerStatus.fold)
    {
        //Debug.Log("NewOpponent _plaseNum " + _placeNum);
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

        newPlayer.ID = _idPlayer;
        newPlayer.prefab.GetComponent<PlayerScript>().ID = _placeNum;
        newPlayer.prefab.name = "Player_" + _placeNum.ToString();

        if (_playerStatus == PlayerStatus.inGame)
            newPlayer.ShowCover();

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

    public void SetRate(int rate)
    {
        HideRatesBtn();

        ClientSend.Rate(rate);
    }


    //=========GAME MANAGER ===============

    public void Preflop(Card card1, Card card2)
    {
        
        player.ShowPreflop(card1, card2);
    }

    public void ShowCover(int idPlayer)
    {
        Debug.Log($"UI ShowCover");
        foreach (Player pl in opponent)
        {
            if (pl.ID == idPlayer)
                pl.ShowCover();

        }
    }

    public void ShowRatesBtn()
    {
        GameObject btnGroup = GameObject.Find("RatesBtn");

        Button[] buttons = btnGroup.GetComponentsInChildren<Button>(true);

        foreach(Button btn in buttons)
        {
           // Debug.Log($"Button {btn.name}");
            btn.gameObject.SetActive(true);
        }

    }

    public void HideRatesBtn()
    {
        GameObject btnGroup = GameObject.Find("RatesBtn");

        Button[] buttons = btnGroup.GetComponentsInChildren<Button>(true);

        foreach (Button btn in buttons)
        {
            // Debug.Log($"Button {btn.name}");
            btn.gameObject.SetActive(false);
        }

    }
}
