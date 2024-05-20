using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyMenu : MonoBehaviour
{
    private Lobby hostLobby; 
    private float heartbeatTimer = 15f;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Singed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "New lobby";
            int maxPlayers = 4;

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);

            hostLobby = lobby;
            StartCoroutine(LobbyHeartbeat());

            Debug.Log("Created: " + lobby.Name + " " + lobby.MaxPlayers);
        }
        catch (LobbyServiceException error)
        {
            Debug.Log(error);
        }
    }

    public void LeaveTheLobby()
    {

    }



    public async void ListLobbies()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);

            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        }
        catch (LobbyServiceException error)
        {
            Debug.Log(error);
        }
    }


    private IEnumerator LobbyHeartbeat()
    {
        while(true)
        {
            if(hostLobby != null)
            {
                yield return new WaitForSeconds(heartbeatTimer);

                LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

}