using UnityEngine;
using TMPro;

public class LobbyMaker : MonoBehaviour
{
    [SerializeField] private TMP_InputField lobbyNameInputField;

    [SerializeField] private Number_IncreaseDecrease maxPlayersNumber;

    public async void CreateLobby()
    {
        string lobbyName;

        if (string.IsNullOrEmpty(lobbyNameInputField.text))
        {
            lobbyName = "New lobby";
        }
        else
        {
            lobbyName = lobbyNameInputField.text;
        }

        LobbyMenu lobbyMenu = GetComponentInParent<LobbyMenu>();
        await lobbyMenu.CreateLobby(lobbyName, maxPlayersNumber.GetCurrentNumber());
    }
}