using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private GameObject[] localObjects;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            for (int i = 0; i < localObjects.Length; i++)
            {
                Destroy(localObjects[i]);
            }
        }
        else
        {
            var loadingScreen = Object.FindAnyObjectByType<SceneLoadingRelay>();
            loadingScreen.gameObject.SetActive(false);
        }
    }
}
