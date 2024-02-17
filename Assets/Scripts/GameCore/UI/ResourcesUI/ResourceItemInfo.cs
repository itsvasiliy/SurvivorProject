using Assets.Scripts.GameCore.Interfaces;
using Assets.Scripts.GameCore.Resources;
using TMPro;
using UnityEngine;
using Zenject;

public class ResourceItemInfo : MonoBehaviour
{
    public ResourceTypes type;

    [SerializeField] TextMeshProUGUI amountText;

    [Inject] readonly IResourceController resourceController;

    public void UpdateAmountFromRepository() =>
        amountText.text = resourceController.GetResourceAmount(type).ToString();

    public void SetAmount(int amount) => amountText.text = amount.ToString();
}
