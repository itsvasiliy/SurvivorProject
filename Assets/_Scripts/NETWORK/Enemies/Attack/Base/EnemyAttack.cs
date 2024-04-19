using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] int bodyDamage;
    [SerializeField] AnimationClip attackClip;

    [SerializeField] EnemyMovement movement;


    public Animator animator;

    private float attackSpeed;

    private bool isAttacking = false;


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
        movement.SetCanMoveStatus(false);
        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        StartCoroutine(DamagePlayerWithDelay(_playerHealthController));
        Invoke("ResetAttackStatus", attackSpeed);
    }

    private IEnumerator DamagePlayerWithDelay(PlayerHealthController _playerHealth)
    {
        yield return new WaitForSeconds(attackSpeed - 0.15f);
        _playerHealth.GetDamage(bodyDamage);
    }

    private void StopToAttack()
    {
        animator.SetBool("IsAttacking", false);
        movement.SetCanMoveStatus(true);
    }

    private void ResetAttackStatus() => isAttacking = false;
}