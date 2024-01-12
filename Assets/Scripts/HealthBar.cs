using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthFill; // ������ �� Image ��� ����������� �������� ��������
    public Unit unit;
    public float maxHealth;
    public void SetHealthBarPosition(Transform targetTransform)
    {
        Vector3 targetPosition = targetTransform.position;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetPosition);
        transform.position = screenPosition;
    }

    public void UpdateHealthBar(float currentHealth)
    {
        // ��������� ������� �� ���� (��������, ���� maxHealth ��� ���������� �����������)
        if (maxHealth == 0)
        {
            Debug.LogWarning("maxHealth ������ ���� ���������� ������ ����.");
            return;
        }

        healthFill.value = currentHealth / maxHealth; // ��������� �������� �������� � ��������
    }

/*    private void Start()
    {
        // ������������� �������� maxHealth � ������������ � ��������� ��������� ��������
        maxHealth = healthFill.maxValue;
    }*/

    private void Update()
    {
        if (unit != null)
        {
            UpdateHealthBar(unit.health); // �������� ����� ���������� �������� ��� ���������� ������� �����
        }
    }
}


