using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class LobbyMaker : MonoBehaviour
{
    [SerializeField] private TMP_InputField lobbyNameInputField;

    [SerializeField] private Number_IncreaseDecrease maxPlayersNumber;

    //public async void CreateLobby123()
    //{
    //    await CreateLobby();
    //}

    public async void CreateLobby()
    {
        string lobbyName;
        lobbyName = lobbyNameInputField.text;

        LobbyMenu lobbyMenu = GetComponentInParent<LobbyMenu>();

        if(lobbyMenu != null)
        {
            await lobbyMenu.CreateLobby(lobbyName, maxPlayersNumber.GetCurrentNumber());
        }
        else
        {

        }
    }
}