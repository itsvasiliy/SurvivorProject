using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Unity.Netcode;


public class MainButton_MainMenu : NetworkBehaviour
{
    [SerializeField] private GameObject lobbiesMenu;
    [SerializeField] private GameObject createLobbyMenu;

    public async void StartGame()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            if (queryResponse.Results.Count == 0)
            {
                lobbiesMenu.SetActive(true);
                createLobbyMenu.SetActive(true);
            }
            else
            {
                lobbiesMenu.SetActive(true);
            }
        }
        catch(LobbyServiceException error)
        {
            Debug.LogError(error.Message);
        }
    }
}