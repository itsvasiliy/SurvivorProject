using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] private GameObject lobbiesBoard;
    [SerializeField] private GameObject joinedLobby;

    [SerializeField] private Transform lobbiesParent;

    [SerializeField] private LobbyInfo lobbyTemplate;

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

    public async Task CreateLobby(string lobbyName, int maxPlayers)
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);

            hostLobby = lobby;
            StartCoroutine(LobbyHeartbeat());

            Debug.Log("Created: " + lobby.Name + " " + lobby.MaxPlayers);

            lobbiesBoard.SetActive(false);
            joinedLobby.SetActive(true);

            joinedLobby.GetComponent<JoinedLobbyInfo>().LoadInfo(lobby.Name, lobby.Players.Count, lobby.MaxPlayers);

        }
        catch (LobbyServiceException error)
        {
            Debug.Log(error);
        }
    }

    public void JoinLobby()
    {
        joinedLobby.SetActive(true);
        lobbiesBoard.SetActive(false);
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
                Instantiate(lobbyTemplate, lobbiesParent);
                lobbyTemplate.LoadInfo(lobby.Name, lobby.Players.Count, lobby.MaxPlayers);

                //Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
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