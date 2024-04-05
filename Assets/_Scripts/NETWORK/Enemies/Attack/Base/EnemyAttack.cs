using System;
using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] int bodyDamage;
    [SerializeField] AnimationClip attackClip;

    public Animator animator;

    private float attackSpeed;

    private bool isAttacking = false;


    private void Start() => attackSpeed = attackClip.length;

    private void OnTriggerStay(Collider other)
    {
        if (isAttacking) return;
        if (other.gameObject.TryGetComponent<PlayerHealthController>(out PlayerHealthController _playerHealthController))
            TryToAttack(_playerHealthController);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerHealthController>(out PlayerHealthController player))
            StopToAttack();
    }


    private void TryToAttack(PlayerHealthController _playerHealthController)
    {
        try
        {
            isAttacking = true;
            animator.SetBool("IsAttacking", true);
            StartCoroutine(DamagePlayerWithDelay(_playerHealthController));
            Invoke("ResetAttackStatus", attackSpeed);

        }
        catch (Exception e)
        {
            Debug.LogError("NetworkObjectHealth not found in PlayerController object");
            Debug.LogException(e);
        }
    }

    private IEnumerator DamagePlayerWithDelay(PlayerHealthController _playerHealth)
    {
        yield return new WaitForSeconds(attackSpeed - 0.15f);
        _playerHealth.GetDamage(bodyDamage);
    }

    private void StopToAttack() => animator.SetBool("IsAttacking", false);

    private void ResetAttackStatus() => isAttacking = false;
}