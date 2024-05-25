using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(nickname_TMP_InputField.text);

        await UnityServices.InitializeAsync(initializationOptions);
    }
}