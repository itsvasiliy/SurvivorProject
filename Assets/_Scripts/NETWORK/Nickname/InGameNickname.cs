using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class InGameNickname : NetworkBehaviour
{
    [SerializeField] private GameObject nickname;
    [SerializeField] private TextMeshProUGUI nicknameText;

    private NetworkVariable<FixedString128Bytes> nick = new NetworkVariable<FixedString128Bytes>(writePerm: NetworkVariableWritePermission.Owner);


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        nick.OnValueChanged += OnNicknameChanged;

        SetPlayerUINickname(RelayServerDataManagerSingleton.playerName);

        UpdateNicknameText(nick.Value.ToString());

    }
    private void UpdateNicknameText(string nickname) => nicknameText.text = nickname;


    private void OnNicknameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue) => nicknameText.text = newValue.ToString();

    private void SetPlayerUINickname(string _nickname)
    {
        if (IsOwner)
            nick.Value = _nickname;
    }

    private void OnDisable() => nick.OnValueChanged -= OnNicknameChanged;
}
