using Unity.Netcode;
using UnityEngine;

public class NETWORK_CONNECTION_UI : MonoBehaviour
{
    [SerializeField] private GameObject accessNetworkElement;

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        Destroy(accessNetworkElement);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        Destroy(accessNetworkElement);
    }
}