using UnityEngine;

public class MineableTree : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject treeDropPrefab;
    public void Dead()
    {
        throw new System.NotImplementedException();
    }

    public void GetDamage()
    {
        Vector3 randomPos = new Vector3(transform.position.x + Random.RandomRange(-3.0f, 3.0f),
               3.0f, transform.position.z + Random.RandomRange(-3.0f, 3.0f));
        Instantiate(treeDropPrefab, randomPos, Quaternion.Euler(0, 0, 0));

    }


}
