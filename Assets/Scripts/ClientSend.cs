using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            _packet.Write(Client.instance.login);
            _packet.Write(Client.instance.pass);

            SendTCPData(_packet);
        }
    }


    public static void SendChatMsg()
    {
        using (Packet _packet = new Packet((int)ClientPackets.chatMsgReceived))
        {
            _packet.Write(Client.instance.login);
            _packet.Write(UIManagerGame.instance.chatInput.text);

            SendTCPData(_packet);
        }
    }


    public static void EnterTheRoom()
    {
        using (Packet _packet = new Packet((int)ClientPackets.enterTheRoom))
        {
            string join = "EnterTheRoom";
            string roomNum = "Room Number";

            _packet.Write(join);
            _packet.Write(roomNum);

            SendTCPData(_packet);
        }
    }  //вход в комнату 

    public static void JoinTheGame()
    {
        using (Packet _packet = new Packet((int)ClientPackets.joinTheGame))
        {
            string join = "JoinTheRoom";
            string roomNum = "Room Number";

            _packet.Write(join);
            _packet.Write(roomNum);

            SendTCPData(_packet);
        }
    } // 

    public static void Rate(int _rate)
    {
        using (Packet _packet = new Packet((int)ClientPackets.rate))
        {
           
            _packet.Write(_rate);

            SendTCPData(_packet);
        }
    }


    public static void Disconect()
    {
        using (Packet _packet = new Packet((int)ClientPackets.disconect))
        {
            string disconect = "disconect";

            _packet.Write(disconect);

            SendTCPData(_packet);
        }
    }

    #endregion
}
