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
    [SerializeField] float rangeToEmergeNearPlayer = 10;


    [Header("Timings")]
    [SerializeField] float emergeUpRechargeTime;
    [SerializeField] float timeToBuryAfterMergeUp;

    [Header("Others")]
    [SerializeField] float detectionRadius;

    private bool isCanEmergeUp = true;


    private void OnEnable()
    {
        //bury under ground in an instant
        SetComponentsStatus(false);
        SetYPosition(underGroundYValue);

        StartCoroutine(DetectPlayers());
    }


    private IEnumerator DetectPlayers()
    {
        while (true)
        {
            if (isCanEmergeUp)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

                foreach (Collider collider in colliders)
                {
                    PlayerHealthHandlerForController aimTarget = collider.GetComponent<PlayerHealthHandlerForController>();

                    if (aimTarget != null && aimTarget.IsAlive())
                    {
                        StartCoroutine(EmergeUp(aimTarget.transform.position));
                        Invoke(nameof(RechargeEmergeUp), emergeUpRechargeTime);
                    }
                }
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator EmergeUp(Vector3 playerPosition)
    {
        isCanEmergeUp = false;
        animator.SetTrigger("EmergeUp");

       // SetRandomPositionNearPlayer(playerPosition);

        Invoke(nameof(ResetEmergeTrigger), emergeUpAnimation.length);
        Invoke(nameof(EnableComponents), emergeUpAnimation.length); // delay to prevent shooting before emerge up

        yield return new WaitForSeconds(0.14f); //wait for animation to change the plant shooter position
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

        yield return new WaitForSeconds(buryDownAnimation.length - 0.1f); //wait for animation to change the plant shooter position
        if (healthController.IsAlive())
            SetYPosition(underGroundYValue);
    }


    private void SetRandomPositionNearPlayer(Vector3 playerPos)
    {
        float offsetX = Random.Range(-rangeToEmergeNearPlayer, rangeToEmergeNearPlayer);
        float offsetZ = Random.Range(-rangeToEmergeNearPlayer, rangeToEmergeNearPlayer);

        Vector3 randomPositionNearPlayer = new Vector3(playerPos.x + offsetX, transform.position.y, playerPos.z + offsetZ);
        transform.position = randomPositionNearPlayer;
    }


    private void SetYPosition(float value)
    {
        Vector3 groundPosition = new Vector3(transform.position.x, value, transform.position.z);
        transform.position = groundPosition;
    }


    private void SetComponentsStatus(bool status)
    {
        peaAttackScript.enabled = status;
        healthController.SetVisibleStatus(status);
    }


    private void EnableComponents() => SetComponentsStatus(true);
    private void RechargeEmergeUp() => isCanEmergeUp = true;
    private void ResetEmergeTrigger() => animator.ResetTrigger("EmergeUp");
    private void OnDisable() => StopAllCoroutines();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
