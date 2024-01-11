using UnityEngine;

public class MageUnit : Unit
{
    // Дополнительные характеристики для магического юнита
    [Header("Mage Specific")]
    [SerializeField] protected int areaDamage = 5; // Урон по области
    [SerializeField] private GameObject projectilePrefab;

    protected MageUnit()
    {
        health = 5;
        moveSpeed = 1;
        attackRange = 40;
        damage = 1;
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
            // Нападаем на врага, если он в пределах атаки
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            if (distanceToTarget <= attackRange)
            {
                state = State.Attack;
                Attack(currentTarget, this.transform);
            }
            else
            {
                state = State.WalkToPoint;
                Move(currentTarget.position);
            }
        }
    }

    // Override метода атаки для магического юнита
    public override void Attack(Transform target, Transform attackUnit)
    {
        state = State.Attack;
        AreaAttack(target);
    }

    // Логика для атаки по области
    protected void AreaAttack(Transform target)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        Vector3 attackPosition = target.position; // Сохраняем позицию цели для атаки

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag != "Terrain")
            {
                Unit unit = hitCollider.GetComponent<Unit>();
                if (unit != null && unit.team != this.team)
                {   
                    // Создаем и запускаем летящий объект в сторону цели
                    GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                    Projectile projectileScript = projectile.GetComponent<Projectile>();
                    if (projectileScript != null)
                    {
                        projectileScript.Launch(attackPosition);
                        this.TakeDamage(unit, areaDamage);
                    }
                }
            }
        }
    }


    // Логика для движения к цели и обычной атаки
    protected void MoveTowardsAndAttack(Transform target, Transform attackUnit)
    {
        if (this.state == State.WalkToPoint)
        {
            agent.speed = moveSpeed; // Устанавливаем скорость перемещения
            agent.SetDestination(target.transform.position);
            /*state = State.WalkToPoint;*/
        }
    }

}

