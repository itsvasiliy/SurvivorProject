using Assets.Scripts.GameCore.Interfaces;
using Assets.Scripts.GameCore.Resources;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private ResourceTypes type;
    [SerializeField] GameObject collectionDisplayPrefab;
    [SerializeField] int amount;
    [SerializeField] Color gizmoColor = Color.red;
    [SerializeField] private float moveSpeed = 5f;

    [Range(5, 20)] public float detectionRange = 20;

    private IResourceController resourceController;

    Vector3 castShape = new Vector3(0.5f, 0.1f, 0.5f);
    private RaycastHit[] hits;


    private void Update()
    {

        hits = Physics.BoxCastAll(transform.position, castShape * detectionRange / 2, transform.forward, transform.rotation, detectionRange);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<ResourceController>(out ResourceController resourcePlayerControl))
                {
                    if (resourceController == null)
                        resourceController = resourcePlayerControl;
                    transform.position = Vector3.MoveTowards(transform.position, resourcePlayerControl.transform.position, moveSpeed * Time.deltaTime);
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
