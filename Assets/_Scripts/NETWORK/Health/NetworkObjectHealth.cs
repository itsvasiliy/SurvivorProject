using Unity.Netcode;
using UnityEngine;

public class NetworkObjectHealth : NetworkBehaviour, IHealthController
{
    [SerializeField] public int maxHealth = 100;

    [HideInInspector]
    public NetworkVariable<int> _health = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server);

    private bool isDead;

    void Start()
    {
        if (IsServer)
            _health.Value = maxHealth;
        isDead = false;
    }

    public void GetDamage(int damage)
    {
        if (IsServer)
            GetDamageServerRpc(damage);
    }

    public void Death()
    {
        if (IsOwner)
            DespawnServerRpc();
        isDead = true;
    }


    [ServerRpc(RequireOwnership = false)]
    private void GetDamageServerRpc(int damage)
    {
        _health.Value -= damage;

        if (_health.Value <= 0)
            Death();
    }


    [ServerRpc(RequireOwnership = false)]
    private void DespawnServerRpc() => GetComponent<NetworkObject>().Despawn();
    public int GetMaxHealth() => maxHealth;
    public int GetCurrentHealth() => _health.Value;
    public NetworkVariable<int> GetHealthVariable() => _health;
    public bool IsAlive() => !isDead;


}
