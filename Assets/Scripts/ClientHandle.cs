using Assets.Scripts.Cards;
using UnityEditor;
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
        Debug.Log($"AuthAnswer");
        string code = _packet.ReadString();
        int num = _packet.ReadInt();

        Client.instance.Authorization(code);
    }


    public static void GetChatMsg(Packet _packet)
    {

        string _user = _packet.ReadString();
        string _msg = _packet.ReadString();

        // Debug.Log($"User {_user} + __ + {_msg}");

        UIManagerGame.instance.ShowMsgToChat(_user, _msg);
    }


    
    public static void PlayerInRoom(Packet _packet) //для нового пользователя отправляеем список всех пользователей в комнтае
    {
      //  Debug.Log($"PlayerInRoom");
        int _playerId = _packet.ReadInt();
        string _playerName = _packet.ReadString();
        PlayerStatus _playerStatus = (PlayerStatus)_packet.ReadInt();
        int _placeNum = _packet.ReadInt();

        UIManagerGame.instance.ShowMsgToChat("servise", _playerName);
        UIManagerGame.instance.NewOpponent(_playerId, _placeNum, _playerName, _playerStatus);
        //  UIManagerGame.instance.NewPlayer(_playerId, _placeNum, _playerName);


    } 

    public static void NewPlayerJoins(Packet _packet) //отправляем всем сообщение что новый игрок присоеденился
    {
        Debug.Log($"NewPlayerJoins");
        int _playerId = _packet.ReadInt();
        string _playerName = _packet.ReadString();
        int _placeNum = _packet.ReadInt();

        UIManagerGame.instance.ShowMsgToChat("service", _playerName);

        if (_playerId == Client.instance.myId)
        {
         //   Debug.Log($"NewPlayer");
            UIManagerGame.instance.NewPlayer(_playerId, _placeNum, _playerName);
        }
        else
        {
         //   Debug.Log($"NewOpponent");
            UIManagerGame.instance.NewOpponent(_playerId, _placeNum, _playerName);
        }

    } 

    public static void StartNewGame(Packet _packet)
    {
        string status = _packet.ReadString();

        string msg = "===Новая игра===";
        UIManagerGame.instance.ShowMsgToChat("server", msg);
        UIManagerGame.instance.ClearTable();
    }


    public static void PlayerInGame(Packet _packet) //отправляем всем. что данный игрок играет в раздаче
    {

     //   Debug.Log($"PlayerInGame");
        int idPlayer = _packet.ReadInt();

        UIManagerGame.instance.ShowCover(idPlayer);
    } 


    public static void ActivePlayer(Packet _packet) //отправляем всем ID активного играка. тот который ходит
    {
        int idActivePlayer = _packet.ReadInt();

        UIManagerGame.instance.SelectActivePlayer(idActivePlayer);
    }  

    public static void PlayerStatus(Packet _packet)
    {
      //  Debug.Log($"PlayerStatus");
        int idPlayer = _packet.ReadInt();
        PlayerStatus status = (PlayerStatus)_packet.ReadInt();

        UIManagerGame.instance.SetStatusPlayer(idPlayer, status);
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

        string cards = "Preflop: " + card1.ToString() + " -- " + card2.ToString();
     //   Debug.Log(cards);

     //   UIManagerGame.instance.ShowMsgToChat("server", cards);


    }

    public static void CardOnDeck(Packet _packet)
    {
        // string gameStatus;
        CardSuit cardSuit = (CardSuit)_packet.ReadInt();
        CardType cardType = (CardType)_packet.ReadInt();  

        Card card = new Card(cardSuit, cardType);
        UIManagerGame.instance.PutCardOnTable(card);

    }


    public static void PlayerBet(Packet _packet)
    {
        //Debug.Log($"PlayerBet");
        int idPlayer = _packet.ReadInt();
        int rate = _packet.ReadInt();

        if (rate >= 0)
        {
          //  UIManagerGame.instance.minRate = rate;
        }

        if (idPlayer != Client.instance.myId)
        {
           // UIManagerGame.instance.ShowMsgToChat("server", "игрок " + idPlayer.ToString() + " ставка: " + rate.ToString());


            
            //устанавливаем в кнопки значения
            // в первую = rate
            //во вторую = rate х2
            //проверка на баланс игрока

        }
    }

    public static void PlayerLeaveRoom(Packet _packet)
    {
        //  Debug.Log($"PlayerLeaveRoom");
        string _user = _packet.ReadString();
        int _placeNum = _packet.ReadInt();
        UIManagerGame.instance.LeaveRoom(_placeNum);

    }

    public static void GameResult(Packet _packet)
    {
        //  Debug.Log($"PlayerLeaveRoom");
        string temp = _packet.ReadString();

        string msg = "";
        if(temp == "aaa")
            msg = "Победила Дружба!)) Потому что я еще не прикрутил проверку на победителя";
        else
            msg = "Победил игрок: " + temp.ToString();


        UIManagerGame.instance.ShowMsgToChat("server", msg);


    }

    /*
     
    public static void NewSpectator(Packet _packet)
    {
        int _myId = _packet.ReadInt();
        string _username = _packet.ReadString();

       // Debug.Log($"NewSpectator: {_myId} + __ + {_username}");

    }
     */
}
