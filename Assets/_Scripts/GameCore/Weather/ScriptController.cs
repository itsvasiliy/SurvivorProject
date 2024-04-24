using UnityEngine;

public class ScriptController : MonoBehaviour
{
    public MonoBehaviour scriptToToggle; // Скрипт, который нужно включать и выключать

    private float toggleDelayMin = 20f; // Минимальная задержка перед включением или выключением скрипта
    private float toggleDelayMax = 600f; // Максимальная задержка перед включением или выключением скрипта

    private float nextToggleTime; // Время следующего включения или выключения скрипта
    private bool isScriptActive; // Состояние скрипта (включен или выключен)

    private void Start()
    {
        // Задаем время первого включения или выключения скрипта
        nextToggleTime = Time.time + Random.Range(60f, 1200f); // От 1 до 20 минут
        // Устанавливаем начальное состояние скрипта в зависимости от того, активен ли он сейчас
        isScriptActive = scriptToToggle.enabled;
    }

    private void Update()
    {
        // Если пришло время включения или выключения скрипта
        if (Time.time >= nextToggleTime)
        {
            // Меняем состояние скрипта на противоположное
            isScriptActive = !isScriptActive;
            // Включаем или выключаем скрипт
            scriptToToggle.enabled = isScriptActive;
            // Генерируем новое время для следующего включения или выключения скрипта
            float randomDelay = Random.Range(toggleDelayMin, toggleDelayMax);
            nextToggleTime = Time.time + randomDelay;
            // Выводим сообщение в консоль, чтобы увидеть, через сколько времени произойдет следующее включение или выключение
            Debug.Log(isScriptActive ? "Скрипт включен через " + randomDelay + " секунд." : "Скрипт выключен через " + randomDelay + " секунд.");
        }
    }
}
