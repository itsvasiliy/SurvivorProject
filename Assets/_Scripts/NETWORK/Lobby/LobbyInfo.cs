using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text lobbyName;
    [SerializeField] private TMP_Text playersCount;

    [SerializeField] private Image lobbyPrivacyIcon;

    private LobbyMenu lobbyMenu;

    private void Start()
    {
        lobbyMenu = GetComponentInParent<LobbyMenu>();

        if (lobbyMenu == null)
        {
            throw new System.NotImplementedException();
        }
    }

    public void LoadInfo(string lobbyName, int currentPlayerCount, int maxPlayers)
    {
        this.lobbyName.text = lobbyName;
        playersCount.text = (currentPlayerCount + "/" + maxPlayers).ToString();
    }

    public void SelectLobby()
    {
        lobbyMenu.JoinLobby();
    }
}
