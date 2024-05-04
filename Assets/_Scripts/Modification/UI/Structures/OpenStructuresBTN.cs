using UnityEngine;

public class OpenStructuresBTN : MonoBehaviour
{
    [SerializeField] private GameObject PlayerJoystick;
    [SerializeField] private GameObject StructuresPanel;

    public void OnClick()
    {
        PlayerJoystick.SetActive(!PlayerJoystick.activeSelf);
        StructuresPanel.SetActive(!StructuresPanel.activeSelf);
    }
}
