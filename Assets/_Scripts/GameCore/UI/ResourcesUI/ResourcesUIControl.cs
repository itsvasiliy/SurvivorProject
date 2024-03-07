using Assets.Scripts.GameCore.Resources;
using UnityEngine;

public class ResourcesUIControl : MonoBehaviour
{
    [SerializeField] ResourceItemInfo[] resourceItems;


    public void UpdateResource(ResourceTypes type)
    {
        foreach (ResourceItemInfo item in resourceItems)
        {
            if (item.type == type)
            {
                item.UpdateAmountFromRepository();
                return;
            }
        }
    }
}
