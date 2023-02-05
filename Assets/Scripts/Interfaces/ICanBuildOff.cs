using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanBuildOff
{
    int Connections { get; set; }
    int MaxConnections { get; }

    NodeSO[] Buildables { get; }
}
