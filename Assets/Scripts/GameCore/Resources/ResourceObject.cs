using UnityEngine;
using Zenject;

public class ResourceObject : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject dropPrefab;
    [Inject] DiContainer container;

    public void GetDamage()
    {
        Vector3 randomPos = new Vector3(transform.position.x + Random.RandomRange(-1.0f, 1.0f),
               0.5f, transform.position.z + Random.RandomRange(-1.0f, 1.0f));

        container.InstantiatePrefab(dropPrefab, randomPos, Quaternion.identity, null);
    }

    public void Dead() => throw new System.NotImplementedException();
}
