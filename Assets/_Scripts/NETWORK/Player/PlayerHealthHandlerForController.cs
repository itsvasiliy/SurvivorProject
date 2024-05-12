using Unity.Netcode;
using UnityEngine;

public class PlayerHealthHandlerForController : MonoBehaviour, IDamageable, IHealthController
{
    [SerializeField] PlayerHealthController healthController;
    public void Dead(ResourceController resourceController = null)
    {
        throw new System.NotImplementedException();
    }

    public int GetCurrentHealth() => healthController.GetCurrentHealth();

    void IHealthController.GetDamage(int damage) => ((IDamageable)this).GetDamage(damage);

    public NetworkVariable<int> GetHealthVariable() =>
        healthController.GetHealthVariable();

    public int GetMaxHealth() => healthController.GetMaxHealth();

    public bool IsAlive() => healthController.IsAlive();

    public void GetDamage(int damage, ResourceController resourceControllerIfKill = null)
            => healthController.GetDamage(damage);
}
