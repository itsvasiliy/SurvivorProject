using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Enemy : NetworkBehaviour, IAimTarget
{
    public void GetDamage(int damage)
    {
    }

    public void Dead()
    {
    }

}
