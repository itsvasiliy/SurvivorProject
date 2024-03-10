using Unity.Netcode;
using UnityEngine;

public class NetworkObjectHealth : NetworkBehaviour
{
    [SerializeField] public int maxHealth = 100;
    private NetworkVariable<int> _health = new NetworkVariable<int>();

    void Start() => _health.Value = maxHealth;


    public void GetDamage(int damage)
    {
        _health.Value -= damage;

        if (_health.Value <= 0)
            Dead();
    }

    public void Dead()
    {
        if (IsOwner)
            DespawnServerRpc();
    }

    [ServerRpc]
    private void DespawnServerRpc() => GetComponent<NetworkObject>().Despawn();
}
