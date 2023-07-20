using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic;

public interface IDamageable
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    int Team { get; }

    void ModifyHealth(ModifyType modificationType, float amount, int team);
}
