using UnityEngine;

public class DestroyNetworkManager : MonoBehaviour
{
    void Awake()
    {
        if (RelayServerDataManagerSingleton.relayCode != null &&
            RelayServerDataManagerSingleton.isHost)
            Destroy(gameObject);
    }
}
