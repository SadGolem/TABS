
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum State
{
    Idle,
    WalkToArena,
    WalkToPoint
}
public class Unit : MonoBehaviour
{
    [Header("Unit")]

    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Collider collider;
    Vector3 targetPoint;
    [SerializeField] protected State state;

    [SerializeField] protected float followDistance = 3f;
    [SerializeField] protected float attackDistance = 3f;
    [SerializeField] protected float moveSpeed = 8f;
    [SerializeField] protected float attackRange = 3f;

    [SerializeField] protected float walkSpeed = 3f;

    [HideInInspector]
    public UnityEvent OnSelect;
    [HideInInspector]
    public UnityEvent OnUnselect;

    protected int health = 10;
    public int damage= 1;



    public virtual void Move(Vector3 newPosition)
    {
        // Реализация перемещения юнита
    }

    public virtual void TakeDamage(Unit target)
    {
    }
}

