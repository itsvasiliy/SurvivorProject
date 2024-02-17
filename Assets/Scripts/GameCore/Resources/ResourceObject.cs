using UnityEngine;

public class ResourceObject : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject dropPrefab;

    public void GetDamage()
    {
        Vector3 randomPos = new Vector3(transform.position.x + Random.RandomRange(-1.0f, 1.0f),
               0.5f, transform.position.z + Random.RandomRange(-1.0f, 1.0f));
        Instantiate(dropPrefab, randomPos, Quaternion.Euler(0, 0, 0));
    }

    public void Dead() => throw new System.NotImplementedException();
}
