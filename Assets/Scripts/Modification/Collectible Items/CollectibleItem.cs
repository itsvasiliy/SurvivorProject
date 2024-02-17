using Assets.Scripts.GameCore.Interfaces;
using Assets.Scripts.GameCore.Resources;
using UnityEngine;
using Zenject;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private ResourceTypes type;
    [SerializeField] GameObject collectionDisplayPrefab;
    [SerializeField] int amount;

   [Inject] readonly IResourceController resourceController;

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Instantiate(collectionDisplayPrefab, transform.position, transform.rotation);
            resourceController.AddResource(type, amount);
            Destroy(gameObject);
        }
    }

}
