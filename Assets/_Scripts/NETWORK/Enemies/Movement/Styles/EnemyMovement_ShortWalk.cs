using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement_ShortWalk : EnemyMovement
{
    [Header("EnemyMovement_ShortWalk")]

    [SerializeField] private Animator animator;

    [SerializeField] private float maxWalkDistance;
    [SerializeField] private float goForWalkRate;

    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        Invoke(nameof(Walk), goForWalkRate);
    }

    private Vector3 GetRandomPositionAround()
    {
        // Generate random angles in spherical coordinates
        float angle = Random.Range(0f, Mathf.PI * 2); // Random angle around the Y-axis
        float height = Random.Range(-1f, 1f); // Random height (Y-coordinate) in the range [-1, 1]

        // Convert spherical coordinates to Cartesian coordinates (x, y, z)
        float x = Mathf.Sqrt(1 - height * height) * Mathf.Cos(angle); // Calculate x-coordinate
        float z = Mathf.Sqrt(1 - height * height) * Mathf.Sin(angle); // Calculate z-coordinate

        // Scale the coordinates by the radius and offset by the transform's position
        Vector3 randomPosition = transform.position + new Vector3(x, 0f, z) * maxWalkDistance;

        return randomPosition;
    }

    private void Walk()
    {
        animator.SetBool("IsWalking", true);
        _navMeshAgent.SetDestination(GetRandomPositionAround());

        //StartCoroutine(CheckForStop());

        Invoke(nameof(Walk), goForWalkRate);
    }


    // Not fucking working

    //private IEnumerator CheckForStop()
    //{
    //    while (_navMeshAgent.isStopped == true)
    //    {
    //        yield return new WaitForFixedUpdate();
    //    }

    //    animator.SetBool("IsWalking", false);
    //    StopCoroutine(CheckForStop());
    //}
}
