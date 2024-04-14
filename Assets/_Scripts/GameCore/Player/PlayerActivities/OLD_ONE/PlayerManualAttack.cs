using Unity.Netcode;
using UnityEngine;

public class PlayerManualAttack : NetworkBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] AnimationClip attackingAnimClip;

    [SerializeField] GameObject tool;

    [SerializeField] IDamageableDetectionCube damageableDetectionCube;

    [SerializeField] PlayerStateController playerStateController;

    [SerializeField] private PlayerLevelSystem playerLevelSystem;

    private float attackSpeed;

    private bool isAttacking = false;

    private void Start()
    {
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
                    if (hit.collider.TryGetComponent<IMineable>(out IMineable _damageable))
                    {
                        _damageable.GetDamage(0);
                        playerStateController.SetState(PlayerStates.Mining);

                        playerLevelSystem.AddExperience = 10;

                        if (tool.activeSelf == false)
                            ActivateTool();

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
        Invoke(nameof(DeactivateTool), 2.5f);
        animator.SetBool("IsMining", false);
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
    private void SetToolStatusServerRpc(bool status)
    {
        Debug.Log("Server changed status");
        tool.SetActive(status);
    }

    [ClientRpc]
    private void SetToolStatusClientRpc(bool status)
    {
        Debug.Log("Client changed status");
        tool.SetActive(status);
    }


    private void OnDestroy()
    {
        damageableDetectionCube.detected -= Attack;
        damageableDetectionCube.leftDetectedAction -= StopAttack;
    }
}