using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthFill; // Ссылка на Image для отображения текущего здоровья
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
        // Проверяем деление на ноль (например, если maxHealth был установлен неправильно)
        if (maxHealth == 0)
        {
            Debug.LogWarning("maxHealth должен быть установлен больше нуля.");
            return;
        }

        healthFill.value = currentHealth / maxHealth; // Обновляем значение здоровья в слайдере
    }

/*    private void Start()
    {
        // Устанавливаем значение maxHealth в соответствии с начальным значением слайдера
        maxHealth = healthFill.maxValue;
    }*/

    private void Update()
    {
        if (unit != null)
        {
            UpdateHealthBar(unit.health); // Вызываем метод обновления здоровья при обновлении каждого кадра
        }
    }
}


