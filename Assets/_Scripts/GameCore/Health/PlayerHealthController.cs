using UnityEngine;

public class PlayerHealthController : MonoBehaviour, IDamageable
{
    [SerializeField] PlayerMovement playerMovementScript;
    [SerializeField] AnimationClip getHitClip;

    Animator animator;
    NetworkObjectHealth health;


    void Start()
    {
        health = GetComponent<NetworkObjectHealth>();
        animator = GetComponent<Animator>();
    }


    public void GetDamage(int damage)
    {
        animator.SetBool("IsGetHit", true);
        Invoke("ResetGetHit", getHitClip.length);

        health.GetDamage(damage);
        if (health._health.Value <= 0)
            Dead();
    }


    public void Dead()
    {
        playerMovementScript.enabled = false;
        animator.SetTrigger("Death");
        this.enabled = false;
    }

    public void Reincarnate()
    {
        playerMovementScript.enabled = true;
        animator.ResetTrigger("Death");
        this.enabled = true;

    }


    public void ResetGetHit() => animator.SetBool("IsGetHit", false);

}
