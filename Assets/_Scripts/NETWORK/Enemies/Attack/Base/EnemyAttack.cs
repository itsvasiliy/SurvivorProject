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
        if (isAttacking) return;
        if (other.gameObject.TryGetComponent<PlayerHealthController>(out PlayerHealthController _playerHealthController))
            if (_playerHealthController.enabled)
                Attack(_playerHealthController);
            else StopToAttack();
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerHealthController>(out PlayerHealthController player))
            StopToAttack();
    }


    private void Attack(PlayerHealthController _playerHealthController)
    {
        IsAttackAborted = false;
        movement.SetCanMoveStatus(false);
        isAttacking = true;
        animator.SetBool("IsAttacking", true);


        StartCoroutine(DamagePlayerWithDelay(_playerHealthController));
        Invoke(nameof(ResetAttackStatus), attackSpeed);
    }

    private IEnumerator DamagePlayerWithDelay(PlayerHealthController _playerHealth)
    {
        yield return new WaitForSeconds(damageAnimationDelay);

        if (IsAttackAborted == false)
            _playerHealth.GetDamage(meleeDamage);
    }

    private void StopToAttack()
    {
        IsAttackAborted = true;
        animator.SetBool("IsAttacking", false);
        movement.SetCanMoveStatus(true);
    }

    private void ResetAttackStatus() => isAttacking = false;
}