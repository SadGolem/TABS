using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSpawnButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject prefabToSpawn; // ������ ��� ������
    public NavMeshSurface surface;

    private GameObject currentSpawnInstance; // ������ �� ������� ��������� ������
    private bool _isPlacing;
    public Camera _camera;
    public NavMeshHit hit; // ��� �������� ���������� ��������� �� NavMesh

    public void OnPointerDown(PointerEventData eventData)
    {
        if (UnitManager.instance.GetFriendUnits().Count == 7 || !GameManager.Instance.isNotLastTurn)
        { return; }
        else
        {
            currentSpawnInstance = Instantiate(prefabToSpawn); // ������� ��������� �������

            MoveSpawnInstanceToMousePosition(); // ���������� � ��������� ������� ����
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentSpawnInstance != null)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 point = ray.GetPoint(distance);
                if (NavMesh.SamplePosition(point, out hit, 3.0f, surface.defaultArea)) // ���������, ��� ����� ��������� �� NavMesh
                {
                    currentSpawnInstance.transform.position = hit.position; // ��������� ��������� �� ������� NavMesh
                    /* UnitManager.instance.AddFriendUnits2(prefabToSpawn.GetComponent<Unit>());*/
                    UnitManager.instance.RegisterUnit(currentSpawnInstance.GetComponent<Unit>());
                    GameManager.Instance.EndTurn();
                }
                else
                {
                    // ���� ����� ��� NavMesh (��������, �� ����������� �����������), ����� ����� �������� ���������
                    // ��������, ����� ��������� ���������
                    Destroy(currentSpawnInstance);
                }

                currentSpawnInstance = null; // ������� ������
            }
        }
    }

    private void Update()
    {
        if (currentSpawnInstance != null)
        {
            if (_isPlacing)
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                float distance;

                if (plane.Raycast(ray, out distance))
                {
                    Vector3 point = ray.GetPoint(distance);
                    currentSpawnInstance.transform.position = point; // ���������� ��������� � ������� ����
                }

                if (Input.GetMouseButtonUp(0))
                {
                    _isPlacing = false;
                    currentSpawnInstance = null; // ������� ������
                }
            }
            else
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                float distance;

                if (plane.Raycast(ray, out distance))
                {
                    Vector3 point = ray.GetPoint(distance);
                    currentSpawnInstance.transform.position = point; // ���������� ��������� � ������� ����
                }
            }
        }
    }

    private void MoveSpawnInstanceToMousePosition()
    {
        Vector3 spawnPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        spawnPosition.z = 0; // ��������� ���������, �� ������� ����� �������� ������
        currentSpawnInstance.transform.position = spawnPosition; // ���������� ��������� � ������� ����
    }
}



