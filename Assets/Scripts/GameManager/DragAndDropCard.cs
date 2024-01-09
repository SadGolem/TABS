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

        // ������� �������� � ������� � ������ ������ �������� �� ��������� � ������� ������
        transform.SetParent(mainCamera.transform);

        // ����������� ������� ����� ��� ����������� �������
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public void OnMouseDragging()
    {
        if (isDragging)
        {
            // �������� ���������� ���� � ���������� ������������, ��������� � ������� �������
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            transform.position = new Vector3(mousePosition.x, 8.027767f, mousePosition.z);
        }
    }

    public void OnMouseUpper()
    {
        isDragging = false;
        transform.SetParent(originalParent);

        // ���������� ������� ����� � ���������
        transform.localScale = new Vector3(1f, 1f, 1f);

        // �������� ������� ������� ���� � ���������� ������������
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

        // ���������� ���������� �� ��� y �� ������� ������� �� ������
        float distanceFromCamera = Mathf.Abs((mainCamera.transform.position.y - mousePosition.y)*10000);

        // ���������, ���� �� ����� �������� �� ��������� ���� ����
        if (!RectTransformUtility.RectangleContainsScreenPoint(cardZoneRectTransform, Input.mousePosition))
        {
            // ����� �������� �� ������ ���� � ����������� �� ����
            CreateCardPrefabAtPosition(mousePosition, distanceFromCamera);
            gameObject.SetActive(false); // ��������
        }
        else
        {
            transform.position = startPosition; // ���������� ����� �� ��������� ���������
        }
    }

    void CreateCardPrefabAtPosition(Vector3 position, float distanceFromCamera)
    {
        float spawnHeight = 8.5f;

        // ���������� ����������� �� ��� y �� ������ ��� ����������� ������� ������� ������
        float searchRadius = Mathf.Clamp(distanceFromCamera, 10000f, 5000f);

        Vector3 spawnPosition = new Vector3(position.x, spawnHeight, position.z);

        // ���������� NavMesh.SamplePosition ��� ������ ��������� ����� �� �������
        NavMeshHit hit;

        if (NavMesh.SamplePosition(spawnPosition, out hit, searchRadius, NavMesh.AllAreas))
        {
            // ���� ������� ��������� ����� �� �������, ������� ������ � ���� �����
            Instantiate(cardPrefab, hit.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("�� ������� ����� ��������� ����� �� ������� ��� ������ �����");
        }
    }
}

