using UnityEngine;

public class CampfireHealing : MonoBehaviour
{
    [SerializeField] private int healRadius;
    [SerializeField] private int healAmountPerTick;
    [SerializeField] private float tickDuration;

    RaycastHit[] hits;


    private void Start() => InvokeRepeating(nameof(TrackPlayerToHeal), 0f, tickDuration);


    private void TrackPlayerToHeal()
    {
        hits = Physics.SphereCastAll(transform.position, healRadius, Vector3.forward);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.TryGetComponent<PlayerHealthController>(out PlayerHealthController health))
                HealPlayer(health);
        }
    }

    void HealPlayer(PlayerHealthController health)
    {
        if (health.IsHealthMax() == false)
            health.Heal(healAmountPerTick);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healRadius);
    }
}
