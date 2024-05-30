using System;
using Unity.Netcode;
using UnityEngine;

public class EnemyHealthController : NetworkBehaviour, IAimTarget, IHealthController
{
    [SerializeField] public int maxHealth;

    [SerializeField] MonoBehaviour movementScript;
    [SerializeField] MonoBehaviour shootingScript;

    private Animator animator;

    private NetworkVariable<int> _health = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server);

    private Action returnToPoolAction;

    private bool isDead;
    private bool isAttackable = true;

    private float bodyDisableTime = 3f;

    void Start()
    {
        if (IsServer)
            _health.Value = maxHealth;

        isDead = false;
        animator = GetComponent<Animator>();
    }

    public void SetReturnAction(Action _returnToPoolAction) => returnToPoolAction = _returnToPoolAction;

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
        SetDeathStatusServerRpc(true);
        animator.SetTrigger("Death");

        if (resourceController != null)
            DropResourcesInDeathCase(resourceController);

        SetColliderStatusColliderClientRpc(false);

        Invoke(nameof(ReturnToPool), bodyDisableTime);
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Respawn");
        SetColliderStatusColliderClientRpc(true);
        SetDeathStatusServerRpc(false);

        if (IsServer)
            _health.Value = maxHealth;
        isDead = false;
    }

    private void ReturnToPool() => returnToPoolAction?.Invoke();


    [ClientRpc]
    private void SetColliderStatusColliderClientRpc(bool status)
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = status;
    }


    [ServerRpc(RequireOwnership = false)]
    private void SetDeathStatusServerRpc(bool status)
    {
        movementScript.enabled = !status;
        shootingScript.enabled = !status;
        SetDeathStatusClientRpc(!status);
    }

    [ClientRpc]
    private void SetDeathStatusClientRpc(bool status)
    {
        movementScript.enabled = status;
        shootingScript.enabled = status;
    }

    private void DropResourcesInDeathCase(ResourceController resourceController)
    {
        var dropScript = GetComponent<DropResourcesOnDeath>();
        if (dropScript != null)
            dropScript.DropResources(resourceController);
        else
            Debug.Log($"Now resources drop from {name}");
    }

    private void OnEnable()
    {
        if (IsSpawned)
            SetActiveSelfStatusClientRpc(true);
    }

    private void OnDisable()
    {
        if (IsSpawned && isDead)
            DisableActiveSelfStatus();
    }
  

    [ClientRpc]
    private void SetActiveSelfStatusClientRpc(bool status) => gameObject.SetActive(status);
    private void DisableActiveSelfStatus() => SetActiveSelfStatusClientRpc(false);
    public int GetMaxHealth() => maxHealth;
    public int GetCurrentHealth() => _health.Value;
    public NetworkVariable<int> GetHealthVariable() => _health;
    bool IAimTarget.IsEnabled() => this.enabled;
    public bool IsAlive() => !isDead;
    public void SetAttackableStatus(bool status) => isAttackable = status;
    public bool IsAttackable() => isAttackable;
}
