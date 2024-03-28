using Unity.Netcode;
using UnityEngine;

public class NetworkObjectHealth : NetworkBehaviour
{
    [SerializeField] public int maxHealth = 100;
    public NetworkVariable<int> _health = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server);

    void Start()
    {
        if (IsServer)
            _health.Value = maxHealth;
    }

    public void GetDamage(int damage)
    {
        if (IsServer)
            GetDamageServerRpc(damage);
    }

    public void Dead()
    {
        if (IsOwner)
            DespawnServerRpc();
    }


    [ServerRpc(RequireOwnership = false)]
    private void GetDamageServerRpc(int damage)
    {
        _health.Value -= damage;

        if (_health.Value <= 0)
            Dead();
    }


    [ServerRpc(RequireOwnership = false)]
    private void DespawnServerRpc() => GetComponent<NetworkObject>().Despawn();
}
