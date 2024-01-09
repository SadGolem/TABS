using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DragAndDropCard : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 startPosition;
    [SerializeField] private RectTransform cardZoneRectTransform;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Camera mainCamera;
    private Transform originalParent;

    public void OnMouseDowner()
    {
        isDragging = true;
        startPosition = transform.position;
        originalParent = transform.parent;

        // Убираем привязку к канвасу и делаем объект дочерним по отношению к главной камере
        transform.SetParent(mainCamera.transform);

        // Увеличиваем масштаб карты для визуального эффекта
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public void OnMouseDragging()
    {
        if (isDragging)
        {
            // Получаем координаты мыши в трехмерном пространстве, связанные с главной камерой
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            transform.position = new Vector3(mousePosition.x, 8.027767f, mousePosition.z);
        }
    }

    public void OnMouseUpper()
    {
        isDragging = false;
        transform.SetParent(originalParent);

        // Возвращаем масштаб карты к исходному
        transform.localScale = new Vector3(1f, 1f, 1f);

        // Получаем текущую позицию мыши в трехмерном пространстве
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

        // Определяем расстояние по оси y от текущей позиции до камеры
        float distanceFromCamera = Mathf.Abs((mainCamera.transform.position.y - mousePosition.y)*10000);

        // Проверяем, была ли карта отпущена за пределами зоны карт
        if (!RectTransformUtility.RectangleContainsScreenPoint(cardZoneRectTransform, Input.mousePosition))
        {
            // Карта исчезает из колоды руки и применяется на поле
            CreateCardPrefabAtPosition(mousePosition, distanceFromCamera);
            gameObject.SetActive(false); // исчезает
        }
        else
        {
            transform.position = startPosition; // возвращаем карту на начальное положение
        }
    }

    void CreateCardPrefabAtPosition(Vector3 position, float distanceFromCamera)
    {
        float spawnHeight = 8.5f;

        // Используем удаленность по оси y от камеры для определения размера области поиска
        float searchRadius = Mathf.Clamp(distanceFromCamera, 10000f, 5000f);

        Vector3 spawnPosition = new Vector3(position.x, spawnHeight, position.z);

        // Используем NavMesh.SamplePosition для поиска ближайшей точки на навмеше
        NavMeshHit hit;

        if (NavMesh.SamplePosition(spawnPosition, out hit, searchRadius, NavMesh.AllAreas))
        {
            // Если найдена доступная точка на навмеше, создаем объект в этой точке
            Instantiate(cardPrefab, hit.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Не удалось найти доступную точку на навмеше для спауна карты");
        }
    }
}

