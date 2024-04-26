using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SpiderAttack : MonoBehaviour
{
    [Header("Spider parameters")]

    [SerializeField] private Animator _animator;

    [SerializeField] private Transform bitingOrigin;

    [SerializeField] private Vector3 bitigZoneSize;

    [SerializeField] private MonoBehaviour movementScript;

    private NavMeshAgent navMeshAgent;

    private float checkForPlayerRate = 0.3f;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        StartCoroutine(PlayerDetecting());   
    }

    private IEnumerator PlayerDetecting()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapBox(bitingOrigin.position, bitigZoneSize, Quaternion.identity);
            //bool playerWasDetected = false;

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<PlayerHealthController>(out PlayerHealthController playerHealth))
                {
                    _animator.SetTrigger("IsAttacking");
                    //playerWasDetected = true;
                }
            }

            //if(playerWasDetected == false)
            //{

            //}

            yield return new WaitForSeconds(checkForPlayerRate);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(bitingOrigin.position, bitigZoneSize);
    }
}
