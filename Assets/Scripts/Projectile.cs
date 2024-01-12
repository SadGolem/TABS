using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 targetPosition;
    public bool isDoletelo = false;
    private Unit owner;
    private Unit target;
    public int areaDamage;

    public void Launch(Vector3 target, Unit owner, Unit targetUnit, int damage)
    {
        // ��������� ������ � ������� ����
        targetPosition = target;
        this.owner = owner;
        this.target = targetUnit;
        this.areaDamage = damage;
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

    private void OnCollisionEnter(Collision collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();

        if (unit != null && unit.team != owner.team)
        {
            owner.TakeDamage(target, areaDamage);
        }

        // ���������� �� ����������, ����� ������������ ���������� ������.
        Destroy(gameObject);
    }

}

