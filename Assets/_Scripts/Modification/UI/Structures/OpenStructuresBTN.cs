using UnityEngine;

public class OpenStructuresBTN : MonoBehaviour
{
    [SerializeField] private GameObject structuresPanel;

    public void OnClick()
    {
        structuresPanel.SetActive(!structuresPanel.activeSelf);
    }
}
