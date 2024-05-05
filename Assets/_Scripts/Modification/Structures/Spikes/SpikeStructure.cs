using System.Collections;
using UnityEngine;

public class SpikeStructure : MonoBehaviour
{
    [SerializeField] private Transform spikeTransform;

    [SerializeField] private float damageRadius;
    [SerializeField] private float damagingRate; 


    [SerializeField] private int damage;

    private void Start()
    {
        StartCoroutine(Damaging());
    }

    private IEnumerator Damaging()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(spikeTransform.position + Vector3.up, damageRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<IHealthController>(out IHealthController healthController))
                {
                    if (healthController.IsAlive())
                    {
                        healthController.GetDamage(damage);
                    }
                }
            }

            yield return new WaitForSeconds(damagingRate);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spikeTransform.position + Vector3.up, damageRadius);
    }
}
