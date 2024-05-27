using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InGameNickname : NetworkBehaviour
{
    [SerializeField] private GameObject nickname;


    private void Start()
    {
        if(IsOwner)
        {
            Destroy(nickname);
        }
    }
}
