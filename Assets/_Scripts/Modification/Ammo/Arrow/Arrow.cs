using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private int damage;

    [SerializeField] private AudioSource damageSound;
    [SerializeField] private AudioSource nonDamageSound;

    private ResourceController playerResourceController;

    public void SetResourceController(ResourceController _resourceController) => playerResourceController = _resourceController;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            DamageTheTarget(damageable);
        else
            DestroyArrow();
    }

    private void DestroyArrow()
    {
        nonDamageSound.Play();
        Destroy(gameObject, nonDamageSound.clip.length);
    }

    private void DamageTheTarget(IDamageable damageable)
    {
        damageable.GetDamage(damage, playerResourceController);

        damageSound.Play();
        Destroy(gameObject, damageSound.clip.length);
    }
}
