using TMPro;
using UnityEngine;

public class RespawnTimer : MonoBehaviour
{
    [SerializeField] private PlayerHealthController healthController;

    [SerializeField] private float startTime;

    private float currentTime;
    private TextMeshProUGUI m_TextMeshPro;

    private void Start() => m_TextMeshPro = GetComponent<TextMeshProUGUI>();

    private void OnEnable() => currentTime = startTime;

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            healthController.Respawn();
        }
    }

    void UpdateTimerDisplay()
    {
        int seconds = Mathf.FloorToInt(currentTime % 60);
        seconds += 1;
        m_TextMeshPro.text = seconds.ToString();
    }
}
