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

    [SerializeField] private Sprite lockedSprite;

    private LobbyMenu lobbyMenu;

    private int maxPlayersCount;

    private void Start()
    {
        lobbyMenu = GetComponentInParent<LobbyMenu>();

        if (lobbyMenu == null)
        {
            throw new System.NotImplementedException();
        }
    }

    public void LoadInfo(string lobbyName, int currentPlayerCount, int maxPlayers, bool isPrivate)
    {
        this.lobbyName.text = lobbyName;
        playersCount.text = (currentPlayerCount + "/" + maxPlayers).ToString();

        if(isPrivate == true)
        {
            lobbyPrivacyIcon.sprite = lockedSprite;
        }
    }

    public void SelectLobby()
    {
        lobbyMenu.JoinLobby();
    }
}
