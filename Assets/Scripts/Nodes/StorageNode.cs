using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageNode : Node, ICanBuildOff
{
    [Header("Storage Class")]
    [SerializeField] protected int maxConnections = 4;
    [SerializeField] protected int connections;
    [SerializeField] protected NodeSO[] buildables;

    public NodeSO[] Buildables => buildables;

    public int MaxConnections => maxConnections;
    public int Connections { get => connections; set => connections = value; }

    public override string GetInspectDesc()
    {
        string desc = base.GetInspectDesc();
        desc += $"\nCONN: {connections}/{maxConnections}";

        return desc;
    }
}
