using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JoinedLobbyInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text lobbyName;
    [SerializeField] private TMP_Text countOfPlayers;

    public void LoadInfo(string lobbyName, int connectedPlayers, int maxPlayers)
    {
        this.lobbyName.text = lobbyName;
        countOfPlayers.text = (connectedPlayers + "/" + maxPlayers).ToString();
    }
}
