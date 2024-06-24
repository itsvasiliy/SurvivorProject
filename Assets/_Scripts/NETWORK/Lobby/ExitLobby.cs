using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLobby : MonoBehaviour
{
    NetworkObject player;
    public async void ExitLobbyButton()
    {
        try
        {
            string playerId = AuthenticationService.Instance.PlayerId;
            await LobbyService.Instance.RemovePlayerAsync(RelayServerDataManagerSingleton.lobbyId, playerId);

            NetworkManager.Singleton.Shutdown();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }

        SceneManager.LoadScene("MainMenu");
    }


   
}