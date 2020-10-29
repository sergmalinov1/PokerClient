using Assets.Scripts.Cards;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived(); 
    }

    public static void AuthAnswer(Packet _packet)
    {
        string code = _packet.ReadString();
        int num = _packet.ReadInt();

        Client.instance.Authorization(code);
    }


    public static void NewSpectator(Packet _packet)
    {
        int _myId = _packet.ReadInt();
        string _username = _packet.ReadString();

       // Debug.Log($"NewSpectator: {_myId} + __ + {_username}");

    }

    public static void PlayerInRoom(Packet _packet)
    {
        Debug.Log($"NewPlayer");
        int _playerId = _packet.ReadInt();
        string _playerName = _packet.ReadString();
        PlayerStatus _playerStatus = (PlayerStatus)_packet.ReadInt();
        int _placeNum = _packet.ReadInt();

        UIManagerGame.instance.ShowMsgToChat("servise", _playerName);
        UIManagerGame.instance.NewOpponent(_playerId, _placeNum, _playerName, _playerStatus);
       

    }

    public static void NewPlayerJoins(Packet _packet)
    {
        int _playerId = _packet.ReadInt();
        string _playerName = _packet.ReadString();
        int _placeNum = _packet.ReadInt();

        UIManagerGame.instance.ShowMsgToChat("service", _playerName);

        if(_playerId == Client.instance.myId)
            UIManagerGame.instance.NewPlayer(_playerId, _placeNum, _playerName);
        else
            UIManagerGame.instance.NewOpponent(_playerId, _placeNum, _playerName);

    }

    public static void GetChatMsg(Packet _packet)
    {
        
        string _user = _packet.ReadString();
        string _msg = _packet.ReadString();

     // Debug.Log($"User {_user} + __ + {_msg}");

        UIManagerGame.instance.ShowMsgToChat(_user, _msg);
    }

    public static void PlayerLeaveRoom(Packet _packet)
    {
        Debug.Log($"PlayerLeaveRoom");
        string _user = _packet.ReadString();
        int _placeNum = _packet.ReadInt();
        UIManagerGame.instance.LeaveRoom(_placeNum);

    }


    public static void PlayerInGame(Packet _packet)
    {
      
        int idPlayer = _packet.ReadInt();

        UIManagerGame.instance.ShowCover(idPlayer);
    }


    public static void Preflop(Packet _packet)
    {
       // string gameStatus;
        CardSuit firstCardSuit = (CardSuit)_packet.ReadInt();
        CardType firstCardType = (CardType)_packet.ReadInt();

        CardSuit secondCardSuit = (CardSuit)_packet.ReadInt();
        CardType secondCardType = (CardType)_packet.ReadInt();

        Card card1 = new Card(firstCardSuit, firstCardType);
        Card card2 = new Card(secondCardSuit, secondCardType);

        UIManagerGame.instance.Preflop(card1, card2);

    //    string cardImg1 = card1.ToFilename();
     //   string cardImg2 = card2.ToFilename();

      //  string cards = "Preflop: " + card1.ToString() + " -- " + card2.ToString();
      //  Debug.Log(cards);

     //   UIManagerGame.instance.ShowMsgToChat("server", cards);


    }

    public static void ActivePlayer(Packet _packet)
    {
        int idActivePlayer = _packet.ReadInt();

        if(idActivePlayer == Client.instance.myId)
        {
            UIManagerGame.instance.ShowMsgToChat("server", "ваш ход блядь ");
            UIManagerGame.instance.ShowRatesBtn();

        }
        else
        {
            UIManagerGame.instance.ShowMsgToChat("server", "ходит игрок " + idActivePlayer.ToString());
        }
    }

    public static void PlayerBet(Packet _packet)
    {
        int idPlayer = _packet.ReadInt();
        int rate = _packet.ReadInt();

        if(idPlayer != Client.instance.myId)
        {
            UIManagerGame.instance.ShowMsgToChat("server", "игрок " + idPlayer.ToString() + " ставка: " + rate.ToString());

            //устанавливаем в кнопки значения
            // в первую = rate
            //во вторую = rate х2
            //проверка на баланс игрока

        }
    }
}
