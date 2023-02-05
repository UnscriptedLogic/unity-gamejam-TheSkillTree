using External.CustomSlider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.MathUtils;

public class Unit : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float health = 10f;
    [SerializeField] private int team = 1;
    [SerializeField] private WorldSpaceCustomSlider healthbar;
    [SerializeField] private Rigidbody2D rb;

    private float currentHealth;
    private Transform target;
    private bool initialized;

    public float MaxHealth => health;
    public float CurrentHealth => currentHealth;
    public float Speed => Speed;
    public int Team => team;

    private void Start()
    {
        currentHealth = health;

        healthbar.SetLimits(currentHealth, health);
    }

    public void Initialize(Transform target)
    {
        this.target = target;
        initialized = true;
    }

    public void ModifyHealth(MathLogic.ModificationType modificationType, float amount, int team)
    {
        if (team == this.team) return;

        switch (modificationType)
        {
            case MathLogic.ModificationType.Add:
                currentHealth += amount;
                break;
            case MathLogic.ModificationType.Subtract:
                currentHealth -= amount;
                break;
            case MathLogic.ModificationType.Set:
                currentHealth = amount;
                break;
            case MathLogic.ModificationType.Divide:
                currentHealth /= amount;
                break;
            case MathLogic.ModificationType.Multiply:
                currentHealth *= amount;
                break;
            default:
                Debug.Log("Modification type not found");
                break;
        }

        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }

        healthbar.SetValue(currentHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.ModifyHealth(MathLogic.ModificationType.Subtract, currentHealth, team);
            ModifyHealth(MathLogic.ModificationType.Subtract, damageable.CurrentHealth, -1);
        }
    }

    private void Update()
    {
        if (!initialized) return;
        if (target == null) return;

        rb.MovePosition((Vector2)transform.position + (Vector2)(target.position - transform.position).normalized * movementSpeed * Time.deltaTime);
    }
}
