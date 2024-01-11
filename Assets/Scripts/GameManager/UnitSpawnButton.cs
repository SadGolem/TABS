using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawnButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject prefabToSpawn; // ѕрефаб дл€ спавна

    private GameObject currentSpawnInstance; // —сылка на текущий экземпл€р спауна
    private bool _isPlacing;
    public Camera _camera;
    public void OnPointerDown(PointerEventData eventData)
    {
        currentSpawnInstance = Instantiate(prefabToSpawn); // —оздаем экземпл€р префаба
        MoveSpawnInstanceToMousePosition(); // ѕеремещаем в начальную позицию мыши
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentSpawnInstance != null)
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPosition.z = 0; // ”казываем плоскость, на которой будет размещен объект
            currentSpawnInstance.transform.position = spawnPosition; // –азмещаем экземпл€р в позиции мыши
            currentSpawnInstance = null; // ќчищаем ссылку
        }
    }

    private void Update()
    {
        if (currentSpawnInstance != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject()) // ѕровер€ем, что курсор не находитс€ над UI элементом
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
            } // ѕеремещаем экземпл€р в позицию мыши
        }
    }

    private void MoveSpawnInstanceToMousePosition()
    {
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnPosition.z = 0; // ”казываем плоскость, на которой будет размещен объект
        currentSpawnInstance.transform.position = spawnPosition; // ѕеремещаем экземпл€р в позицию мыши
    }
}


