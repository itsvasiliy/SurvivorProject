using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SliderLookAtCamera : MonoBehaviour
{
    private Transform sliderTransform;

    private Transform playerCamera;

    private void Start()
    {
        sliderTransform = GetComponent<Slider>().transform;
        StartCoroutine(BindCamera());
    }

    private IEnumerator BindCamera() //used in case when server is started but players is not spawned yet
    {
        if (Camera.main != null)
            playerCamera = Camera.main.transform;
        else
            yield return new WaitForSeconds(1);
    }

    private void LateUpdate()
    {
        sliderTransform.LookAt(playerCamera);
    }
}
