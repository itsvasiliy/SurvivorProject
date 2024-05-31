using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class ChangeNickname : MonoBehaviour
{
    [SerializeField] private TMP_Text nickname;

    [SerializeField] private TMP_InputField nickname_TMP_InputField;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        LoadPlayerName();
    }

    public async void LoadPlayerName()
    {
        nickname.text = await AuthenticationService.Instance.GetPlayerNameAsync();
    }

    public async void SaveNewNickname()
    {
        await AuthenticationService.Instance.UpdatePlayerNameAsync(nickname_TMP_InputField.text);
        LoadPlayerName();
    }
}