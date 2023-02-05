using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnscriptedLogic.MathUtils;

public class MinerNode : TurretNode, IResourceObtainer
{
    [Header("Miner Class")]
    [SerializeField] private int resourceMined;
    [SerializeField] private float mineInterval;
    [SerializeField] private Image fillImage;

    private float _mineInterval;
    private float currentMax;

    public override void Start()
    {
        base.Start();

        _mineInterval = mineInterval;
        currentMax = mineInterval + GlobalStatManager.instance.MinerSpeedReduction;
    }

    protected override void Update()
    {
        if (_mineInterval <= 0f)
        {
            CurrencyManager.instance.ModifyCash(MathLogic.ModificationType.Add, resourceMined + GlobalStatManager.instance.MinerAmount);
            currentMax = mineInterval + GlobalStatManager.instance.MinerSpeedReduction;
            _mineInterval = currentMax;
        }

        _mineInterval -= Time.deltaTime;

        float percent = _mineInterval / currentMax;
        fillImage.fillAmount = 1 - percent;

        if (GlobalStatManager.instance.minersCanShoot)
        {
            base.Update();
        }
    }

    public override string GetInspectDesc()
    {
        string desc = nodeSO.name;
        desc += $"\nHP: {currentHealth}/{health}";
        desc += $"\nAMT: {resourceMined}";
        desc += $"\nRATE: {1 / mineInterval}s";

        if (GlobalStatManager.instance.minersCanShoot)
        {
            desc += $"\nDMG: {damage}";
            desc += $"\nRNG: {range}";
            desc += $"\nRATE: {1 / interval}s";
        }

        return desc;
    }
}
