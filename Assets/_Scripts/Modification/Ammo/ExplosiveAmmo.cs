using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveAmmo : MonoBehaviour
{
    [SerializeField] private int damage;

    [SerializeField] private float lifeTime;

    private void Start()
    {
        Destroy(gameObject,lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<IDamageable>(out IDamageable _damageable))
        {
            Explosion();
        }
    }

    private void Explosion()
    {

    }
}
