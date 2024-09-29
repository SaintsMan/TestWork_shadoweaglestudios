using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemie : MonoBehaviour
{
    [SerializeField] private float damageCount;
    [SerializeField] private float startCountHp;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackRange = 2;
    [SerializeField] private float rotationSpeed = 5f; 
    [SerializeField] private Animator animatorController;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private EnemyType thisType;
    private float lastAttackTime = 0;
    private bool isDead = false;
    private float hp;
    private void OnEnable()
    {
        GlobalEvent.GameOverEvent += DestroyLogic;
        GlobalEvent.GameWinEvent += DestroyLogic;
    }
    private void Start()
    {
        hp = startCountHp;
        agent.SetDestination(SceneManager.Instance.Player.transform.position);
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (hp <= 0)
        {
            Die();
            agent.isStopped = true;
            return;
        }

        var playerPosition = SceneManager.Instance.Player.transform.position;
        var distance = Vector3.Distance(transform.position, playerPosition);

        if (distance <= attackRange)
        {
            animatorController.SetFloat("Speed", 0);
            agent.isStopped = true;

            Vector3 directionToPlayer = (playerPosition - transform.position).normalized;
            RotateTowardsPlayer(directionToPlayer);

            if (Time.time - lastAttackTime > attackSpeed)
            {
                lastAttackTime = Time.time;
                SceneManager.Instance.Player.DamagePlayer(damageCount);
                animatorController.SetTrigger("Attack");
            }
        }
        else
        {
            animatorController.SetFloat("Speed", agent.speed);
            agent.isStopped = false;

            agent.SetDestination(playerPosition);

            Vector3 directionToPlayer = (playerPosition - transform.position).normalized;
            RotateTowardsPlayer(directionToPlayer);
        }
    }

    private void RotateTowardsPlayer(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void DamageEnemy(float countDamage)
    {
        hp -= countDamage;
        Mathf.Clamp(hp, 0f, startCountHp);
    }

    public virtual void Die()
    {
        if (thisType == EnemyType.Super)
        {
            GlobalEvent.DeadSuperGoblin?.Invoke();
        }

        SceneManager.Instance.enemySpawner.RemoveEnemie(this);
        SceneManager.Instance.Player.Plus10HP();
        isDead = true;
        animatorController.SetTrigger("Die");
    }
    private void DestroyLogic()
    {
        Destroy(this);
    }
    private void OnDestroy()
    {
        GlobalEvent.GameOverEvent -= DestroyLogic;
        GlobalEvent.GameWinEvent -= DestroyLogic;
    }
}
