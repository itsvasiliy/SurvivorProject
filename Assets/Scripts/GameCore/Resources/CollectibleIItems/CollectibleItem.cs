using Assets.Scripts.GameCore.Interfaces;
using Assets.Scripts.GameCore.Resources;
using TMPro;
using UnityEngine;
using Zenject;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private ResourceTypes type;
    [SerializeField] GameObject collectionDisplayPrefab;
    [SerializeField] int amount;
    [SerializeField] Color gizmoColor = Color.red;
    [Range(5, 20)] public float detectionRange = 20;


    [Inject] readonly IResourceController resourceController;

    Vector3 castShape = new Vector3(0.5f, 0.1f, 0.5f);
    private RaycastHit[] hits;
    private float moveSpeed = 3f;


    private void Update()
    {

        hits = Physics.BoxCastAll(transform.position, castShape * detectionRange / 2, transform.forward, transform.rotation, detectionRange);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<PlayerController>(out PlayerController player))
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                }
            }
        }
    }



    public void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
            CollectItem();
    }

    public void CollectItem()
    {
        var collectItemRising = Instantiate(collectionDisplayPrefab, transform.position, 
            transform.rotation).GetComponent<CollectItemRising>();
        collectItemRising.SetAmount(amount);

        resourceController.AddResource(type, amount);
        Destroy(gameObject);
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        var boxsize = castShape.x * detectionRange;
        Gizmos.DrawCube(transform.position, new Vector3(boxsize, 0.05f, boxsize));
    }

}
