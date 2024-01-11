using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 targetPosition;
    public bool isDoletelo = false;

    public void Launch(Vector3 target)
    {
        // Запустить объект в сторону цели
        targetPosition = target;
    }

    private void Update()
    {
        // Двигаем объект к цели
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // При достижении цели - применяем эффект и разрушаем объект
        if (transform.position == targetPosition)
        {
            isDoletelo |= true;
           Destroy(gameObject);
        }
    }

}

