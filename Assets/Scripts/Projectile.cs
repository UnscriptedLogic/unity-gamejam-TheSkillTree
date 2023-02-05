using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.MathUtils;

[RequireComponent(typeof(CircleCollider2D))]
public class Projectile : MonoBehaviour
{
    protected float speed;
    protected float damage;
    [SerializeField] protected Rigidbody2D rb;

    protected int team;
    protected Transform target;
    protected bool initialized;
    protected Vector2 direction;

    public void Init(Transform target, int team, float speed, float damage)
    {
        this.speed = speed;
        this.damage = damage;
        this.team = team;
        this.target = target;
        initialized = true;
    }

    private void Update()
    {
        if (!initialized) return;

        if (target != null)
            direction = (Vector2)(target.position - transform.position);
        else
            Destroy(gameObject, 5f);

        rb.MovePosition((Vector2)transform.position + (direction.normalized * speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            if (damageable.Team == team) return;

            float unitdamage = damage;

            if (GlobalStatManager.instance.criticalShots)
            {
                if (MathLogic.MathHelper.RandomFromIntZeroTo(10) == 5)
                {
                    unitdamage *= 2f;
                }
            }

            damageable.ModifyHealth(MathLogic.ModificationType.Subtract, unitdamage, team);
            Destroy(gameObject);
        }
    }
}
