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
    public Transform cardOnDeck;

    private int colCardOnDesk = 0;

    private List<Player> opponent = new List<Player>();
    private Player player = new Player();
    public GameObject canvas;

    public int minRate = 0; 

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


        ClientSend.JoinTheGame();
    }

    public void NewPlayer(int _idPlayer, int _placeNum, string _userName, PlayerStatus _playerStatus = PlayerStatus.inGame)
    {
        
        Vector3 pos = place[_placeNum].transform.position;
        Quaternion rotation = place[_placeNum].transform.rotation;



        player.prefab = Instantiate(playerPrefab, pos, rotation, gameField);

  //      Debug.Log($"NewPlayer 222 " + _idPlayer.ToString());

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
        Debug.Log("NewOpponent _plaseNum " + _placeNum);
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

    public void PutCardOnTable(Card card)
    {

        string fileName = card.ToFilename();

       // Debug.Log($"PutCardOnTable: {fileName}");

        Texture2D img = Resources.Load("Cards/" + fileName) as Texture2D;

        RawImage[] rawImage = cardOnDeck.GetComponentsInChildren<RawImage>();

        if (colCardOnDesk < 5)
        {
            rawImage[colCardOnDesk].texture = img;
            colCardOnDesk++;
        }
    }

    public void ClearTable()
    {
        colCardOnDesk = 0;
        Texture2D img = Resources.Load("Cards/cover") as Texture2D;

        RawImage[] rawImage = cardOnDeck.GetComponentsInChildren<RawImage>();

        foreach(RawImage ri in rawImage)
        {
            ri.texture = img;
        }
    }

    public void ShowCover(int idPlayer)
    {
    //    Debug.Log($"UI ShowCover");
        foreach (Player pl in opponent)
        {
            if (pl.ID == idPlayer)
                pl.ShowCover();

        }
    }

    public void SelectActivePlayer(int idActivePlayer)
    {       

        if (idActivePlayer == Client.instance.myId)
        {
          //  ShowMsgToChat("server", "ваш ход блядь! ");
            ShowRatesBtn();
            UnselectActivePlayerAll();
            player.EnableActiveState();

        }
        else
        {
          //  ShowMsgToChat("server", "ходит игрок " + idActivePlayer.ToString());
            Player pl = GetOpponentById(idActivePlayer);
            UnselectActivePlayerAll();
            pl.EnableActiveState();
        }
    }

    private void UnselectActivePlayerAll()
    {
        player.DisableActiveState();
        foreach (Player item in opponent)
        {
            item.DisableActiveState();
        }
    }

    public void ShowRatesBtn()
    {
        GameObject btnGroup = GameObject.Find("RatesBtn");

        Button[] buttons = btnGroup.GetComponentsInChildren<Button>(true);

        foreach(Button btn in buttons)
        {
            // if (btn.name == "check" && minRate > 0)
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



    public void SetStatusPlayer(int idPlayer, PlayerStatus playerStatus)
    {
        //   Debug.Log($"SetStatusPlayer {idPlayer}");

      

        if (Client.instance.myId == idPlayer)
        {
            ShowMsgToChat("server", "Вы сбросили карты");
            player.Fold();
            
        }
        else
        {
            foreach (Player pl in opponent)
            {
                if (pl.ID == idPlayer)
                {
                    if (playerStatus == PlayerStatus.fold)
                    {
                        ShowMsgToChat("server", "Игрок " + pl.name + " сбросил карты");
                        pl.Fold();
                        
                    }
                }
            }
        }
    }


    private Player GetOpponentById(int idPlayer)
    {
        Player _pl = null;
        foreach (Player item  in opponent)
        {

            if (item.ID == idPlayer)
            {
                _pl = item;
            }
        }
        return _pl;
    }
}
