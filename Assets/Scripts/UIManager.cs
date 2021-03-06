﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;

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

    public void ConnectToServer()
    {
        //  startMenu.SetActive(false);
        // usernameField.interactable = false;
       // Debug.Log("ConnectToServer");
        Client.instance.login = usernameField.text;
        Client.instance.ConnectToServer();
    }


    public void ConnectToServerStatic(string _username)
    {
        //  startMenu.SetActive(false);
        // usernameField.interactable = false;
        // Debug.Log("ConnectToServer");
        Client.instance.login = _username;
        Client.instance.ConnectToServer();
    }

    public void Disconect()
    {
       // Client.instance.Disconnect();
    }
}
