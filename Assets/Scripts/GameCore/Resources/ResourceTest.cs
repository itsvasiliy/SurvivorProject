using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTest : MonoBehaviour, IDamageable
{
    public void GetDamage()
    {
        print("damaged");
    }

    public void Dead()
    {
        throw new System.NotImplementedException();
    }
}