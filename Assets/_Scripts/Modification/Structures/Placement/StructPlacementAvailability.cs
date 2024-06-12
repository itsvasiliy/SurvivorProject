using UnityEngine;

public class StructPlacementAvailability : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    private Color red035alpha = new Color(1.0f, 0.0f, 0.0f, 0.35f);
    private Color green035alpha = new Color(0.0f, 1.0f, 0.0f, 0.35f);

    public bool canBuild;

    private void Start()
    {
        SetCollidersAsTrigger();

        meshRenderers = GetComponentsInChildren<MeshRenderer>();

        SetColorAndBuildStatus(green035alpha, true);
    }

    private void SetColorAndBuildStatus(Color _color, bool buildStatus)
    {
        canBuild = buildStatus;

        foreach (var renderer in meshRenderers)
        {
            var materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
                materials[i].color = _color;

            renderer.materials = materials;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("FenceConnection"))
            return;

        if (canBuild)
            SetColorAndBuildStatus(red035alpha, false);
    }

    private void OnTriggerExit(Collider other) => SetColorAndBuildStatus(green035alpha, true);

    private void SetCollidersAsTrigger()
    {
        var colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
            collider.isTrigger = true;
    }
}
