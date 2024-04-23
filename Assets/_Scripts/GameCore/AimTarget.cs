using UnityEngine;

public class AimTarget : MonoBehaviour, IAimTarget
{
    NetworkObjectHealth health;

    private void Start() => health = GetComponent<NetworkObjectHealth>();

    public void GetDamage(int damage)
    {
        health.GetDamage(damage);
    }

    public void Dead()
    {
        throw new System.NotImplementedException();
    }

    public bool IsEnabled() => this.enabled;
  
}
