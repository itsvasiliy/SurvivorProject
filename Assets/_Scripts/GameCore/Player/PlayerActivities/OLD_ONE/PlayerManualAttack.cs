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
        Invoke(nameof(DeactivateTool), 2.5f);
        animator.SetBool("IsMining", false);
    }

    private void MineResource()
    {
        if (tool.activeSelf == false)
            ActivateTool();
        animator.SetBool("IsMining", true);

        playerStateController.SetState(PlayerStates.Mining);
       // playerLevelSystem.AddExperience = 10;

        Invoke(nameof(MineResourceAfterAnimation), dropResourceDelay);
    }

    private void MineResourceAfterAnimation()
    {
        if (isAttackAborted)
            return;

        mineableResource.MineResource(playerResourceController);
    }

    private void ActivateTool()
    {
        if (tool.activeSelf)
            return;
        SetToolActiveStatus(true);

        if (tool.activeSelf == false)
            tool.SetActive(true);
    }

    private void DeactivateTool() => SetToolActiveStatus(false);

    private void SetToolActiveStatus(bool status)
    {
        SetToolStatusServerRpc(status);
        SetToolStatusClientRpc(status);
    }


    [ServerRpc(RequireOwnership = false)]
    private void SetToolStatusServerRpc(bool status) => tool.SetActive(status);

    [ClientRpc]
    private void SetToolStatusClientRpc(bool status) => tool.SetActive(status);


    private void OnDestroy()
    {
        damageableDetectionCube.detected -= Attack;
        damageableDetectionCube.leftDetectedAction -= StopAttack;
    }
}