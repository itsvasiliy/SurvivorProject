using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LookAt : MonoBehaviour
{
    private Transform uiTransform;
    private Transform playerCamera;

    private void Start()
    {
        uiTransform = GetComponent<Transform>();
        playerCamera = Camera.main.transform;
    }

    private void LateUpdate()
    {
        uiTransform.LookAt(playerCamera);
    }
}
