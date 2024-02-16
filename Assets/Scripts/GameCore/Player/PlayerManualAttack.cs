using UnityEngine;
using Zenject;

[RequireComponent(typeof(IDamageableDetectionCube))]
public class PlayerManualAttack : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject tool;

    private IDamageableDetectionCube damageableDetectionCube;

    private float attackSpeed;

    [Inject] IPlayerStateController playerStateController;


    private void Start()
    {
        if (gameObject.TryGetComponent<IDamageableDetectionCube>(out IDamageableDetectionCube _damageableDetectionCube))
        {
            damageableDetectionCube = _damageableDetectionCube;
        }

        damageableDetectionCube.detected += Attack;
        damageableDetectionCube.leftDetectedAction += StopAttack;
    }

    private void Attack()
    {
        // Add attacking rate and animation 
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
    }

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