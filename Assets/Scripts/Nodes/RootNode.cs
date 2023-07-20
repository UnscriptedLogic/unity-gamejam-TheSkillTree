using External.CustomSlider;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
using UnscriptedLogic;

public class RootNode : Node, ICanBuildOff, IResourceObtainer
{
    [SerializeField] private SceneChanger changer;
    [SerializeField] private NodeSO[] buildables;

    [SerializeField] private int connections;
    [SerializeField] private int maxConnections;

    public int Connections { get => connections; set => connections = value; }
    public int MaxConnections => maxConnections;

    [SerializeField] private int resourceMined;
    [SerializeField] private float interval;
    [SerializeField] private Image fillImage;

    private float _interval;
    private float currentMax;
    public NodeSO[] Buildables => buildables;

    public override void Start()
    {
        base.Start();

        _interval = interval;
    }

    private void Update()
    {
        if (_interval <= 0f)
        {
            CurrencyManager.instance.CurrencyHandler.Modify(ModifyType.Add, resourceMined + GlobalStatManager.instance.MinerAmount);
            currentMax = interval + GlobalStatManager.instance.MinerSpeedReduction;
            _interval = currentMax;
        }

        _interval -= Time.deltaTime;

        float percent = _interval / currentMax;
        fillImage.fillAmount = 1 - percent;
    }

    protected override void OnNodeDamaged(Node node)
    {

    }

    public override void ModifyHealth(ModifyType modificationType, float amount, int team)
    {
        base.ModifyHealth(modificationType, amount, team);

        if (currentHealth <= 0f)
        {
            GameManager.hasLost = true;
            changer.ChangeScene(0);
        }
    }

    public override string GetInspectDesc()
    {
        string desc = base.GetInspectDesc();

        desc += $"\nCONN: {connections}/{maxConnections}";

        return desc;
    }
}
