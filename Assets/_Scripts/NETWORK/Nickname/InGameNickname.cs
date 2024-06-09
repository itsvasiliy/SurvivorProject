using TMPro;
using Unity.Netcode;
using UnityEngine;

public class InGameNickname : NetworkBehaviour
{
    [SerializeField] private GameObject nickname;
    [SerializeField] private TextMeshProUGUI nicknameText;


    private void Start()
    {
        if (IsOwner)
        {
            Destroy(nickname);
        }
        else
        {
            SetPlayerUINicknameServerRpc(RelayServerDataManagerSingleton.playerName);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerUINicknameServerRpc(string _nickname) => SetPlayerUINicknameClientRpc(_nickname);

    [ClientRpc]
    private void SetPlayerUINicknameClientRpc(string _nickname) => nicknameText.text = _nickname;
}
