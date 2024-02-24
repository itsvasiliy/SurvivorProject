using UnityEngine;
using Zenject;

[RequireComponent(typeof(IDamageableDetectionCube))]
public class PlayerManualAttack : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] AnimationClip attackingAnimClip;

    [SerializeField] GameObject tool;

    private IDamageableDetectionCube damageableDetectionCube;

    private float attackSpeed;

    private bool isAttacking = false;

    [Inject] IPlayerStateController playerStateController;


    private void Start()
    {
        if (gameObject.TryGetComponent<IDamageableDetectionCube>(out IDamageableDetectionCube _damageableDetectionCube))
        {
            damageableDetectionCube = _damageableDetectionCube;
        }

        attackSpeed = attackingAnimClip.length;

        damageableDetectionCube.detected += Attack;
        damageableDetectionCube.leftDetectedAction += StopAttack;
    }

    private void Attack()
    {
        if (isAttacking == false)
        {
            isAttacking = true;

            RaycastHit[] hits = damageableDetectionCube.GetHits;

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent<IDamageable>(out IDamageable _damageable))
                    {
                        _damageable.GetDamage();
                        playerStateController.SetState(PlayerStates.Mining);
                        tool.SetActive(true);
                        animator.SetBool("IsMining", true);
                    }
                }
            }

            Invoke(nameof(ResetAttackingStatus), attackSpeed);
        }
    }

    private void ResetAttackingStatus() => isAttacking = false;

    private void StopAttack()
    {
        Invoke("DeactivateTool", 2.5f);
        animator.SetBool("IsMining", false);
    }

    private void DeactivateTool() => tool.SetActive(false);

    private void OnDestroy()
    {
        damageableDetectionCube.detected -= Attack;
        damageableDetectionCube.leftDetectedAction -= StopAttack;
    }
}