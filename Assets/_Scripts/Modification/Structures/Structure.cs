using Assets.Scripts.GameCore.Resources;
using System;
using System.Collections.Generic;
using Unity.Netcode;

public class Structure : NetworkBehaviour
{
    public string structureName;

    public bool isViewing = false;

    [Serializable]
    public class ResourceCost
    {
        public ResourceTypes resourceType;
        public int cost;
    }

    public List<ResourceCost> constructionCost = new List<ResourceCost>();
}
