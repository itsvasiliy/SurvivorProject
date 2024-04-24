using UnityEngine;

public class ScriptController : MonoBehaviour
{
    public MonoBehaviour scriptToToggle; // ������, ������� ����� �������� � ���������

    private float toggleDelayMin = 20f; // ����������� �������� ����� ���������� ��� ����������� �������
    private float toggleDelayMax = 600f; // ������������ �������� ����� ���������� ��� ����������� �������

    private float nextToggleTime; // ����� ���������� ��������� ��� ���������� �������
    private bool isScriptActive; // ��������� ������� (������� ��� ��������)

    private void Start()
    {
        // ������ ����� ������� ��������� ��� ���������� �������
        nextToggleTime = Time.time + Random.Range(60f, 1200f); // �� 1 �� 20 �����
        // ������������� ��������� ��������� ������� � ����������� �� ����, ������� �� �� ������
        isScriptActive = scriptToToggle.enabled;
    }

    private void Update()
    {
        // ���� ������ ����� ��������� ��� ���������� �������
        if (Time.time >= nextToggleTime)
        {
            // ������ ��������� ������� �� ���������������
            isScriptActive = !isScriptActive;
            // �������� ��� ��������� ������
            scriptToToggle.enabled = isScriptActive;
            // ���������� ����� ����� ��� ���������� ��������� ��� ���������� �������
            float randomDelay = Random.Range(toggleDelayMin, toggleDelayMax);
            nextToggleTime = Time.time + randomDelay;
            // ������� ��������� � �������, ����� �������, ����� ������� ������� ���������� ��������� ��������� ��� ����������
            Debug.Log(isScriptActive ? "������ ������� ����� " + randomDelay + " ������." : "������ �������� ����� " + randomDelay + " ������.");
        }
    }
}
