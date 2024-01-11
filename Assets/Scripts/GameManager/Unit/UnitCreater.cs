
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitCreater : MonoBehaviour
{
    private Unit _unitPrefab;
    [SerializeField] Camera _camera;

    private bool _isPlacing = false;
    private Unit _currentUnitInstance;

    public void SetUnitPrefab(Unit prefab)
    {
        _unitPrefab = prefab;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!EventSystem.current.IsPointerOverGameObject()) // Проверяем, что курсор не находится над UI элементом
            {
                _isPlacing = true;
                _currentUnitInstance = Instantiate(_unitPrefab, Vector3.zero, Quaternion.identity);
            }
        }

        if (_isPlacing && _currentUnitInstance != null)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 point = ray.GetPoint(distance);
                _currentUnitInstance.transform.position = point;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isPlacing = false;
                _currentUnitInstance = null;
            }
        }
    }
}
