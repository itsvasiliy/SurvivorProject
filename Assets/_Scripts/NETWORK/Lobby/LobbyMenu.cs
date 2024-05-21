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
    [SerializeField] private GameObject joinedLobbyBoard;

    [SerializeField] private Transform lobbiesParent;

    [SerializeField] private LobbyInfo lobbyTemplate;

    private Dictionary<string, GameObject> instantiatedLobbies = new Dictionary<string, GameObject>();

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

        ListLobbies();
    }

    public async Task CreateLobby(string lobbyName, int maxPlayers)
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);

            hostLobby = lobby;
            StartCoroutine(LobbyHeartbeat());

            joinedLobbyBoard.SetActive(true);
            lobbiesBoard.SetActive(false);


            joinedLobbyBoard.GetComponent<JoinedLobbyInfo>().LoadInfo(lobby.Name, lobby.Players.Count, lobby.MaxPlayers);
        }
        catch (LobbyServiceException error)
        {
            Debug.Log(error);
        }
    }

    public async void JoinLobby(string lobbyID)
    {
        try
        {
            joinedLobbyBoard.SetActive(true);
            lobbiesBoard.SetActive(false);

            await Lobbies.Instance.JoinLobbyByIdAsync(lobbyID);
        }
        catch(LobbyServiceException error)
        {
            Debug.Log(error);
        }

    }

    public async void LeaveTheLobby()
    {
        try
        {
            if (hostLobby != null)
            {
                await LobbyService.Instance.RemovePlayerAsync(hostLobby.Id, AuthenticationService.Instance.PlayerId);

                joinedLobbyBoard.SetActive(false);
                lobbiesBoard.SetActive(true);
            }
        }
        catch (LobbyServiceException error)
        {
            Debug.Log(error);
        }
    }
    
    public async void ListLobbies()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);

            foreach (Lobby lobby in queryResponse.Results)
            {
                if (!instantiatedLobbies.ContainsKey(lobby.Id))
                {
                    LobbyInfo newLobby = Instantiate(lobbyTemplate, lobbiesParent);
                    newLobby.LoadInfo(lobby.Id, lobby.Name, lobby.Players.Count, lobby.MaxPlayers);

                    instantiatedLobbies.Add(lobby.Id, newLobby.gameObject);
                }
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