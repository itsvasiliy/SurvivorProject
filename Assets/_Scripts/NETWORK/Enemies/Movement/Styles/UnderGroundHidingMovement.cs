using System.Collections;
using UnityEngine;

public class UnderGroundHidingMovement : MonoBehaviour
{
    [SerializeField] EnemyShooting peaAttackScript;
    [SerializeField] Transform enemyTransform;

    [SerializeField] Animator animator;
    [SerializeField] AnimationClip buryDownAnimation;

    [SerializeField] float underGroundYValue;
    [SerializeField] float groundDefaultYValue = 1;

    [SerializeField] float emergeUpRechargeTime;
    [SerializeField] float timeToBuryAfterMergeUp;


    private bool isCanEmergeUp = true;


    private void Start() => StartCoroutine(BuryInTheGround());


    private IEnumerator EmergeUp()
    {
        animator.SetTrigger("EmergeUp");
        SetComponentsStatus(true);

        yield return new WaitForSeconds(0.14f); //wait for animation to change the enemy position
        SetYPosition(groundDefaultYValue);

        yield return new WaitForSeconds(timeToBuryAfterMergeUp); //wait for moment to bury back
        StartCoroutine(BuryInTheGround());
    }


    private IEnumerator BuryInTheGround()
    {
        SetComponentsStatus(false);
        animator.SetTrigger("BuryDown");

        yield return new WaitForSeconds(buryDownAnimation.length - 0.1f); //wait for animation to change the enemy position
        SetYPosition(underGroundYValue);
    }


    private void OnTriggerStay(Collider other)
    {
        if (isCanEmergeUp)
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

    private void SetComponentsStatus(bool status) => peaAttackScript.enabled = status;

    private void RechargeEmergeUp() => isCanEmergeUp = true;
}
