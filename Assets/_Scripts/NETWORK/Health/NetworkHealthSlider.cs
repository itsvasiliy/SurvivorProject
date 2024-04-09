using UnityEngine;
using UnityEngine.UI;

public class NetworkHealthSlider : MonoBehaviour
{
    [SerializeField] Slider healthSlider; // Reference to the UI Slider for health
    private IHealthController networkObjectHealth;


    void Awake()
    {
        networkObjectHealth = GetComponent<NetworkObjectHealth>();
        if(networkObjectHealth == null )
            networkObjectHealth = GetComponent<PlayerHealthController>();

        healthSlider.maxValue = networkObjectHealth.GetMaxHealth();
        healthSlider.value = networkObjectHealth.GetCurrentHealth();
    }

    private void OnEnable() => networkObjectHealth.GetHealthVariable().OnValueChanged += HealthChanged;

    private void HealthChanged(int previousValue, int newValue) => healthSlider.value = networkObjectHealth.GetCurrentHealth();

    private void OnDisable() => networkObjectHealth.GetHealthVariable().OnValueChanged -= HealthChanged;


}
