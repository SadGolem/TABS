using System.Collections;
using UnityEngine;

public class ArcherUnit : Unit
{
    [SerializeField] private GameObject projectilePrefab;
    private bool canShoot = true; // Флаг, который указывает, можно ли производить выстрел
    public float shootDelay = 1.0f; // Промежуток между выстрелами
    protected ArcherUnit()
    {
        health = 15;
        moveSpeed = 2.5f;
        damage = 2;
    }

    public override void Move(Vector3 newPosition)
    {
        if (this.state == State.WalkToPoint)
        {
            agent.speed = moveSpeed; // Устанавливаем скорость перемещения
            agent.SetDestination(newPosition);
            state = State.WalkToPoint;
        }
    }

    void Update()
    {
        // Логика выбора врага и просчета лучшей атаки будет здесь
        if (currentTarget == null)
        {
            currentTarget = FindBestTarget();
            Debug.Log(currentTarget);
        }

        if (currentTarget != null)
        {
            if (getBackNOW)
            {
                state = State.WalkToPoint;
                GetBack(currentTarget);
                return;
            }
            // Нападаем на врага, если он в пределах атаки
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            if (distanceToTarget <= attackRange)
            {
                state = State.Attack;
                Attack(currentTarget, this.transform);
                currentTarget = null;
            }
            else
            {
                state = State.WalkToPoint;
                Move(currentTarget.position);
            }
        }
        GetBack(currentTarget);
    }

    public override void Attack(Transform target, Transform attackUnit)
    {
        state = State.Attack;
        ArrowAttack(target);
    }

    protected void ArrowAttack(Transform target)
    {
        if (canShoot)
        {
            Vector3 attackPosition = target.position; // Сохраняем позицию цели для атаки

            Unit unit = target.GetComponent<Unit>();
            if (unit != null && unit.team != this.team)
            {
                // Запускаем выстрел
                StartCoroutine(ShootProjectile(attackPosition, unit));
                StartCoroutine(ShootDelay());
            }
        }
    }

    // Корутина для выстрела
    private IEnumerator ShootProjectile(Vector3 attackPosition, Unit unit)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.Launch(attackPosition, this, unit, damage);
            /*this.TakeDamage(unit, damage);*/
        }
        yield return null;
    }

    // Корутина для задержки между выстрелами
    private IEnumerator ShootDelay()
    {
        canShoot = false; // Нельзя стрелять, пока активна задержка
        yield return new WaitForSeconds(shootDelay);
        canShoot = true; // Разрешаем следующий выстрел
    }
}



