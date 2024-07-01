using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] int meleeDamage;

    [SerializeField] AnimationClip attackClip;

    [SerializeField] float damageAnimationDelay;

    [SerializeField] EnemyMovement movement;

    [SerializeField] MMF_Player attackFeedback;

    public Animator animator;

    private float attackSpeed;

    private bool isAttacking = false;

    private bool IsAttackAborted = false;

    private void Start() => attackSpeed = attackClip.length;

    private void OnTriggerStay(Collider other)
    {
        if (isAttacking || !enabled) return;
        if (other.gameObject.TryGetComponent<IHealthController>(out IHealthController targetHealth))
            if (targetHealth.IsAlive() && other.gameObject.GetComponent<EnemyHealthController>() == null) // 2nd condition. make sure this is not another enemy
                Attack(targetHealth);
           // else StopToAttack();
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
        {
            attackFeedback?.PlayFeedbacks();
            _playerHealth.GetDamage(meleeDamage);
        }
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