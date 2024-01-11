using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawnButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject prefabToSpawn; // ������ ��� ������

    private GameObject currentSpawnInstance; // ������ �� ������� ��������� ������
    private bool _isPlacing;
    public Camera _camera;
    public void OnPointerDown(PointerEventData eventData)
    {
        currentSpawnInstance = Instantiate(prefabToSpawn); // ������� ��������� �������
        MoveSpawnInstanceToMousePosition(); // ���������� � ��������� ������� ����
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentSpawnInstance != null)
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPosition.z = 0; // ��������� ���������, �� ������� ����� �������� ������
            currentSpawnInstance.transform.position = spawnPosition; // ��������� ��������� � ������� ����
            currentSpawnInstance = null; // ������� ������
        }
    }

    private void Update()
    {
        if (currentSpawnInstance != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject()) // ���������, ��� ������ �� ��������� ��� UI ���������
                {
                    _isPlacing = true;
                    currentSpawnInstance = Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
                }
            }

            if (_isPlacing && currentSpawnInstance != null)
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                float distance;
                if (plane.Raycast(ray, out distance))
                {
                    Vector3 point = ray.GetPoint(distance);
                    currentSpawnInstance.transform.position = point;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    _isPlacing = false;
                    currentSpawnInstance = null;
                }
            } // ���������� ��������� � ������� ����
        }
    }

    private void MoveSpawnInstanceToMousePosition()
    {
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnPosition.z = 0; // ��������� ���������, �� ������� ����� �������� ������
        currentSpawnInstance.transform.position = spawnPosition; // ���������� ��������� � ������� ����
    }
}


