using UnityEngine;

public class AimTarget : MonoBehaviour, IAimTarget
{
    NetworkObjectHealth health;

    private void Start() => health = GetComponent<NetworkObjectHealth>();

    public void GetDamage(int damage, ResourceController resourceController = null)
    {
        health.GetDamage(damage);
    }

    public void Dead(ResourceController resourceController = null)
    {
        throw new System.NotImplementedException();
    }

    public bool IsAlive()
    {
        throw new System.NotImplementedException();
    }
}
