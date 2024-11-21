using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PonyAI : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public enum PonyState { Idle, Chase, Attack, Dead }

    public PonyState currentState = PonyState.Idle;

    public Transform player;

    public float chaseDistance = 10f;
    public float attackDistance = 2f;
    public float attackCooldown = 2f;
    public float attackDelay = 1.5f;
    public int damage = 10;

    // Health variables
    public int maxHealth = 100;
    [SerializeField] // Visible in Inspector
    private int currentHealth;

    private bool isAttacking;
    private float lastAttackTime;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        lastAttackTime = -attackCooldown;
        currentHealth = maxHealth; // Initialize current health
    }

    void Update()
    {
        if (player == null) return; // Ensure the player is assigned

        switch (currentState)
        {
            case PonyState.Idle:
                if (Vector3.Distance(transform.position, player.position) <= chaseDistance)
                    currentState = PonyState.Chase;
                break;

            case PonyState.Chase:
                navAgent.SetDestination(player.position);
                if (Vector3.Distance(transform.position, player.position) <= attackDistance)
                    currentState = PonyState.Attack;
                break;

            case PonyState.Attack:
                navAgent.SetDestination(transform.position); // Stop moving
                if (!isAttacking && Time.time - lastAttackTime >= attackCooldown)
                {
                    StartCoroutine(AttackWithDelay());
                }
                if (Vector3.Distance(transform.position, player.position) > attackDistance)
                    currentState = PonyState.Chase;
                break;

            case PonyState.Dead:
                Die();
                break;
        }
    }

    private IEnumerator AttackWithDelay()
    {
        isAttacking = true;

        // Damage the player
        PlayerScript playerDamage = player.GetComponent<PlayerScript>();
        if (playerDamage != null)
        {
            playerDamage.TakeDamage(damage);
        }

        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
        lastAttackTime = Time.time;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(name + " takes damage: " + damageAmount + " | Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentState = PonyState.Dead;
        }
    }

    private void Die()
    {
        Debug.Log(name + " is dead.");
        Destroy(gameObject); // Destroy this pony's GameObject
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BulletSpawn"))
        {
            Debug.Log(name + " hit by bullet!");
            TakeDamage(10); // Damage amount from bullet
            Destroy(other.gameObject); // Optionally destroy the bullet
        }
    }
}
