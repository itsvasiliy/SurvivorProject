using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NETWORK_CONNECTION_UI : MonoBehaviour
{
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

}
