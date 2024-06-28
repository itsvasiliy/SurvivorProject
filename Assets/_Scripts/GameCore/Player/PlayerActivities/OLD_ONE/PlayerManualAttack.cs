using Firebase.Analytics;
using Unity.Netcode;
using UnityEngine;

public class PlayerManualAttack : NetworkBehaviour
{
    [Header("Animations")]
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip attackingAnimClip;
    [SerializeField] float dropResourceDelay;

    [Header("Tool properties")]
    [SerializeField] GameObject tool;
    [SerializeField] IDamageableDetectionCube damageableDetectionCube;

    [Header("Player properties")]
    [SerializeField] PlayerStateController playerStateController;
    [SerializeField] ResourceController playerResourceController;
    // [SerializeField] private PlayerLevelSystem playerLevelSystem;

    private float attackSpeed;
    private bool isAttacking = false;
    private bool isAttackAborted = false;

    IMineable mineableResource;

    private void Start()
    {
        attackSpeed = attackingAnimClip.length;

        damageableDetectionCube.detected += Attack;
        damageableDetectionCube.leftDetectedAction += StopAttack;
    }

    private void Attack()
    {
        isAttackAborted = false;
        if (isAttacking == false)
        {
            isAttacking = true;

            RaycastHit[] hits = damageableDetectionCube.GetHits;

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent<IMineable>(out IMineable _mineable))
                    {
                        mineableResource = _mineable;
                        MineResource();
                    }
                }
            }

            Invoke(nameof(ResetAttackingStatus), attackSpeed);
        }
    }

    private void ResetAttackingStatus() => isAttacking = false;

    private void StopAttack()
    {
        isAttackAborted = true;
        Invoke(nameof(DeactivateTool), 3.5f);
        animator.SetBool("IsMining", false);
    }

    private void MineResource()
    {
        animator.SetBool("IsMining", true);

        playerStateController.SetState(PlayerStates.Mining);
        // playerLevelSystem.AddExperience = 10;

        //Invoke(nameof(MineResourceAfterAnimation), dropResourceDelay); // this call in animation

        FirebaseAnalytics.LogEvent("mining");
    }

    private void MineResourceAfterAnimation() // this call in animation
    {
        //if (isAttackAborted)
        //    return;
        mineableResource.MineResource(playerResourceController);
    }



    private void ActivateTool() // this call in animation
    {
        if (!tool.activeSelf)
            tool.SetActive(true);
    }
    private void DeactivateToolWitDelay() => Invoke(nameof(DeactivateTool), 6f); // this call in animation
    private void DeactivateTool()
    {
        if (!isAttacking)
            tool.SetActive(false);
    }


    private void OnDestroy()
    {
        damageableDetectionCube.detected -= Attack;
        damageableDetectionCube.leftDetectedAction -= StopAttack;
    }
}