using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Transform _transform;

    [SerializeField] private float speed;

    public Vector3 targetPosition;

    private void Start()
    {
        StartCoroutine(MoveToTarget());
    }

    //public Vector3 SetTargetPOsition
    //{
    //    set
    //    {
    //        targetPosition = value;
    //        StartCoroutine(MoveToTarget());
    //    }
    //}

    private IEnumerator MoveToTarget()
    {
        while (Vector3.Distance(_transform.position, targetPosition) > 0.1f)
        {
            Vector3 direction = (targetPosition - _transform.position).normalized;
            float distanceToMove = speed * Time.fixedDeltaTime;

            // Move towards the target
            _transform.position += direction * distanceToMove;

            yield return null;
        }
    }

    }
