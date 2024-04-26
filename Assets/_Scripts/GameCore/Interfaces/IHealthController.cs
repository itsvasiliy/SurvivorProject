using Unity.Netcode;

public interface IHealthController
{
    int GetMaxHealth();
    int GetCurrentHealth();
    bool IsAlive();
    NetworkVariable<int> GetHealthVariable();
}
