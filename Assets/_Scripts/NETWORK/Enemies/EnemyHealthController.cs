using Unity.Netcode;
using UnityEngine;

public class EnemyHealthController : NetworkBehaviour, IAimTarget, IHealthController
{
    [SerializeField] public int maxHealth;
    [SerializeField] MonoBehaviour movementScript;
    [SerializeField] MonoBehaviour shootingScript;

    private Animator animator;

    private NetworkVariable<int> _health = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server);

    private bool isDead;


    void Start()
    {
        if (IsServer)
            _health.Value = maxHealth;

        isDead = false;
        animator = GetComponent<Animator>();
    }

    public void GetDamage(int damage) => ((IDamageable)this).GetDamage(damage);
    void IHealthController.GetDamage(int damage) => ((IDamageable)this).GetDamage(damage);
    public void GetDamage(int damage, ResourceController resourceController = null)
    {
        if (IsOwner)
            GetDamageServerRpc(damage);

        if (_health.Value <= 0)
            Dead(resourceController);
    }


    [ServerRpc(RequireOwnership = true)]
    private void GetDamageServerRpc(int damage) => _health.Value -= damage;

    public void Dead(ResourceController resourceController = null)
    {
        if (isDead)
            return;

        isDead = true;
        SetDeathStatusServerRpc(false);
        animator.SetTrigger("Death");

        if (resourceController != null)
            DropResourcesInDeathCase(resourceController);

        DisableColliderClientRpc();

        Invoke(nameof(DespawnServerRpc), 4.5f);
    }


    [ServerRpc(RequireOwnership = false)]
    private void DespawnServerRpc() => GetComponent<NetworkObject>().Despawn();

    [ClientRpc]
    private void DisableColliderClientRpc()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
    }


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

    private void DropResourcesInDeathCase(ResourceController resourceController)
    {
        var dropScript = GetComponent<DropResourcesOnDeath>();
        if (dropScript != null)
            dropScript.DropResources(resourceController);
        else
            Debug.Log($"Now resources drop from {name}");
    }


    public int GetMaxHealth() => maxHealth;

    public int GetCurrentHealth() => _health.Value;

    public NetworkVariable<int> GetHealthVariable() => _health;

    bool IAimTarget.IsEnabled() => this.enabled;

    public bool IsAlive() => !isDead;
}
