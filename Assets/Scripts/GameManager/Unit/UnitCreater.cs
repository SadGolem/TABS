
using UnityEngine;

public class UnitCreater : MonoBehaviour
{
    [SerializeField] Unit _currentUnit;
    [SerializeField] Camera _camera;

    private void Update()
    {
        if(_currentUnit)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            float distance;
            plane.Raycast(ray, out distance);
            Vector3 point = ray.GetPoint(distance);

            _currentUnit.transform.position = point;

            if(Input.GetMouseButtonUp(0))
            {
               // _currentUnit.Place();
                _currentUnit = null;
            }
        }
    }

    public void Create(Unit unit)
    {
        Instantiate(unit);
    }
}
