using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            DamageTheTarget(damageable);
        }
    }

    private void DamageTheTarget(IDamageable damageable)
    {
        damageable.GetDamage(damage);
    }
}
