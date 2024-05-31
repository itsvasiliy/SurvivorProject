using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class ChangeNickname : MonoBehaviour
{
    [SerializeField] private TMP_Text nickname;

    [SerializeField] private TMP_InputField nickname_TMP_InputField;

    [SerializeField] private GameObject invalidTextMessage;

    [SerializeField] private Button saveBtn;


    private async void Start()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        LoadPlayerName();
    }


    public async void LoadPlayerName() => nickname.text = await AuthenticationService.Instance.GetPlayerNameAsync();
    public void SetPlayerName(string _name) => nickname.text = _name; //use instead of LoadPlayerName because AuthenticationService has rate limit for requests


    public async void SaveNewNickname()
    {
        var _name = nickname_TMP_InputField.text;
        await AuthenticationService.Instance.UpdatePlayerNameAsync(_name);
        SetPlayerName(_name);
    }

    
    public void ValidateNickname()
    {
        var _name = nickname_TMP_InputField.text;

        if (IsNameValid(_name))
        {
            invalidTextMessage.SetActive(false);
            saveBtn.interactable = true;
        }
        else
        {
            invalidTextMessage.SetActive(true);
            saveBtn.interactable = false;
        }
    }


    //name must be not null, don't contain spaces and should have a maximum length of 50 characters
    private bool IsNameValid(string _name)
    {
        return !string.IsNullOrWhiteSpace(_name) && _name.Length <= 50 && !_name.Contains(" ");
    }


    public void ClearBoard()
    {
        nickname_TMP_InputField.text = "";
        invalidTextMessage.SetActive(false);
    }
}