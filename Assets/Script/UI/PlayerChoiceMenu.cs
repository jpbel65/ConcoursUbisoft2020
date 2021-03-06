﻿using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Demos.DemoAnimator;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChoiceMenu : Photon.MonoBehaviour, IPunObservable
{
    [SerializeField] private PhotonView view;
    [SerializeField] private Button player1Button;
    [SerializeField] private Button player2Button;
    [SerializeField] private Button readyButton;
    [SerializeField] private GameObject player1Prefab;
    [SerializeField] private GameObject player2Prefab;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private Text playerChosenText;
    [SerializeField] private Text nbPlayerInRoomText;

    // private bool hasLoaded = false;
    private bool player1Selected = false;
    private bool player2Selected = false;
    private int ready = 0;

    private void Start()
    {
        readyButton.interactable = false;
        player1Button.Select();
    }

    private void Update()
    {
        if (player1Selected && player1Button.interactable)
        {
            player1Button.interactable = false;
        }
        
        if (player2Selected && player2Button.interactable)
        {
            player2Button.interactable = false;
        }
        
        if (ready == 2)
        {
            view.RPC("LoadGame", PhotonTargets.All);
        }
    }
    public void OnJoinedRoom()
    {
        //InitCustomPropreties();
        view.RPC("SetNbPlayerInRoomText", PhotonTargets.All);
    }

    [PunRPC]
    private void SetNbPlayerInRoomText()
    {
        nbPlayerInRoomText.text = PhotonNetwork.room.PlayerCount + " / " + PhotonNetwork.room.MaxPlayers +
                                  " joueurs dans la partie.";
    }

    private void InitCustomPropreties()
    {
        var p1ChosenProp = "p1chosen";
        var p2ChosenProp = "p2chosen";
        // Si le p1 ou le p2 n'a pas été initialisé, on le fait à false
        if (PhotonNetwork.room.CustomProperties[p1ChosenProp] == null)
        {
            PhotonNetwork.room.CustomProperties.Add(p1ChosenProp, false);    
        }
        if (PhotonNetwork.room.CustomProperties[p2ChosenProp] == null)
        {
            PhotonNetwork.room.CustomProperties.Add(p2ChosenProp, false);    
        } 
    }

    [PunRPC]
    public void LoadGame()
    {
        ready = 0;
        Debug.Log("Loading level");
        PhotonNetwork.LoadLevel(1);
    }

    public void Player1Choosen()
    {
        PlayerManager.LocalPlayerInstance = player1Prefab;
        playerChosenText.text = "Vous êtes le: " + PlayerManager.LocalPlayerInstance.name;
        player2Button.interactable = false;
        //PhotonNetwork.room.CustomProperties["p1chosen"] = true;
        
        readyButton.interactable = true;
        readyButton.Select();
        
       // view.RPC("updateChoices", PhotonTargets.All);
        view.RPC("DisableButtonPlayer1", PhotonTargets.AllBuffered);
        view.RPC("SelectPlayer2Button", PhotonTargets.OthersBuffered);
    }
    
    public void Player2Choosen()
    {
        PlayerManager.LocalPlayerInstance = player2Prefab;
        playerChosenText.text = "Vous êtes le: " + PlayerManager.LocalPlayerInstance.name;
        player1Button.interactable = false;
        //PhotonNetwork.room.CustomProperties["p2chosen"] = true;
        
        
        readyButton.interactable = true;
        readyButton.Select();
        
        // en fonction des choix dans le serveur, on vient mettre à jour les choix
        //view.RPC("updateChoices", PhotonTargets.All);
        view.RPC("DisableButtonPlayer2", PhotonTargets.AllBuffered);
        view.RPC("SelectPlayer1Button", PhotonTargets.OthersBuffered);
    }

    [PunRPC]
    private void updateChoices()
    {
        player1Selected = GetP1ChoiceState();
        player2Selected = GetP2ChoiceState();
    }

    private bool GetP1ChoiceState()
    {
        var p1ChosenProp = "p1chosen";
        if (PhotonNetwork.room != null)
        {
            var result = PhotonNetwork.room.CustomProperties[p1ChosenProp];
            if (result is bool b)
            {
                return b;
            } 
        }
        return false;
    }

    private bool GetP2ChoiceState()
    {
        var p2ChosenProp = "p2chosen";
        if (PhotonNetwork.room != null)
        {
            var result = PhotonNetwork.room.CustomProperties[p2ChosenProp];
            if (result is bool b)
            {
                return b;
            } 
        }
        return false;
    }

    [PunRPC] 
    public void DisableButtonPlayer1()
    {
        player1Button.interactable = false;
       
    }
    
    [PunRPC]
    public void DisableButtonPlayer2()
    {
        player2Button.interactable = false;
    }

    [PunRPC]
    public void SelectPlayer2Button()
    {
        player2Button.Select();
    }
    
    [PunRPC]
    public void SelectPlayer1Button()
    {
        player1Button.Select();
    }

    public void Ready()
    {
        readyButton.interactable = false;
        setReady();
        view.RPC("setReady", PhotonTargets.Others);
    }

    [PunRPC]
    public void setReady()
    {
        ready += 1;
    }
    
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
             stream.SendNext(player1Selected);
             stream.SendNext(player2Selected);
        } else if (stream.isReading)
        {
             player1Selected = (bool) stream.ReceiveNext();
             player2Selected = (bool) stream.ReceiveNext();
        }
    }
}
