using System.Linq;
using UnityEngine;

public class StructPlacementAvailability : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    private Color red035alpha = new Color(1.0f, 0.0f, 0.0f, 0.35f);
    private Color green035alpha = new Color(0.0f, 1.0f, 0.0f, 0.35f);
    private Material material;

    public bool canBuild;

    private void Start()
    {
        material = new Material(Shader.Find("Standard"));

        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in meshRenderers)
            renderer.material = material;

        material.SetFloat("_Mode", 3); //Transparent render mode
        SetColorAndBuildStatus(green035alpha, true);
    }

    private void SetColorAndBuildStatus(Color color, bool buildStatus)
    {
        canBuild = buildStatus;
        material.color = color;
    }

    private void OnTriggerStay(Collider other)
    {
        if (canBuild)
            SetColorAndBuildStatus(red035alpha, false);
    }

    private void OnTriggerExit(Collider other) => SetColorAndBuildStatus(green035alpha, true);
}
