using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] private GameObject lobbiesBoard;
    [SerializeField] private GameObject loadingScreen;

    [SerializeField] private Transform lobbiesParent;

    [SerializeField] private LobbyRelay lobbyRelay;
    [SerializeField] private LobbyInfo lobbyTemplate;
    [SerializeField] private LobbyHeartbeater lobbyHeartbeater;

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
    }

    private void OnEnable() => ListLobbies();

    public async Task CreateLobby(string lobbyName, int maxPlayers)
    {
        try
        {
            loadingScreen.SetActive(true);

            var joinCode = await lobbyRelay.InitRelay(maxPlayers);

            CreateLobbyOptions options = new CreateLobbyOptions();
            options.Data = new Dictionary<string, DataObject>()
                {
                    {
                        "joinCode", new DataObject(
                            visibility: DataObject.VisibilityOptions.Member,
                            value: joinCode)
                    },
                };


            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

            lobby.Data.TryGetValue("joinCode", out DataObject data);
            hostLobby = lobby;

            lobbyHeartbeater.StartLobbyHeartbeat(hostLobby.Id);
            SceneManager.LoadScene("_Map");
        }
        catch (LobbyServiceException error)
        {
            Debug.LogError(error);
        }
    }

    public async void JoinLobby(string lobbyID)
    {
        try
        {
            loadingScreen.SetActive(true);
            lobbiesBoard.SetActive(false);

            var joinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyID);
            joinedLobby.Data.TryGetValue("joinCode", out DataObject lobbyData);

            RelayServerDataManagerSingleton.relayCode = lobbyData.Value;
            RelayServerDataManagerSingleton.isHost = false;
            RelayServerDataManagerSingleton.lobbyId = lobbyID;

            SceneManager.LoadScene("_Map");
        }
        catch (LobbyServiceException error)
        {
            Debug.LogError(error);
        }

    }

    public async void LeaveTheLobby()
    {
        try
        {
            if (hostLobby != null)
            {
                await LobbyService.Instance.RemovePlayerAsync(hostLobby.Id, AuthenticationService.Instance.PlayerId);

                //   joinedLobbyBoard.SetActive(false);
                lobbiesBoard.SetActive(true);
            }
        }
        catch (LobbyServiceException error)
        {
            Debug.LogError(error);
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
            Debug.LogError(error);
        }
    }

    //private IEnumerator LobbyHeartbeat()
    //{
    //    while (true)
    //    {
    //        if (hostLobby != null)
    //        {
    //            yield return new WaitForSeconds(heartbeatTimer);

    //            LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
    //        }
    //    }
    //}

}