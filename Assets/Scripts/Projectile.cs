using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 targetPosition;
    public bool isDoletelo = false;

    public void Launch(Vector3 target)
    {
        // ��������� ������ � ������� ����
        targetPosition = target;
    }

    private void Update()
    {
        // ������� ������ � ����
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // ��� ���������� ���� - ��������� ������ � ��������� ������
        if (transform.position == targetPosition)
        {
            isDoletelo |= true;
           Destroy(gameObject);
        }
    }

}

