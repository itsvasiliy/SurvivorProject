using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] int meleeDamage;
    [SerializeField] AnimationClip attackClip;
    [SerializeField] float damageAnimationDelay;

    [SerializeField] EnemyMovement movement;

    public Animator animator;

    private float attackSpeed;

    private bool isAttacking = false;

    private bool IsAttackAborted = false;

    private void Start() => attackSpeed = attackClip.length;

    private void OnTriggerStay(Collider other)
    {
        if (isAttacking || !enabled) return;
        if (other.gameObject.TryGetComponent<IHealthController>(out IHealthController targetHealth))
            if (targetHealth.IsAlive())
                Attack(targetHealth);
            else StopToAttack();
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<IHealthController>(out IHealthController player))
            StopToAttack();
    }


    private void Attack(IHealthController targetHealth)
    {
        IsAttackAborted = false;
        isAttacking = true;
        animator.SetTrigger("Attack");

        StartCoroutine(DamagePlayerWithDelay(targetHealth));
        Invoke(nameof(StopToAttack), attackSpeed);
    }

    private IEnumerator DamagePlayerWithDelay(IHealthController _playerHealth)
    {
        yield return new WaitForSeconds(damageAnimationDelay);

        if (IsAttackAborted == false)
            _playerHealth.GetDamage(meleeDamage);
    }

    private void StopToAttack()
    {
        IsAttackAborted = true;
        ResetAttackStatus();
        movement.SetCanMoveStatus(true);
    }

    private void ResetAttackStatus()
    {
        animator.ResetTrigger("Attack");
        isAttacking = false;
    }
}