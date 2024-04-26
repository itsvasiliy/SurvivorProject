using UnityEngine;
using UnityEngine.UI;

public class SliderLookAtCamera : MonoBehaviour
{
    private Transform sliderTransform;

    private Transform playerCamera;

    private void Start()
    {
        sliderTransform = GetComponent<Slider>().transform;
        playerCamera = Camera.main.transform;
    }

    private void LateUpdate()
    {
        sliderTransform.LookAt(playerCamera);
    }
}
