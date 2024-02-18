using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTarget : MonoBehaviour, IAimTarget
{
    public void GetDamage()
    {
        print(gameObject.name + "Get hit");
    }

    public void Dead()
    {
    }

}
