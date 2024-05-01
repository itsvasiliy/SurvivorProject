using Unity.Netcode;
using UnityEngine;

public class StructureHealthController : NetworkBehaviour, IDamageable, IHealthController
{
    [SerializeField] public int maxHealth;

    private NetworkVariable<int> _health = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server);

    private bool isDead;


    void Start()
    {
        if (IsServer)
            _health.Value = maxHealth;

        isDead = false;
    }

    public void GetDamage(int damage, ResourceController resourceController = null)
    {
        GetDamageServerRpc(damage);

        if (_health.Value <= 0)
            Dead();
    }


    [ServerRpc(RequireOwnership = false)]
    private void GetDamageServerRpc(int damage) => _health.Value -= damage;


    public void Dead(ResourceController resourceController = null)
    {
        if (isDead)
            return;

        isDead = true;
        DespawnServerRpc();
    }


    [ServerRpc(RequireOwnership = false)]
    private void DespawnServerRpc() => GetComponent<NetworkObject>().Despawn();




    public int GetCurrentHealth() => _health.Value;
    public NetworkVariable<int> GetHealthVariable() => _health;
    public int GetMaxHealth() => maxHealth;
    public bool IsAlive() => isDead;

}
