using System;
using Unity.Netcode;
using UnityEngine;

public interface IHealthController
{
    void GetDamage(int damage) { throw new NotImplementedException(); }
    int GetMaxHealth();
    int GetCurrentHealth();
    bool IsAlive();
    NetworkVariable<int> GetHealthVariable();
}
