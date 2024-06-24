using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private int damage;

    private ResourceController playerResourceController;

    public void SetResourceController(ResourceController _resourceController) => playerResourceController = _resourceController;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            DamageTheTarget(damageable);
            Destroy(gameObject);
        }
    }

    private void DamageTheTarget(IDamageable damageable)
    {
        damageable.GetDamage(damage, playerResourceController);
    }
}
