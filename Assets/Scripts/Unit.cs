using External.CustomSlider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic;

public class Unit : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float health = 10f;
    [SerializeField] private int team = 1;
    [SerializeField] private WorldSpaceCustomSlider healthbar;

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

    public void ModifyHealth(ModifyType modificationType, float amount, int team)
    {
        if (team == this.team) return;

        switch (modificationType)
        {
            case ModifyType.Add:
                currentHealth += amount;
                break;
            case ModifyType.Subtract:
                currentHealth -= amount;
                break;
            case ModifyType.Set:
                currentHealth = amount;
                break;
            case ModifyType.Divide:
                currentHealth /= amount;
                break;
            case ModifyType.Multiply:
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
            damageable.ModifyHealth(ModifyType.Subtract, currentHealth, team);
            ModifyHealth(ModifyType.Subtract, damageable.CurrentHealth, -1);
        }
    }

    private void Update()
    {
        if (!initialized) return;
        if (target == null) return;

        transform.position = Vector2.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
    }
}
