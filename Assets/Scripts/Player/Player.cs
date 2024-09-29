using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float damageCount;
    [SerializeField] private float damageSuperCount;
    [SerializeField] private float startCountHp;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackRange = 2;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Animator animatorController;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Button attackButton;

    private float lastAttackTime = 0;
    private bool isDead = false;
    private float hp;
    private Vector3 movement;
    private bool isAttacking = false;
    private void OnEnable()
    {
        GlobalEvent.GameOverEvent += DestroyLogic;
        GlobalEvent.GameWinEvent += DestroyLogic;
    }
    private void Start()
    {
        hp = startCountHp;
        healthBar.SetMaxHealth(hp);

        attackButton.onClick.AddListener(Attack);
    }

    private void Update()
    {
        if (isDead) return;

        if (hp <= 0)
        {
            Die();
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movement = new Vector3(horizontal, 0, vertical).normalized;

        if (movement.magnitude >= 0.1f && isAttacking == false)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

            animatorController.SetFloat("Speed", movement.magnitude);
        }
        else
        {
            animatorController.SetFloat("Speed", 0);
        }

    }

    private void Attack()
    {
        if (Time.time - lastAttackTime < attackSpeed || isAttacking) return;

        lastAttackTime = Time.time;

        StartCoroutine(PerformAttack("Attack", damageCount));
    }

    public void SuperAttack()
    {
        if (isAttacking == true) return;

        StartCoroutine(PerformAttack("SuperAttack", damageSuperCount));
    }

    private IEnumerator PerformAttack(string nameAnimation, float countDamage)
    {
        isAttacking = true;
        animatorController.SetTrigger(nameAnimation);
        yield return new WaitForSeconds(0.2f); // pause to damage

        var enemies = SceneManager.Instance.enemySpawner.Enemies;
        Enemie closestEnemie = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemie = enemies[i];
            if (enemie == null) continue;

            var directionToEnemy = (enemie.transform.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(transform.forward, directionToEnemy);

            if (dotProduct > 0.1f)
            {
                if (closestEnemie == null)
                {
                    closestEnemie = enemie;
                }
                else
                {
                    var distance = Vector3.Distance(transform.position, enemie.transform.position);
                    var closestDistance = Vector3.Distance(transform.position, closestEnemie.transform.position);

                    if (distance < closestDistance)
                    {
                        closestEnemie = enemie;
                    }
                }
            }
        }

        if (closestEnemie != null)
        {
            var distance = Vector3.Distance(transform.position, closestEnemie.transform.position);
            if (distance <= attackRange)
            {
                closestEnemie.DamageEnemy(countDamage);
            }
        }

        isAttacking = false;
    }

    public bool CheckEnemiesInRange()
    {
        var enemies = SceneManager.Instance.enemySpawner.Enemies;

        foreach (var enemie in enemies)
        {
            if (enemie != null)
            {
                float distance = Vector3.Distance(transform.position, enemie.transform.position);
                var directionToEnemy = (enemie.transform.position - transform.position).normalized;
                float dotProduct = Vector3.Dot(transform.forward, directionToEnemy);

                if (distance <= attackRange && dotProduct > 0.1f)
                {
                    return true;
                }
            }
        }
        return false;
    }


    public void DamagePlayer(float countDamage)
    {
        hp -= countDamage;
        UpdateBar();
    }

    public void Plus10HP()
    {
        hp += 10;
        UpdateBar();
    }

    private void UpdateBar()
    {
        hp = Mathf.Clamp(hp, 0f, startCountHp);
        healthBar.SetHealth(hp);
    }

    private void Die()
    {
        isDead = true;
        animatorController.SetTrigger("Die");

        GlobalEvent.GameOverEvent?.Invoke();
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
