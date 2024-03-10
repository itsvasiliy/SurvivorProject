using UnityEngine;
using UnityEngine.UI;

public class NetworkHealthSlider : MonoBehaviour
{
    [SerializeField] Slider healthSlider; // Reference to the UI Slider for health
    private NetworkObjectHealth networkObjectHealth;


    void Awake()
    {
        networkObjectHealth = GetComponent<NetworkObjectHealth>();
        healthSlider.maxValue = networkObjectHealth.maxHealth;
        healthSlider.value = networkObjectHealth._health.Value;
    }

    private void OnEnable() => networkObjectHealth._health.OnValueChanged += HealthChanged;

    private void HealthChanged(int previousValue, int newValue) => healthSlider.value = networkObjectHealth._health.Value;

    private void OnDisable() => networkObjectHealth._health.OnValueChanged -= HealthChanged;


}
