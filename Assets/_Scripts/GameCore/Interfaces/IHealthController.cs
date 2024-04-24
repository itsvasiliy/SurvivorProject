using Unity.Netcode;

public interface IHealthController
{
    int GetMaxHealth();
    int GetCurrentHealth();
    NetworkVariable<int> GetHealthVariable();
}
