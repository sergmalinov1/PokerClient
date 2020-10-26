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

    
    public InputField chatInput;
    public Transform chatContent;
    public GameObject chatMessage;

    public Button btnJoin;

    public GameObject[] place = new GameObject[4];
    public GameObject playerPrefab;
    public Transform gameField;

    private List<GameObject> players = new List<GameObject>();

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
       // Debug.Log("_plaseNum " + _placeNum);
        Vector3 pos = place[_placeNum].transform.position;
        Quaternion rotation = place[_placeNum].transform.rotation;
        GameObject newPlayer = Instantiate(playerPrefab, pos, rotation, gameField);

        Text[] content = newPlayer.GetComponentsInChildren<Text>();


        for(int i=0; i< content.Length; i++)
        {
            if(content[i].name == "username")
                content[1].text = _userName; //User name
        }
      

       

        newPlayer.GetComponent<PlayerScript>().ID = _placeNum;
        newPlayer.name = "Player_" + _placeNum.ToString();

        players.Add(newPlayer);
    }

    public void LeaveRoom(int _placeNum)
    {
        string prefabName = "Player_" + _placeNum.ToString();

        GameObject playerToDestroy = null; 

        foreach (GameObject player in players)
        {
            if (player.name == prefabName)
            {
                Destroy(player.gameObject);
                playerToDestroy = player;
            }
        }

        if(playerToDestroy != null)
        {
            players.Remove(playerToDestroy);
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
}
