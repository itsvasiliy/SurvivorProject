using UnityEngine;
using UnityEngine.UI;

public class OpenStructuresBTN : MonoBehaviour
{
    [SerializeField] private GameObject structuresPanel;

    [SerializeField] private Image _icon;

    [SerializeField] private Sprite buildIcon;
    [SerializeField] private Sprite closeIcon;

    private bool isOpen = false;

    public void OnClick()
    {
        isOpen = !isOpen;

        structuresPanel.SetActive(isOpen);

        if(isOpen)
        {
            _icon.sprite = closeIcon;
        }
        else
        {
            _icon.sprite = buildIcon;
        }
    }
}
