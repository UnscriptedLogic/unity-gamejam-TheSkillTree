using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStatManager : MonoBehaviour
{
    private int turretDamage;
    private float turretIntervalReduction;
    private int storageUnit;

    public int TurretDamage => turretDamage;
    public float TurretAttackInterval => turretIntervalReduction;
    public float TurretRange { get; private set; }
    public float ProjectileSpeed { get; private set; }

    public int StorageUnit => storageUnit;
    public int MinerAmount { get; private set; }
    public float MinerSpeedReduction { get; private set; }

    public bool minersCanShoot { get; private set; }
    public bool criticalShots { get; private set; }

    public Action<Node> OnNodeDestroyed;

    public static GlobalStatManager instance;

    public void Awake()
    {
        instance = this;
    }

    public void NodeDamaged(Node node)
    {
        OnNodeDestroyed?.Invoke(node);
    }

    //Turret
    public void AddDamage(int value) => turretDamage += value;
    public void ReduceAttackInterval(float value) => turretIntervalReduction -= value;
    public void AddRange(float amount) => TurretRange += amount;
    public void AddProjectileSpeed(float amount) => ProjectileSpeed += amount;
    public void GGEZ() => criticalShots = true;

    //Storage
    public void AddStorageUnit(int value) => storageUnit += value;

    //Miner
    public void OffensiveDefense() => minersCanShoot = true;
    public void AddMinerAmount(int value) => MinerAmount += value;
    public void ReduceMinerSpeed(float value) => MinerSpeedReduction -= value;
    public void Overclock(int value) 
    {
        MinerSpeedReduction -= 2f;
        AddMinerAmount(value);
    }
}
