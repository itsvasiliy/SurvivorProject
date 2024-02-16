using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IDamageableDetectionCube))]
public class PlayerManualAttack : MonoBehaviour
{
    private IDamageableDetectionCube damageableDetectionCube;

    private float attackSpeed;

    private void Start()
    {
        if (gameObject.TryGetComponent<IDamageableDetectionCube>(out IDamageableDetectionCube _damageableDetectionCube))
        {
            damageableDetectionCube = _damageableDetectionCube;
        }

        damageableDetectionCube.detected += Attack;
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
                }
            }
        }

    //-----------------------------
    }

    private void OnDestroy()
    {
        damageableDetectionCube.detected -= Attack;
    }
}