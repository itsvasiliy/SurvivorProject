using System.Collections;
using UnityEngine;

public class UnderGroundHidingMovement : MonoBehaviour
{
    [Header("Components to disable when hiding")]
    [SerializeField] EnemyShooting peaAttackScript;
    [SerializeField] EnemyHealthController healthController;

    [Header("Animations")]
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip buryDownAnimation;
    [SerializeField] AnimationClip emergeUpAnimation;

    [Header("Hiding|Unhiding positions")]
    [SerializeField] float underGroundYValue;
    [SerializeField] float groundDefaultYValue = 1;

    [Header("Timings")]
    [SerializeField] float emergeUpRechargeTime;
    [SerializeField] float timeToBuryAfterMergeUp;

    [Header("Others")]
    [SerializeField] Transform enemyTransform;


    private bool isCanEmergeUp = true;
    private bool isEmerded = false;


    private void OnEnable() => StartCoroutine(BuryInTheGround());
    private void OnDisable() => StopCoroutine(BuryInTheGround());


    private IEnumerator EmergeUp()
    {
        animator.SetTrigger("EmergeUp");
        Invoke(nameof(ResetEmergeTrigger), emergeUpAnimation.length);
        Invoke(nameof(EnableComponents), emergeUpAnimation.length); // delay to prevent shooting before emerge up

        yield return new WaitForSeconds(0.14f); //wait for animation to change the enemy position
        SetYPosition(groundDefaultYValue);

        yield return new WaitForSeconds(timeToBuryAfterMergeUp); //wait for moment to bury back
        StartCoroutine(BuryInTheGround());
    }


    private IEnumerator BuryInTheGround()
    {
        if (!enabled) //exit  the coroutine if component disabled
            yield break;
        
        SetComponentsStatus(false);
        animator.SetTrigger("BuryDown");

        yield return new WaitForSeconds(buryDownAnimation.length - 0.1f); //wait for animation to change the enemy position
        SetYPosition(underGroundYValue);
    }


    private void OnTriggerStay(Collider other)
    {
        if (isCanEmergeUp && this.enabled)
        {
            PlayerHealthController player = other.GetComponent<PlayerHealthController>();
            if (player != null && player.enabled) //player.enabled means player is alive
            {
                StartCoroutine(EmergeUp());
                isCanEmergeUp = false;
                Invoke(nameof(RechargeEmergeUp), emergeUpRechargeTime);
            }
        }
    }

    private void SetYPosition(float value)
    {
        Vector3 groundPosition = new Vector3(enemyTransform.position.x, value, enemyTransform.position.z);
        enemyTransform.position = groundPosition;
    }

    private void EnableComponents() => SetComponentsStatus(true);
    private void SetComponentsStatus(bool status)
    {
        peaAttackScript.enabled = status;
        healthController.enabled = status;
    }

    private void RechargeEmergeUp() => isCanEmergeUp = true;
    private void ResetEmergeTrigger() => animator.ResetTrigger("EmergeUp");

}
