using Unity.Netcode;
using UnityEngine;

public class PlayerHealthController : NetworkBehaviour, IDamageable, IHealthController
{
    [SerializeField] public int maxHealth;

    [SerializeField] MonoBehaviour playerMovementScript;
    [SerializeField] MonoBehaviour playerShooting;
    [SerializeField] Transform playerTransform;

    [SerializeField] Collider playerCollider;

    [SerializeField] GameObject respawnButton;

    private Animator animator;

    private int bulletIgnoreLayer = 128; //128 - bulletIgnoreLayer, it is layer 7

    private NetworkVariable<int> _health = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server);
    private NetworkVariable<bool> _isDead = new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Owner);

    void Start()
    {

        if (IsServer)
            _health.Value = maxHealth;
        _isDead.Value = false;

        animator = GetComponent<Animator>();
    }

    public bool IsHealthMax() => maxHealth == _health.Value;
    public void Heal(int value)
    {
        _health.Value += value;
        if (_health.Value > maxHealth)
            _health.Value = maxHealth;
    }

    public void GetDamage(int damage) => ((IDamageable)this).GetDamage(damage);
    void IHealthController.GetDamage(int damage) => ((IDamageable)this).GetDamage(damage);

    void IDamageable.GetDamage(int damage, ResourceController resourceController = null)
    {
        if (!IsOwner)
            return;

        GetDamageServerRpc(damage);

        if (_health.Value <= 0)
            Dead();
    }


    [ServerRpc(RequireOwnership = false)]
    private void GetDamageServerRpc(int damage) => _health.Value -= damage;

    public void Dead(ResourceController resourceController = null)
    {
        if (!IsOwner)
            return;

        SetDeathStatus(true);
        respawnButton.SetActive(true);

        animator.SetBool("IsRunning", false);
        animator.SetTrigger("Death");

        _isDead.Value = true;

        DisableColliderClientRpc();
    }


    public void Respawn()
    {
        if (!IsOwner)
            return;

        var tentPosition = TentPlayerRespawner.GetLastTentPosition();
        if (tentPosition == Vector3.zero)
            Debug.Log("Need tent to respawn the player");

        PlayRespawnAnimationServerRpc();
        SetDeathStatus(false);
        respawnButton.SetActive(false);

        HealMaxServerRpc();
        playerTransform.position = tentPosition;

        _isDead.Value = false;
        EnableColliderClientRpc();
    }


    [ServerRpc(RequireOwnership = false)]
    private void HealMaxServerRpc() => _health.Value = maxHealth;


    [ServerRpc(RequireOwnership = false)]
    private void PlayRespawnAnimationServerRpc()
    {
        animator.ResetTrigger("Death");
        animator.SetTrigger("Respawn");
        PlayRespawnAnimationClientRpc();
    }

    [ClientRpc]
    private void PlayRespawnAnimationClientRpc()
    {
        animator.ResetTrigger("Death");
        animator.SetTrigger("Respawn");
    }

    [ClientRpc]
    private void DisableColliderClientRpc()
    {
        var ignoreBulletsLayerMask = playerCollider.excludeLayers;
        ignoreBulletsLayerMask.value = bulletIgnoreLayer;
        playerCollider.excludeLayers = ignoreBulletsLayerMask;
    }

    [ClientRpc]
    private void EnableColliderClientRpc()
    {
        var ignoreBulletsLayerMask = playerCollider.excludeLayers;
        ignoreBulletsLayerMask.value = 0;
        playerCollider.excludeLayers = ignoreBulletsLayerMask;
    }

    private void SetDeathStatus(bool status)
    {
        playerMovementScript.enabled = !status;
        playerShooting.enabled = !status;
        this.enabled = !status;
    }


    public int GetMaxHealth() => maxHealth;
    public int GetCurrentHealth() => _health.Value;
    public NetworkVariable<int> GetHealthVariable() => _health;
    public bool IsAlive() => !_isDead.Value;
}
