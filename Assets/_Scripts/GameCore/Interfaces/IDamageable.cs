interface IDamageable
{
    void GetDamage(int damage, ResourceController resourceControllerIfKill = null);

    public void Dead(ResourceController resourceController = null);
    bool IsAlive();
}