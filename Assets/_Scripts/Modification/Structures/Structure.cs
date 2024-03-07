using Assets.Scripts.GameCore.Resources;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public int maxHealth;
    public int health;

    [Serializable]
    public class ResourceCost
    {
        public ResourceTypes resourceType;
        public int cost;
    }

    public List<ResourceCost> constructionCost = new List<ResourceCost>();
}
