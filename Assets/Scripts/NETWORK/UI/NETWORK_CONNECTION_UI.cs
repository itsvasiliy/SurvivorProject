using Unity.Netcode;
using UnityEngine;

public class NETWORK_CONNECTION_UI : MonoBehaviour
{
    [SerializeField] private GameObject accessNetworkElement;

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        accessNetworkElement.SetActive(false);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        accessNetworkElement.SetActive(false);
    }
}