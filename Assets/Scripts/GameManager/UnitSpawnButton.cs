using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSpawnButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject prefabToSpawn; // Префаб для спавна
    public NavMeshSurface surface;

    private GameObject currentSpawnInstance; // Ссылка на текущий экземпляр спауна
    private bool _isPlacing;
    public Camera _camera;
    public NavMeshHit hit; // Для хранения результата попадания на NavMesh

    public void OnPointerDown(PointerEventData eventData)
    {
        if (UnitManager.instance.GetFriendUnits().Count == 7 || !GameManager.Instance.isNotLastTurn)
        { return; }
        else
        {
            currentSpawnInstance = Instantiate(prefabToSpawn); // Создаем экземпляр префаба

            MoveSpawnInstanceToMousePosition(); // Перемещаем в начальную позицию мыши
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
                if (NavMesh.SamplePosition(point, out hit, 3.0f, surface.defaultArea)) // Проверяем, что точка находится на NavMesh
                {
                    currentSpawnInstance.transform.position = hit.position; // Размещаем экземпляр на позиции NavMesh
                    /* UnitManager.instance.AddFriendUnits2(prefabToSpawn.GetComponent<Unit>());*/
                    UnitManager.instance.RegisterUnit(currentSpawnInstance.GetComponent<Unit>());
                    GameManager.Instance.EndTurn();
                }
                else
                {
                    // Если точка вне NavMesh (например, на недоступной поверхности), здесь можно добавить обработку
                    // Например, можно разрушить экземпляр
                    Destroy(currentSpawnInstance);
                }

                currentSpawnInstance = null; // Очищаем ссылку
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
                    currentSpawnInstance.transform.position = point; // Перемещаем экземпляр в позицию мыши
                }

                if (Input.GetMouseButtonUp(0))
                {
                    _isPlacing = false;
                    currentSpawnInstance = null; // Очищаем ссылку
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
                    currentSpawnInstance.transform.position = point; // Перемещаем экземпляр в позицию мыши
                }
            }
        }
    }

    private void MoveSpawnInstanceToMousePosition()
    {
        Vector3 spawnPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        spawnPosition.z = 0; // Указываем плоскость, на которой будет размещен объект
        currentSpawnInstance.transform.position = spawnPosition; // Перемещаем экземпляр в позицию мыши
    }
}



