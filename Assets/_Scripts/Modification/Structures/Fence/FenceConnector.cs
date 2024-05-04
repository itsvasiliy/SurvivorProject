using UnityEngine;

public class FenceConnector : MonoBehaviour
{
    [SerializeField] private Structure structure;

    [SerializeField] private GameObject connections;

    private bool isConnected = false;


    void Start()
    {
        if (structure.isViewing == false)
            Destroy(this);
        else
            Destroy(connections);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isConnected)
            return;

        if (other.CompareTag("FenceConnection"))
        {
            var pos = other.transform.position;
            structure.transform.position = new Vector3(pos.x, structure.transform.position.y, pos.z);
            structure.transform.rotation = other.transform.rotation;
            structure.canFollow = false;
            isConnected = true;
            Invoke(nameof(ResetConnection), 2.4f);
        }
    }


    private void ResetConnection()
    {
        structure.canFollow = true;
        isConnected = false;
    }
}
