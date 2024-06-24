using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLobby : MonoBehaviour
{
    public async void ExitLobbyButton()
    {
        try
        {
            string playerId = AuthenticationService.Instance.PlayerId;
            await LobbyService.Instance.RemovePlayerAsync(RelayServerDataManagerSingleton.lobbyId, playerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }

        SceneManager.LoadScene("MainMenu");
    }
}