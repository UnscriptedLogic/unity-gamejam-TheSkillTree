using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretNode : Node
{
    [Header("Turret Class")]
    [SerializeField] protected float damage;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float interval;
    [SerializeField] protected float range;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected LayerMask unitLayer;

    protected List<Upgrade> upgrades;
    protected float _interval;

    public float Damage => damage;
    public float Interval => interval;
    public float Range => range;
    public List<Upgrade> Upgrades => upgrades;

    public override void Start()
    {
        base.Start();

        upgrades = new List<Upgrade>();

        Upgrade attackUpgrade = new Upgrade(5, node =>
        {
            TurretNode turret = (TurretNode)node;
            turret.SetDamage(turret.Damage + 1);
        });

        upgrades.Add(attackUpgrade);
    }

    protected virtual void Update()
    {
        if (_interval <= 0f)
        {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, range + GlobalStatManager.instance.TurretRange, unitLayer);

            if (collider == null) return;
            if (!collider.TryGetComponent(out IDamageable damageable)) return;

            Projectile projectile = Instantiate(projectilePrefab, transform.position, transform.rotation).GetComponent<Projectile>();
            projectile.Init(collider.transform, team, projectileSpeed + GlobalStatManager.instance.ProjectileSpeed, damage + GlobalStatManager.instance.TurretDamage);

            _interval = interval + GlobalStatManager.instance.TurretAttackInterval;
        }

        _interval -= Time.deltaTime;
    }

    public void SetDamage(float damage) => this.damage = damage;
    public void SetInterval(float interval) => this.interval = interval;
    public void SetRange(float range) => this.range = range;

    public override string GetInspectDesc()
    {
        string desc = base.GetInspectDesc();
        desc += $"\nDMG: {damage + GlobalStatManager.instance.TurretDamage}";
        desc += $"\nRNG: {range + GlobalStatManager.instance.TurretRange}";
        desc += $"\nRATE: {1 / interval + GlobalStatManager.instance.TurretAttackInterval}s";

        return desc;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
