using Unity.Netcode;
using UnityEngine;

public class NetworkObjectHealth : NetworkBehaviour
{
    [SerializeField] public int maxHealth = 100;
    public NetworkVariable<int> _health = new NetworkVariable<int>();

    void Start() => SetMaxHealthServerRpc();


    public void GetDamage(int damage) => GetDamageServerRpc(damage);

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
    private void SetMaxHealthServerRpc() => _health.Value = maxHealth;


    [ServerRpc]
    private void DespawnServerRpc() => GetComponent<NetworkObject>().Despawn();
}
