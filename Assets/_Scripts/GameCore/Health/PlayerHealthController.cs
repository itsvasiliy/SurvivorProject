using Unity.Netcode;
using UnityEngine;

public class PlayerHealthController : NetworkBehaviour, IDamageable, IHealthController
{
    [SerializeField] public int maxHealth;
    [SerializeField] PlayerMovement playerMovementScript;
    [SerializeField] AnimationClip getHitClip;


    [SerializeField] GameObject respawnButton;

    private Animator animator;

    private NetworkVariable<int> _health = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server);


    void Start()
    {
        if (IsServer)
            _health.Value = maxHealth;

        animator = GetComponent<Animator>();
    }

    public bool IsHealthMax() => maxHealth == _health.Value;
    public void Heal(int value)
    {
        _health.Value += value;
        if (_health.Value > maxHealth)
            _health.Value = maxHealth;
    }


    public void GetDamage(int damage)
    {
        PlayGetHitAnimation();
        GetDamageServerRpc(damage);

        if (_health.Value <= 0)
            Dead();
    }


    [ServerRpc(RequireOwnership = false)]
    private void GetDamageServerRpc(int damage) => _health.Value -= damage;

    public void Dead()
    {
        if (!IsOwner)
            return;

        SetDeathStatusServerRpc(false);
        respawnButton.SetActive(true);

        animator.SetBool("IsRunning", false);
        animator.SetTrigger("Death");

    }

    public void Respawn()
    {
        var tentPosition = TentPlayerRespawner.GetLastTentPosition();
        if (tentPosition == Vector3.zero)
            Debug.Log("Need tent to respawn the player");

        animator.ResetTrigger("Death");
        SetDeathStatusServerRpc(true);
        respawnButton.SetActive(false);

        HealMaxServerRpc();
        transform.position = tentPosition;
    }


    [ServerRpc(RequireOwnership = false)]
    private void HealMaxServerRpc() => _health.Value = maxHealth;


    [ServerRpc(RequireOwnership = false)]
    private void SetDeathStatusServerRpc(bool status)
    {

        playerMovementScript.enabled = status;
        this.enabled = status;
        SetDeathStatusClientRpc(status);
    }

    [ClientRpc]
    private void SetDeathStatusClientRpc(bool status)
    {
        playerMovementScript.enabled = status;
        this.enabled = status;
    }



    public void PlayGetHitAnimation()
    {
        animator.SetBool("IsGetHit", true);
        Invoke(nameof(ResetGetHit), getHitClip.length);
    }


    public void ResetGetHit() => animator.SetBool("IsGetHit", false);

    public int GetMaxHealth() => maxHealth;

    public int GetCurrentHealth() => _health.Value;

    public NetworkVariable<int> GetHealthVariable() => _health;
}
