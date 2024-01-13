
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class СavalryUnit : Unit
{
    [SerializeField] float acceleration = 1f;
    [SerializeField] float confidenceThreshold = 0.6f; // Declaring the confidence attribute
    [SerializeField] float rangeBattleField = 50f;
    private UnitManager unitManager;

    List<Unit> unitsNearTheCavalry = new List<Unit>();
    private bool isConfidenceAttack = false;

    protected СavalryUnit()
    {
        health = 10;
        moveSpeed = 3;
        attackRange = 1;
        damage = 3;
    }

    private void OnEnable()
    {
        StartCoroutine(CheckConfidence());
    }

    void Update()
    {
        // Logic for enemy selection and determining the best attack will be here
        if (currentTarget == null)
        {
            currentTarget = FindBestTarget();
            Debug.Log(currentTarget);
        }

        if (currentTarget != null)
        {
            // Attack the enemy if within range
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            if (distanceToTarget <= attackRange)
            {
                state = State.Attack;
                // Учитываем уверенность перед атакой
                if (isConfidenceAttack)
                {
                    Attack(currentTarget, this.transform);
                }
                else
                {
                    GetBack(currentTarget);
                }
                currentTarget = null;
            }
            else
            {
                state = State.WalkToPoint;
                Move(currentTarget.position);
            }
        }
    }

    private void Start()
    {
        unitManager = UnitManager.instance;
    }

    public void Place()
    {

    }

    public override void Move(Vector3 newPosition)
    {
        if (this.state == State.WalkToPoint)
        {
            agent.speed = moveSpeed; // Устанавливаем скорость перемещения
            agent.acceleration = acceleration; // Устанавливаем ускорение
            agent.SetDestination(newPosition);
            state = State.WalkToPoint;
        }
    }
    public override void GetBack(Transform target) //побег
    {
        Vector3 directionToTarget = transform.position - target.position;
        Vector3 newPosition = transform.position + directionToTarget.normalized;

        Move(newPosition);
    }

    public override void Attack(Transform target, Transform attackUnit)
    {
        StartCoroutine(AttackWithDelay(target, attackUnit));
    }

    private IEnumerator AttackWithDelay(Transform target, Transform attackUnit)
    {
        // Ждем некоторое время перед атакой
        yield return new WaitForSeconds(2f);
        if (target != null)
        {
            if (Vector3.Distance(attackUnit.transform.position, target.position) <= attackRange)
            {
                Debug.Log(Vector3.Distance(transform.position, target.position));
                Unit targetUnit = target.GetComponent<Unit>();
                state = State.Attack;

                if (targetUnit is MageUnit || targetUnit is HealerUnit || targetUnit is ArcherUnit) // Преимущество над Магами, Лекарями, Лучниками
                {
                    targetUnit.TakeDamage(targetUnit, damage * 2); // Наносим двойной урон
                }
                else if (targetUnit is SpearmanUnit) // Слабее против Копейщиков
                {
                    targetUnit.TakeDamage(targetUnit, damage / 2); // Наносим половинный урон
                }
                else
                {
                    targetUnit.TakeDamage(targetUnit, damage); // Стандартный урон
                }
            }
            else
            {
                state = State.WalkToPoint;
                Move(target.position);
            }
        }
        else
            state = State.Idle;
    }

    private IEnumerator CheckConfidence()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            GetListAllUnits();
                // Рассчитываем уверенность на основе вашей предоставленной формулы
                int enemyCount = CalculateEnemyCount(); // Рассчитываем количество врагов, включая близлежащих
                int allyCount = CalculateFriendCount();  // Рассчитываем количество близлежащих союзников
                float enemyAdvantage = CalculateEnemyAdvantage(enemyCount); // Рассчитываем преимущество врага (0.2 за каждого врага)
                float selfAdvantage = CalculateSelfAdvantage(currentTarget); // Рассчитываем преимущество над основной целью (0.2)

                float calculatedConfidence = 1 /*- (0.2f * enemyCount)*/ + (0.2f * allyCount) - enemyAdvantage + selfAdvantage;

                Debug.Log("Рассчитанная уверенность: " + calculatedConfidence);

                // Принимаем решение в зависимости от уровня уверенности
                if (calculatedConfidence > confidenceThreshold)
                {
                    isConfidenceAttack = true;// уверенная атака
                }
                else if (calculatedConfidence == confidenceThreshold) { /* здесь добавить для 50/50 */ isConfidenceAttack = true; }
                else
                {
                    isConfidenceAttack = false; // пересмотрите цель или примите другие меры
                }
            /*}*/
        }
    }

        

    private void GetListAllUnits()
    {
        if (unitsNearTheCavalry.Count > 0)
        {
            unitsNearTheCavalry.Clear();
        }
        // Получаем всех коллайдеры в радиусе атаки
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, rangeBattleField);
        foreach(Collider col in hitColliders)
        {
            if (!col.CompareTag("Terrain"))
            {
                Unit unit = col.GetComponent<Unit>();
                unitsNearTheCavalry.Add(unit);
            }
        }
    }
    int CalculateEnemyCount()
    {
        int enemyCount = 0;

        // Перебираем найденные коллайдеры
        foreach (Unit unit in unitsNearTheCavalry)
        {
            // Проверяем, является ли объект врагом
            if (unit != null && unit.tag != this.tag )
            {
                enemyCount++;
            }
        }

        return enemyCount;
    }

    int CalculateFriendCount()
    {
        int friendsCount = 0;

        // Перебираем найденные коллайдеры
        foreach (Unit unit in unitsNearTheCavalry)
        {
            // Проверяем, является ли объект сокомандником
            if (unit != null && unit.tag == this.tag)
            {
                friendsCount++;
            }
        }

        return friendsCount;
    }

    float CalculateEnemyAdvantage(int enemyCount)
    {
        float enemyAdvantage = 0.0f;
        // Подсчитываем количество врагов разного типа
/*        int mageCount = 0;*/
        int healerCount = 0;
/*        int archerCount = 0;*/
        int spearmanCount = 0;
        int otherCount = 0;

        foreach (Unit unit in unitsNearTheCavalry)
        {
            if (unit != null && unit.tag != this.tag)
            {
                // Проверяем, является ли объект врагом нужного типа
                if (unit is MageUnit)
                {
                    /*mageCount++;*/
                }
                else if (unit is HealerUnit && enemyCount > 1 ) // Наедине хилеры слабее, чем в команде
                {
                    healerCount++;
                }
                else if (unit is ArcherUnit)
                {
                   /* archerCount++;*/
                }
                else if (unit is SpearmanUnit)
                {
                    spearmanCount++;
                }
                else
                {
                    otherCount++;
                }
            }
        }

        // Пример: Считаем общее преимущество врага на основе количества врагов разного типа
        enemyAdvantage = (float)(0.2f * otherCount + 0.4f * spearmanCount + 0.4 * healerCount);

        return enemyAdvantage;
    }


    float CalculateSelfAdvantage(Transform target)
    {
        float selfAdvantage = 0.0f;
        if (target != null)
        {
            Unit unit = target.transform.GetComponent<Unit>();

            if (unit != null)
            {
                if (unit is HealerUnit)
                {
                    selfAdvantage += 0.4f;
                }
                else if (unit is MageUnit)
                { selfAdvantage += 0.4f; }
                else if (unit is ArcherUnit) { selfAdvantage += 0.4f; }
                else { selfAdvantage += 0.2f; }

                // Логика определения преимущества текущего юнита над основной целью
            }
        }
        return selfAdvantage;
    }
}
