using UnityEngine;

public class FenceConnector : MonoBehaviour
{
    [SerializeField] Structure structure;
    [SerializeField] GameObject connections;
    void Start()
    {
        if (structure.isViewing == false)
            Destroy(this);
        else
            Destroy(connections);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FenceConnection"))
        {
            var pos = other.transform.position;
            structure.transform.position = new Vector3(pos.x, structure.transform.position.y, pos.z);
            structure.transform.rotation = other.transform.rotation;
            structure.canFollow = false;
            Invoke(nameof(ResetFollow), 1.8f);
        }
    }


    private void ResetFollow() => structure.canFollow = true;
}
