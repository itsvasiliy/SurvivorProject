using Assets.Scripts.GameCore.Resources;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private ResourceTypes type;
    [SerializeField] GameObject collectionDisplayPrefab;
    [SerializeField] int amount;


    public void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Instantiate(collectionDisplayPrefab, transform.position, transform.rotation);
            //add item to inventory
            Destroy(gameObject);
        }
    }

}
