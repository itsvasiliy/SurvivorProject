using Assets.Scripts.GameCore.Resources;
using System;
using System.Collections.Generic;
using Unity.Netcode;

public class Structure : NetworkBehaviour
{
    public string structureName;

    public bool isViewing = false;
    public bool canFollow = true;

    [Serializable]
    public class ResourceCost
    {
        public ResourceTypes resourceType;
        public int cost;
    }

    public List<ResourceCost> constructionCost = new List<ResourceCost>();
}
