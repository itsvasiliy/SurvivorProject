using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) GetComponentInChildren<Camera>().gameObject.SetActive(false);
    }
}
