using Unity.Netcode;
using UnityEngine;

public class EnemyHealthController : NetworkBehaviour, IAimTarget, IHealthController
{
    [SerializeField] public int maxHealth;
    [SerializeField] MonoBehaviour movementScript;
    [SerializeField] MonoBehaviour shootingScript;

    private Animator animator;

    private NetworkVariable<int> _health = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server);


    void Start()
    {
        if (IsServer)
            _health.Value = maxHealth;

        animator = GetComponent<Animator>();
    }


    public void GetDamage(int damage)
    {
        GetDamageServerRpc(damage);

        if (_health.Value <= 0)
            Dead();
    }


    [ServerRpc(RequireOwnership = false)]
    private void GetDamageServerRpc(int damage) => _health.Value -= damage;

    public void Dead()
    {
        SetDeathStatusServerRpc(false);
        animator.SetTrigger("Death");
        Invoke(nameof(DespawnServerRpc), 4.5f);
    }


    [ServerRpc(RequireOwnership = false)]
    private void DespawnServerRpc() => GetComponent<NetworkObject>().Despawn();


    [ServerRpc(RequireOwnership = false)]
    private void SetDeathStatusServerRpc(bool status)
    {
        movementScript.enabled = status;
        shootingScript.enabled = status;
        this.enabled = status;
        SetDeathStatusClientRpc(status);
    }

    [ClientRpc]
    private void SetDeathStatusClientRpc(bool status)
    {
        movementScript.enabled = status;
        shootingScript.enabled = status;
        this.enabled = status;
    }


    public int GetMaxHealth() => maxHealth;

    public int GetCurrentHealth() => _health.Value;

    public NetworkVariable<int> GetHealthVariable() => _health;

    bool IAimTarget.IsEnabled() => this.enabled;
}
