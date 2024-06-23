using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;
using Unity.Services.Authentication;

public class ExitLobby : MonoBehaviour
{
    public void ExitLobbyButton()
    {
        //try
        //{
        //    string playerId = AuthenticationService.Instance.PlayerId;
        //    await LobbyService.Instance.RemovePlayerAsync(" fuck) ", playerId);
        //}
        //catch (LobbyServiceException e)
        //{
        //    Debug.Log(e);
        //}

        SceneManager.LoadScene("MainMenu");
    }
}