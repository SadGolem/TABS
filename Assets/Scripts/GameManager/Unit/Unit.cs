using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    public int health;
    public int damage;

    public virtual void Move(Vector3 newPosition)
    {
        // ���������� ����������� �����
    }

    public virtual void Attack(Unit target)
    {
        // ���������� �����
    }
}

